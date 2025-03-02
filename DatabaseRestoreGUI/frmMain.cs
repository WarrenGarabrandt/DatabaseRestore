/* The MIT License
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), 
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
 * IN THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DatabaseRestoreGUI
{
    public partial class frmMain : Form
    {
        private DataTable OptsMoveFile = new DataTable();
        private DataTable RightsUsers = new DataTable();
        public frmMain()
        {
            InitializeComponent();
            lbl1Version.Text = string.Format("Version {0}", DatabaseRestore.Program.ProgramVersionString);
            OptsMoveFile.Columns.Add(new DataColumn("LogicalName", typeof(string)));
            OptsMoveFile.Columns.Add(new DataColumn("OriginalPath", typeof(string)));
            OptsMoveFile.Columns.Add(new DataColumn("OverriddenPath", typeof(string)));
            dgvOptsMoveFile.DataSource = OptsMoveFile;
            RightsUsers.Columns.Add(new DataColumn("LoginName", typeof(string)));
            RightsUsers.Columns.Add(new DataColumn("db_datareader", typeof(bool)));
            RightsUsers.Columns.Add(new DataColumn("db_datawriter", typeof(bool)));
            RightsUsers.Columns.Add(new DataColumn("db_owner", typeof(bool)));
            dgvRightsUsers.DataSource = RightsUsers;
            dgvRightsUsers.Columns[1].Width = 100;
            dgvRightsUsers.Columns[2].Width = 100;
            dgvRightsUsers.Columns[3].Width = 100;
            dgvRightsUsers.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Panels = new[] { pnl0Start, pnl1Source, pnl2SQL, pnl3Opts, pnl4Rights, pnl5Log, pnl6SMTP, pnl7CLI, pnl8Run };
            Buttons = new[] { btn0Start, btn1Soruce, btn2SQL, btn3Opts, btn4Rights, btn5Log, btn6SMTP, btn7CLI, btn8Run }; 
            SetActivePage(Pages.Start);
        }

        private enum Pages
        {
            Start = 0,
            Source = 1,
            SQLConnection = 2,
            RestoreOptions = 3,
            DatabaseRights = 4,
            Logging = 5,
            SMTPOptions = 6,
            CLIArguments = 7,
            Run = 8
        }

        private Panel[] Panels;
        private CheckBox[] Buttons;

        private void SetActivePage(Pages page)
        {
            foreach (var pan in Panels)
            {
                pan.Visible = (string)pan.Tag == page.ToString();
            }
            foreach (var btn in Buttons)
            {
                btn.Checked = (string)btn.Tag == page.ToString();
            }

        }

        private void btn_Click(object sender, EventArgs e)
        {
            Pages page;
            if (Enum.TryParse((string)((CheckBox)sender).Tag, out page))
            {
                SetActivePage(page);
            }
        }

        private void cmdSourceAutoBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtSourceAutoPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void cmdSourceManualBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Filter = "SQL Server Backup Files (*.BAK)|*.BAK|All Files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtSourceFile.Text = dialog.FileName;
                }
            }
        }

        private void pnl1Source_VisibleChanged(object sender, EventArgs e)
        {
            if (pnl1Source.Visible == true)
            {
                if (cmbSourceSelectionMode.SelectedIndex == -1)
                {
                    cmbSourceSelectionMode.SelectedIndex = 0;
                }
            }
        }

        private void cmbSourceSelectionMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSourceSelectionMode.SelectedIndex == 0)
            {
                pnlSourceAutoSelect.Visible = true;
                pnlSourceManualSelect.Visible = false;
            }
            else if (cmbSourceSelectionMode.SelectedIndex == 1)
            {
                pnlSourceAutoSelect.Visible = false;
                pnlSourceManualSelect.Visible = true;
            }
        }

        private void pnlSourceAutoSelect_VisibleChanged(object sender, EventArgs e)
        {
            if (pnlSourceAutoSelect.Visible == true)
            {
                if (cmbAutoSourceAttribute.SelectedIndex == -1)
                {
                    cmbAutoSourceAttribute.SelectedIndex = 0;
                }
            }
        }

        private void chkSourceTempEnable_CheckedChanged(object sender, EventArgs e)
        {
            txtSourceTempFilePath.Visible = chkSourceTempEnable.Checked;
            cmdSourceTempFileBrowse.Visible = chkSourceTempEnable.Checked;
            lblSourceTempHelp.Visible = chkSourceTempEnable.Checked;
        }

        private void cmdSourceTempFileBrowse_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "BAK";
                dialog.Filter = "SQL Server Backups (*.BAK)|*.BAK";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtSourceTempFilePath.Text = dialog.FileName;
                }
            }
        }

        private void cmdSQLServerNameHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter the SQL Server Name, IP Address, or Name\\Instance.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdSQLDatabaseNameHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter the Database name to be created or overwriten.\r\nYou can click Query to get a list of current databases that exist.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdSQLDatabaseNameQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSQLServerName.Text) || 
                chkSQLUseCredentials.Checked && (string.IsNullOrWhiteSpace(txtSQLUsername.Text) || string.IsNullOrWhiteSpace(txtSQLPassword.Text)))
            {
                MessageBox.Show("Enter the SQL server name, and if the \"Specify Username and Password\" checkbox is checked, enter the Username and Password as well.", 
                    "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Connect to the SQL server now and get a list of existing databases?", "Query Databases", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    DatabaseRestore.Program.OptionsClass options = new DatabaseRestore.Program.OptionsClass()
                    {
                        SQLServerName = txtSQLServerName.Text.Trim(),
                        ServerPort = (int)nudSQLPort.Value,
                        SSPI = !chkSQLUseCredentials.Checked,
                        SQLUsername = txtSQLUsername.Text.Trim(),
                        SQLPassword = txtSQLPassword.Text,
                        EncryptSQL = cmbSQLEncryptionMode.SelectedIndex == 1,
                        TrustServerCert = chkSQLTrustCert.Checked,
                        DatabaseName = "",
                    };
                    List<string> databases = DatabaseRestore.Program.GetUserDatabases(options);
                    if (databases.Count == 0)
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Connection succeeded, but there are no user databases on that SQL server. Please type the database name instead.", 
                            "No Databases", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbSQLDatabaseName.Items.Clear();
                        return;
                    }
                    cmbSQLDatabaseName.Items.Clear();
                    foreach (var db in databases)
                    {
                        cmbSQLDatabaseName.Items.Add(db);
                    }
                    cmbSQLDatabaseName.DroppedDown = true;
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Couldn't get a list of databases. Exception: \r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void chkSQLUseCredentials_CheckedChanged(object sender, EventArgs e)
        {
            lblSQLUsername.Visible = chkSQLUseCredentials.Checked;
            txtSQLUsername.Visible = chkSQLUseCredentials.Checked;
            lblSQLPassword.Visible = chkSQLUseCredentials.Checked;
            txtSQLPassword.Visible = chkSQLUseCredentials.Checked;
            lblSQLCredentialInfo.Visible = chkSQLUseCredentials.Checked;

        }

        private void cmdOptsMoveAllPathHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This must be a path on the SQL server that is storing the database.\r\n" +
                "Unless this program is being run on the SQL server, the browse button will likely not be useful.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chkOptsMoveall_CheckedChanged(object sender, EventArgs e)
        {
            lblOptsMoveallPath.Visible = chkOptsMoveall.Checked;
            txtOptsMoveallPath.Visible= chkOptsMoveall.Checked;
            cmdOptsMoveAllBrowse.Visible = chkOptsMoveall.Checked;
            cmdOptsMoveAllPathHelp.Visible = chkOptsMoveall.Checked;

        }

        private void chkOptsMoveFile_CheckedChanged(object sender, EventArgs e)
        {
            cmdOptsMoveFileImport.Visible = chkOptsMoveFile.Checked;
            dgvOptsMoveFile.Visible = chkOptsMoveFile.Checked;
        }
        
        private void cmdOptsMoveFileImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSQLServerName.Text) ||
                chkSQLUseCredentials.Checked && (string.IsNullOrWhiteSpace(txtSQLUsername.Text) || string.IsNullOrWhiteSpace(txtSQLPassword.Text)) ||
                string.IsNullOrWhiteSpace(cmbSQLDatabaseName.Text))
            {
                MessageBox.Show("On the SQL Connection page, enter the SQL server name, and if the \"Specify Username and Password\" checkbox is checked, enter the Username and Password as well. Then enter or select the Database name you want to query.",
                    "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Connect to SQL server now and query the database files for the entered database (SQL Connection page)?\r\n" + 
                "The database must exist and be connectable. This will clear the below grid and replace it with files from the database.",
                "Query Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    DatabaseRestore.Program.OptionsClass options = new DatabaseRestore.Program.OptionsClass()
                    {
                        SQLServerName = txtSQLServerName.Text.Trim(),
                        ServerPort = (int)nudSQLPort.Value,
                        SSPI = !chkSQLUseCredentials.Checked,
                        SQLUsername = txtSQLUsername.Text.Trim(),
                        SQLPassword = txtSQLPassword.Text,
                        EncryptSQL = cmbSQLEncryptionMode.SelectedIndex == 1,
                        TrustServerCert = chkSQLTrustCert.Checked,
                        DatabaseName = cmbSQLDatabaseName.Text
                    };
                    List<DatabaseRestore.Program.MoveItem> MoveItems = DatabaseRestore.Program.GetDatabaseFiles(options);

                    OptsMoveFile.Clear();
                    foreach (var m in MoveItems)
                    {
                        OptsMoveFile.Rows.Add(new[] { m.LogicalName, m.PhysicalName, m.PhysicalName });
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(string.Format("Couldn't get a list of database files. Exception: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void cmdOptsMoveAllBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtOptsMoveallPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void cmdRightsImportLogins_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSQLServerName.Text) ||
                chkSQLUseCredentials.Checked && (string.IsNullOrWhiteSpace(txtSQLUsername.Text) || string.IsNullOrWhiteSpace(txtSQLPassword.Text)))
            {
                MessageBox.Show("On the SQL Connection page, enter the SQL server name, and if the \"Specify Username and Password\" checkbox is checked, enter the Username and Password as well.",
                    "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Connect to SQL server now and query the logins? This won't populate role permissions, only the login names.\r\n" +
                "The SQL server must be connectable. This will clear the below grid and replace it with logins from SQL Server.",
                "Query Logins", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    DatabaseRestore.Program.OptionsClass options = new DatabaseRestore.Program.OptionsClass()
                    {
                        SQLServerName = txtSQLServerName.Text.Trim(),
                        ServerPort = (int)nudSQLPort.Value,
                        SSPI = !chkSQLUseCredentials.Checked,
                        SQLUsername = txtSQLUsername.Text.Trim(),
                        SQLPassword = txtSQLPassword.Text,
                        EncryptSQL = cmbSQLEncryptionMode.SelectedIndex == 1,
                        TrustServerCert = chkSQLTrustCert.Checked,
                        DatabaseName = cmbSQLDatabaseName.Text
                    };
                    List<string> logins = DatabaseRestore.Program.GetSQLLogins(options);
                    RightsUsers.Clear();
                    foreach (var m in logins)
                    {
                        RightsUsers.Rows.Add(new object[] { m, false, false, false });
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(string.Format("Couldn't get a list of database files. Exception: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void chkRightsEnable_CheckedChanged(object sender, EventArgs e)
        {
            cmdRightsImportLogins.Visible = chkRightsEnable.Checked;
            dgvRightsUsers.Visible = chkRightsEnable.Checked;
            lblRightsHelp.Visible = chkRightsEnable.Checked;
        }

        private void cmdLogFileBrowse_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.DefaultExt = "TXT";
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.Filter = "Text Files (*.txt)|*.TXT";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtLogFile.Text = saveFileDialog.FileName;
                }
            }
        }

        private void chkLogEnable_CheckedChanged(object sender, EventArgs e)
        {
            lblLogFile.Visible = chkLogEnable.Checked;
            txtLogFile.Visible = chkLogEnable.Checked;
            cmdLogFileBrowse.Visible = chkLogEnable.Checked;
        }

        private void cmdLogAppendBrowse_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.DefaultExt = "TXT";
                saveFileDialog.Filter = "Text Files (*.txt)|*.TXT";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtLogFile.Text = saveFileDialog.FileName;
                }
            }
        }

        private void chkLogAppendEnable_CheckedChanged(object sender, EventArgs e)
        {
            lblLogAppendFile.Visible = chkLogAppendEnable.Checked;
            txtLogAppendFile.Visible = chkLogAppendEnable.Checked;
            cmdLogAppendBrowse.Visible = chkLogAppendEnable.Checked;
        }

        private void chkSMTPSendEmail_CheckedChanged(object sender, EventArgs e)
        {
            lblSMTPProfile.Visible = chkSMTPSendEmail.Checked;
            txtSMTPProfile.Visible = chkSMTPSendEmail.Checked;
            lblSMTPProfilePass.Visible = chkSMTPSendEmail.Checked;
            txtSMTPProfilePass.Visible = chkSMTPSendEmail.Checked;
            cmdSMTPProfileBrowse.Visible = chkSMTPSendEmail.Checked;
        }

        private void chkSMTPAuth_CheckedChanged(object sender, EventArgs e)
        {
            lblSMTPUser.Visible = chkSMTPAuth.Checked;
            txtSMTPUser.Visible = chkSMTPAuth.Checked;
            lblSMTPPass.Visible = chkSMTPAuth.Checked;
            txtSMTPPass.Visible = chkSMTPAuth.Checked;
            lblSMTPPassWarn.Visible = chkSMTPAuth.Checked;
        }

        private void cmdSMTPSaveProfile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSMTPServer.Text))
            {
                MessageBox.Show("Missing SMTP Server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DatabaseRestore.Program.SMTPProfileClass profile = new DatabaseRestore.Program.SMTPProfileClass()
            {
                SMTPServer = txtSMTPServer.Text.Trim(),
                Port = (int)nudSMTPPort.Value,
                TLS = chkSMTPTLS.Checked,
                RequireAuth = chkSMTPAuth.Checked,
                UserName = chkSMTPAuth.Checked ? txtSMTPUser.Text.Trim() : "",
                Password = chkSMTPAuth.Checked ? txtSMTPPass.Text.Trim() : "",
                EmailFrom = txtSMTPEmailFrom.Text.Trim(),
                EmailTo = txtSMTPEmailTo.Text.Trim(),
                EmailSubjectTemplate = txtSMTPSubject.Text,
                EmailBodyTemplte = txtSMTPBody.Text,
                AttachLog = chkSMTPAttachLog.Checked
            };

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "SMTP Profile (*.ESF)|*.ESF";
                saveFileDialog.DefaultExt = "ESF";
                saveFileDialog.OverwritePrompt = true;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    frmPassword pass = new frmPassword();
                    if (pass.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    if (!DatabaseRestore.Program.SaveSMTPProfile(profile, saveFileDialog.FileName, pass.txtPass1.Text))
                    {
                        MessageBox.Show("There was an error saving the profile.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void cmdSMTPLoadProfile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SMTP Profile (*.ESF)|*.ESF";
                openFileDialog.CheckFileExists = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    frmPassword pass = new frmPassword();
                    pass.CreateNewPassword = false;
                    if (pass.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    DatabaseRestore.Program.SMTPProfileClass profile = DatabaseRestore.Program.LoadSMTPProfile(openFileDialog.FileName, pass.txtPass1.Text);
                    if (profile != null)
                    {
                        // populate UI controls
                        txtSMTPServer.Text = profile.SMTPServer;
                        nudSMTPPort.Value = profile.Port > 0 && profile.Port <= 65535 ? profile.Port : 25;
                        chkSMTPAuth.Checked = profile.RequireAuth;
                        txtSMTPUser.Text = profile.UserName;
                        txtSMTPPass.Text = profile.Password;
                        txtSMTPEmailFrom.Text = profile.EmailFrom;
                        txtSMTPEmailTo.Text = profile.EmailTo;
                        txtSMTPSubject.Text = profile.EmailSubjectTemplate;
                        txtSMTPBody.Text = profile.EmailBodyTemplte;
                        chkSMTPAttachLog.Checked = profile.AttachLog;
                    }
                    else
                    {
                        MessageBox.Show("There was an error loading the profile.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private bool ValidateSettings()
        {
            if (cmbSourceSelectionMode.SelectedIndex == 0 && string.IsNullOrWhiteSpace(txtSourceAutoPath.Text))
            {
                MessageBox.Show("On the Source page, enter a path where the backup file can be located.", "Missing Source Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (cmbSourceSelectionMode.SelectedIndex == 1 && string.IsNullOrWhiteSpace(txtSourceFile.Text))
            {
                MessageBox.Show("On the Source page, enter a backup file to restore.", "Missing Source Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (chkSourceTempEnable.Checked && string.IsNullOrWhiteSpace(txtSourceTempFilePath.Text))
            {
                MessageBox.Show("On the Source page, the Copy Backup to Temp checkbox is checked, but no path is entered for the copy.", 
                    "Missing Source Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (chkOptsMoveall.Checked && string.IsNullOrWhiteSpace(txtOptsMoveallPath.Text))
            {
                MessageBox.Show("On the Restore Options page, the Move all backup files checkbox is checked, but no path is entered.",
                    "Missing Options Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (chkOptsMoveFile.Checked && OptsMoveFile.Rows.Count == 0)
            {
                MessageBox.Show("On the Restore Options page, the Move backup files checkbox is checked, but no database files are entered.",
                    "Missing Options Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (chkOptsMoveFile.Checked && OptsMoveFile.Rows.Count > 0)
            {
                foreach (DataRow row in OptsMoveFile.Rows)
                {
                    if (string.IsNullOrWhiteSpace((string)row[0]) || string.IsNullOrWhiteSpace((string)row[2]))
                    {
                        MessageBox.Show("On the Restore Options page, make sure that all files to move have a logical name and new path entered.",
                            "Missing Options Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            if (string.IsNullOrEmpty(txtSQLServerName.Text) || string.IsNullOrEmpty(cmbSQLDatabaseName.Text))
            {
                MessageBox.Show("On the SQL Connection page, enter a server name and database name.", "Missing SQL Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (chkSQLUseCredentials.Checked && (string.IsNullOrWhiteSpace(txtSQLUsername.Text) || string.IsNullOrEmpty(txtSQLPassword.Text)))
            {
                MessageBox.Show("On the SQL Connection page, the Specify Username and Password checkbox is check, but required username and password has not been entered. \r\n" +
                    "Enter Uncheck that to use integrated authentication, or enter a username and password.", "Missing SQL Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if ((chkLogEnable.Checked && string.IsNullOrWhiteSpace(txtLogFile.Text)) ||
                (chkLogAppendEnable.Checked && string.IsNullOrWhiteSpace(txtLogAppendFile.Text)))
            {
                MessageBox.Show("On the Logging page, you must specify a log file for any enabled Logging methods.", "Missing Logging Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (chkSMTPSendEmail.Checked && string.IsNullOrWhiteSpace(txtSMTPProfile.Text))
            {
                MessageBox.Show("On the SMTP page, the Enable SMTP Profile is checked but required profile has not been specified.",
                    "Missing SMTP Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void cmdStartLoadSettings_Click(object sender, EventArgs e)
        {
            DatabaseRestore.Program.OptionsClass opts = null;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "XML Settings File (*.XSF)|*.XSF";
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    frmPassword pass = new frmPassword();
                    pass.CreateNewPassword = false;
                    if (pass.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    opts = DatabaseRestore.Program.LoadOptionsFile(dialog.FileName, pass.txtPass1.Text);
                    if (opts == null)
                    {
                        MessageBox.Show("Unable to load the settings file.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            // apply settintgs

            ApplyOptionsFile(opts);

        }

        private void ApplyOptionsFile(DatabaseRestore.Program.OptionsClass opts)
        {
            // source page
            if (opts.AutoSourceMode == DatabaseRestore.Program.AutoSourceMode.None)
            {
                cmbSourceSelectionMode.SelectedIndex = 1;
                txtSourceFile.Text = opts.SourceFile;
                txtSourceAutoPath.Text = "";
                cmbAutoSourceAttribute.SelectedIndex = 0;
            }
            else
            {
                cmbSourceSelectionMode.SelectedIndex = 0;
                txtSourceAutoPath.Text = opts.SourcePath;
                txtSourceFile.Text = "";
                if (opts.AutoSourceMode == DatabaseRestore.Program.AutoSourceMode.lastcreated)
                {
                    cmbAutoSourceAttribute.SelectedIndex = 0;
                }
                else
                {
                    cmbAutoSourceAttribute.SelectedIndex = 1;
                }
            }
            chkSourceTempEnable.Checked = !string.IsNullOrWhiteSpace(opts.TempFile);
            txtSourceTempFilePath.Text = opts.TempFile;
            
            // sql connection page
            txtSQLServerName.Text = opts.SQLServerName;
            if (opts.ServerPort < 1 || opts.ServerPort > 65535)
            {
                nudSQLPort.Value = 1433;
            }
            else
            {
                nudSQLPort.Value = opts.ServerPort;
            }
            if (opts.SSPI)
            {
                chkSQLUseCredentials.Checked = false;
                txtSQLUsername.Text = "";
                txtSQLPassword.Text = "";
            }
            else
            {
                chkSQLUseCredentials.Checked = true;
                txtSQLUsername.Text = opts.SQLUsername;
                txtSQLPassword.Text = opts.SQLPassword;
            }
            if (opts.EncryptSQL)
            {
                cmbSQLEncryptionMode.SelectedIndex = 1;
            }
            else
            {
                cmbSQLEncryptionMode.SelectedIndex = 0;
            }
            chkSQLTrustCert.Checked = opts.TrustServerCert;
            cmbSQLDatabaseName.Text = opts.DatabaseName;

            // restore options page
            chkOptsReplace.Checked = opts.ReplaceDatabase;
            chkOptsCheckdb.Checked = opts.DbccCheckDB;
            chkOptsMoveall.Checked = opts.MoveAllFiles;
            chkOptsCloseConnections.Checked = opts.CloseConnections;
            if (opts.MoveAllFiles)
            {
                txtOptsMoveallPath.Text = opts.MoveAllPath;
            }
            else
            {
                txtOptsMoveallPath.Text = "";
            }
            OptsMoveFile.Clear();
            if (opts.MoveItems.Count > 0)
            {
                chkOptsMoveFile.Checked = true;
                foreach (var item in opts.MoveItems)
                {
                    OptsMoveFile.Rows.Add(new[] { item.LogicalName, "", item.PhysicalName });
                }
            }
            else
            {
                chkOptsMoveFile.Checked = false;
            }

            RightsUsers.Clear();
            // rights page
            if (opts.UserRights.Count > 0)
            {
                chkRightsEnable.Checked = true;
                foreach (var item in opts.UserRights)
                {
                    RightsUsers.Rows.Add(new object[] { item.Name, item.Read, item.Write, item.Owner });
                }
            }
            else
            {
                chkRightsEnable.Checked = false;
            }

            // loggin page
            if (!string.IsNullOrEmpty(opts.LogFile))
            {
                chkLogEnable.Checked = true;
                txtLogFile.Text = opts.LogFile;
            }
            else
            {
                chkLogEnable.Checked = false;
                txtLogFile.Text = "";
            }
            if (!string.IsNullOrEmpty(opts.LogAppend))
            {
                chkLogAppendEnable.Checked = true;
                txtLogAppendFile.Text = opts.LogAppend;
            }
            else
            {
                chkLogAppendEnable.Checked = false;
                txtLogAppendFile.Text = "";
            }

            // smtp page
            if (!string.IsNullOrEmpty(opts.SMTPProfile))
            {
                chkSMTPSendEmail.Checked = true;
                txtSMTPProfile.Text = opts.SMTPProfile;
                txtSMTPProfilePass.Text = opts.SMTPPassword;
            }
            else
            {
                chkSMTPSendEmail.Checked = false;
                txtSMTPProfile.Text = "";
                txtSMTPProfilePass.Text = "";
            }
        }

        private DatabaseRestore.Program.OptionsClass BuildOptionsFile()
        {
            DatabaseRestore.Program.OptionsClass opts = new DatabaseRestore.Program.OptionsClass();

            // source page
            if (cmbSourceSelectionMode.SelectedIndex == 0)
            {
                opts.SourcePath = txtSourceAutoPath.Text.Trim();
                if (cmbAutoSourceAttribute.SelectedIndex == 0)
                {
                    opts.AutoSourceMode = DatabaseRestore.Program.AutoSourceMode.lastcreated;
                }
                else
                {
                    opts.AutoSourceMode = DatabaseRestore.Program.AutoSourceMode.lastmodified;
                }
            }
            else
            {
                opts.SourceFile = txtSourceFile.Text.Trim();
                opts.AutoSourceMode = DatabaseRestore.Program.AutoSourceMode.None;
            }
            if (chkSourceTempEnable.Checked)
            {
                opts.TempFile = txtSourceTempFilePath.Text.Trim();
            }

            // sql connection page
            opts.SQLServerName = txtSQLServerName.Text.Trim();
            opts.ServerPort = (int)nudSQLPort.Value;
            if (chkSQLUseCredentials.Checked)
            {
                opts.SSPI = false;
                opts.SQLUsername = txtSQLUsername.Text.Trim();
                opts.SQLPassword = txtSQLPassword.Text;
            }
            else
            {
                opts.SSPI = true;
            }

            opts.EncryptSQL = cmbSQLEncryptionMode.SelectedIndex == 1;
            opts.TrustServerCert = chkSQLTrustCert.Checked;
            opts.DatabaseName = cmbSQLDatabaseName.Text.Trim();

            // restore options page
            opts.ReplaceDatabase = chkOptsReplace.Checked;
            opts.DbccCheckDB = chkOptsCheckdb.Checked;
            opts.CloseConnections = chkOptsCloseConnections.Checked;
            opts.MoveAllFiles = chkOptsMoveall.Checked;
            if (chkOptsMoveall.Checked)
            {
                opts.MoveAllPath = txtOptsMoveallPath.Text.Trim();
            }
            if (chkOptsMoveFile.Checked)
            {
                List<DatabaseRestore.Program.MoveItem> moveList = new List<DatabaseRestore.Program.MoveItem>();
                foreach (DataRow row in OptsMoveFile.Rows)
                {
                    if (!string.IsNullOrWhiteSpace((string)row[0]) && !string.IsNullOrWhiteSpace((string)row[2]))
                    {
                        moveList.Add(new DatabaseRestore.Program.MoveItem()
                        {
                            LogicalName = (string)row[0],
                            PhysicalName = (string)row[2]
                        });
                    }
                }

                opts.MoveItems.AddRange(moveList);
            }

            // rights page
            if (chkRightsEnable.Checked)
            {
                List<DatabaseRestore.Program.UserRightItem> rights = new List<DatabaseRestore.Program.UserRightItem>();
                foreach (DataRow row in RightsUsers.Rows)
                {
                    if (!string.IsNullOrWhiteSpace((string)row[0]))
                    {
                        rights.Add(new DatabaseRestore.Program.UserRightItem()
                        {
                            Name = (string)row[0],
                            Read = row[1].Equals(true),
                            Write = row[2].Equals(true),
                            Owner = row[3].Equals(true),
                        });
                    }
                }
                opts.UserRights.AddRange(rights);
            }

            // logging page
            if (chkLogEnable.Checked)
            {
                opts.LogFile = txtLogFile.Text.Trim();
            }
            if (chkLogAppendEnable.Checked)
            {
                opts.LogAppend = txtLogAppendFile.Text.Trim();
            }

            // smtp page
            if (chkSMTPSendEmail.Checked)
            {
                opts.SMTPProfile = txtSMTPProfile.Text.Trim();
                opts.SMTPPassword = txtSMTPProfilePass.Text;
            }
            return opts;
        }

        private void cmdStartSaveSettings_Click(object sender, EventArgs e)
        {
            if (!ValidateSettings())
            {
                return;
            }
            string outputPath = null;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "XML Settings File (*.XSF)|*.XSF";
                dialog.DefaultExt = "XSF";
                dialog.AddExtension = true;
                if (dialog.ShowDialog()  == DialogResult.OK)
                {
                    outputPath = dialog.FileName;
                }
                else
                {
                    return;
                }
            }
            frmPassword pass = new frmPassword();
            if (pass.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DatabaseRestore.Program.OptionsClass opts = BuildOptionsFile();

            if (!DatabaseRestore.Program.SaveOptionsFile(opts, outputPath, pass.txtPass1.Text))
            {
                MessageBox.Show("Error occurred saving the settings file.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pnl7CLI_VisibleChanged(object sender, EventArgs e)
        {
            if (pnl7CLI.Visible)
            {
                StringBuilder sb = new StringBuilder();
                // build options for display
                if (cmbSourceSelectionMode.SelectedIndex == 0)
                {
                    // source mode: auto directory
                    string temp = EscapeArguments(txtSourceAutoPath.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "Error: no source path specified.";
                        return;
                    }
                    sb.Append("--autosource ");
                    if (cmbAutoSourceAttribute.SelectedIndex == 0)
                    {
                        sb.Append("lastcreated ");
                    }
                    else
                    {
                        sb.Append("lastmodified ");
                    }
                    sb.Append(temp);
                }
                else
                {
                    // source mode: file
                    string temp = EscapeArguments(txtSourceFile.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "Error: no source file specified.";
                        return;
                    }
                    sb.AppendFormat("--source {0}", temp);
                }
                if (chkSourceTempEnable.Checked)
                {
                    // temp file in use
                    string temp = EscapeArguments(txtSourceTempFilePath.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "Error: no temp file specified.";
                        return;
                    }
                    sb.AppendFormat(" --temp {0}", temp);
                }
                // sql connection options
                if (!string.IsNullOrWhiteSpace(txtSQLServerName.Text))
                {
                    string temp = EscapeArguments(txtSQLServerName.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "SQL server name is not valid.";
                        return;
                    }
                    sb.AppendFormat(" --servername {0}", temp);
                }
                else
                { 
                    txtCLIArgs.Text = "No SQL server specified.";
                    return;
                }
                if (nudSQLPort.Value != 1433)
                {
                    sb.AppendFormat(" --serverport {0}", (int)nudSQLPort.Value);
                }
                if (cmbSQLEncryptionMode.SelectedIndex == 1)
                {
                    sb.Append(" --encrypt");
                }
                if (chkSQLTrustCert.Checked)
                {
                    sb.Append(" --trustservercert");
                }
                if (!string.IsNullOrWhiteSpace(cmbSQLDatabaseName.Text))
                {
                    string temp = EscapeArguments(cmbSQLDatabaseName.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "SQL Database Name is not valid. (Some characters aren't valid to pass through command line arguments using this tool, but you may still be able to override this by escaping in double quotes on the command line manually.)";
                        return;
                    }
                    sb.AppendFormat(" --database {0}", temp);
                }
                else
                {
                    txtCLIArgs.Text = "No SQL Database specified.";
                    return;
                }
                if (chkSQLUseCredentials.Checked)
                {
                    string temp = EscapeArguments(txtSQLUsername.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "No SQL server credentials specified. It's recommended to use integrated authentication.";
                        return;
                    }
                    sb.AppendFormat(" --username {0}", temp);
                    temp = EscapeArguments(txtSQLPassword.Text);
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "No SQL server credentials specified. It's recommended to use integrated authentication.";
                        return;
                    }
                    sb.AppendFormat(" --password {0}", temp);
                }
                // restore options
                if (chkOptsReplace.Checked)
                {
                    sb.Append(" --replacedatabase");
                }
                if (chkOptsCloseConnections.Checked)
                {
                    sb.Append(" --closeconnections");
                }
                if (chkOptsCheckdb.Checked)
                {
                    sb.Append(" --dbcccheckdb");
                }
                if (chkOptsMoveall.Checked)
                {
                    string temp = EscapeArguments(txtOptsMoveallPath.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "Move All Files reqires a path to be specified.";
                        return;
                    }
                    sb.AppendFormat(" --moveallfiles {0}", temp);
                }
                if (chkOptsMoveFile.Checked)
                {
                    List<DatabaseRestore.Program.MoveItem> moveList = new List<DatabaseRestore.Program.MoveItem>();
                    foreach (DataRow row in OptsMoveFile.Rows)
                    {
                        if (!string.IsNullOrWhiteSpace((string)row[0]) && !string.IsNullOrWhiteSpace((string)row[2]))
                        {
                            moveList.Add(new DatabaseRestore.Program.MoveItem()
                            {
                                LogicalName = (string)row[0],
                                PhysicalName = (string)row[2]
                            });
                        }
                    }
                    foreach (var m in moveList)
                    {
                        string tempLogical = EscapeArguments(m.LogicalName.Trim());
                        string tempPhysical = EscapeArguments(m.PhysicalName.Trim());
                        if (string.IsNullOrEmpty(tempLogical) || string.IsNullOrEmpty(tempPhysical))
                        {
                            txtCLIArgs.Text = "Move database files requires a logical name and a physical path for each line.";
                            return;
                        }
                        sb.AppendFormat(" --movefile {0} {1}", tempLogical, tempPhysical);
                    }                    
                }

                // rights
                if (chkRightsEnable.Checked)
                {
                    sb.Append(" --rights ");
                    List<DatabaseRestore.Program.UserRightItem> rights = new List<DatabaseRestore.Program.UserRightItem>();
                    StringBuilder rightssb = new StringBuilder();
                    foreach (DataRow row in RightsUsers.Rows)
                    {
                        string tempUser = ((string)row[0]).Trim();
                        if (string.IsNullOrEmpty(tempUser) || tempUser.Contains(";") || tempUser.Contains(":"))
                        {
                            txtCLIArgs.Text = "On the Database Rights page, Username is required and can't contain a ; or a :";
                            return;
                        }
                        bool read = row[1].Equals(true);
                        bool write = row[2].Equals(true);
                        bool owner = row[3].Equals(true);
                        if (!(read || write || owner))
                        {
                            txtCLIArgs.Text = "On the Database Rights page, east listed user must have at least one role selected:";
                            return;
                        }
                        if (rightssb.Length > 0)
                        {
                            rightssb.Append(";");
                        }
                        rightssb.AppendFormat("{0}:{1}{2}{3}", tempUser, read ? "R" : "", write ? "W" : "", owner ? "O" : "");
                    }
                    string temp = EscapeArguments(rightssb.ToString());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "On the Database Rights page, Add SQL logins option require at least one user.";
                        return;
                    }
                    sb.Append(temp);
                }
                // log
                if (chkLogEnable.Checked)
                {
                    string temp = EscapeArguments(txtLogFile.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "Logging requires a file to be specified.";
                        return;
                    }
                    sb.AppendFormat(" --logfile {0}", temp);
                }
                if (chkLogAppendEnable.Checked)
                {
                    string temp = EscapeArguments(txtLogAppendFile.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "Append logging requires a file to be specified.";
                        return;
                    }
                    sb.AppendFormat(" --logappend {0}", temp);
                }
                //smtp
                if (chkSMTPSendEmail.Checked)
                {
                    string temp = EscapeArguments(txtSMTPProfile.Text.Trim());
                    if (string.IsNullOrEmpty(temp))
                    {
                        txtCLIArgs.Text = "SMTP Profile requires a file to be specified.";
                        return;
                    }
                    sb.AppendFormat(" --smtpprofile {0}", temp); 
                    if (!string.IsNullOrEmpty(txtSMTPProfilePass.Text))
                    {
                        temp = EscapeArguments(txtSMTPProfilePass.Text);
                        sb.AppendFormat(" --smtppassword {0}", temp);
                    }
                }

                txtCLIArgs.Text = sb.ToString();
            }
        }

        private void pnl2SQL_VisibleChanged(object sender, EventArgs e)
        {
            if (cmbSQLEncryptionMode.SelectedIndex == -1 )
            {
                cmbSQLEncryptionMode.SelectedIndex = 1;
            }
        }

        public string EscapeArguments(string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                return "";
            }
            char[] illegalchars = { '\0', '\r', '\n', '\t' };
            if (arg.Any(c => illegalchars.Contains(c)))
            {
                throw new Exception("Can't contain null, tab, or a new line.");
            } 
            //strip trailing \
            while (arg.Length > 0 && arg.EndsWith("\\"))
            {
                arg = arg.Substring(0, arg.Length - 1);
            }
            if (arg.Contains('\"') || arg.Contains(' '))
            {
                arg = arg.Replace("\"", "\\\"");
                arg = "\"" + arg + "\"";
            }
            return arg;
        }

        bool Running = false;
        BackgroundWorker DemandJobRunner = null;
        private void pnl8Run_VisibleChanged(object sender, EventArgs e)
        {
            if (!Running && pnl8Run.Visible)
            {
                lblRunBuildStatus.Text = "";
                cmdRunStart.Enabled = false;
                // check values and build job.
                if (!ValidateSettings())
                {
                    lblRunBuildStatus.Text = "Please check settings and come back to this page to retry checks.";
                    return;
                }
                lblRunBuildStatus.Text = "Initial checks complete, and job has been built.\r\nClick Run restore the backup now.";
                cmdRunStart.Enabled = true;
            }        
        }

        private void cmdRunStart_Click(object sender, EventArgs e)
        {
            if (Running)
            {
                cmdRunStart.Enabled = false;
                return;
            }
            if (DemandJobRunner == null)
            {
                DemandJobRunner = new BackgroundWorker();
                DemandJobRunner.RunWorkerCompleted += DemandJobRunner_RunWorkerCompleted;
                DemandJobRunner.DoWork += DemandJobRunner_DoWork;
            }
            cmdRunStart.Enabled = false;
            if (DemandJobRunner.IsBusy == false)
            {
                if (!ValidateSettings())
                {
                    lblRunBuildStatus.Text = "Please check settings and come back to this page to retry checks.";
                    return;
                }
                DatabaseRestore.Program.OptionsClass DemandRunOptions = BuildOptionsFile();
                Running = true;
                lblRunBuildStatus.Text = "Job is running. Please wait for completion, then see output below.";
                txtRunOutput.Text = "Please wait for log output.";
                DemandJobRunner.RunWorkerAsync(DemandRunOptions);
            }
        }

        private void DemandJobRunner_DoWork(object sender, DoWorkEventArgs e)
        {
            DatabaseRestore.Program.OptionsClass opts = e.Argument as DatabaseRestore.Program.OptionsClass;
            if (opts == null)
            {
                e.Result = "No demand job options provided.";
                return;
            }
            DatabaseRestore.Program.LogOutput.Clear();
            DatabaseRestore.Program.RunJob(opts);
            e.Result = DatabaseRestore.Program.LogOutput.ToString();
            return;
        }

        private void DemandJobRunner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is string s)
            {
                txtRunOutput.Text += s;
                txtRunOutput.SelectionStart = txtRunOutput.Text.Length;
                txtRunOutput.ScrollToCaret();
            }
            Running = false;
            lblRunBuildStatus.Text = "Job as stopped. See lob below for any errors.";
        }

        private void cmdSMTPProfileBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SMTP Profile (*.ESF)|*.ESF";
                openFileDialog.CheckFileExists = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtSMTPProfile.Text = openFileDialog.FileName;
                }

            }
        }
    }
}
