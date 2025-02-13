using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
                    DatabaseRestore.Program.OptionsClass options = new DatabaseRestore.Program.OptionsClass()
                    {
                        SQLServerName = txtSQLServerName.Text.Trim(),
                        ServerPort = (int)nudSQLPort.Value,
                        SSPI = !chkSQLUseCredentials.Checked,
                        SQLUsername = txtSQLUsername.Text.Trim(),
                        SQLPassword = txtSQLPassword.Text,
                        DatabaseName = "",
                    };
                    List<string> databases = DatabaseRestore.Program.GetUserDatabases(options);
                    if (databases.Count == 0)
                    {
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
                    MessageBox.Show("Couldn't get a list of databases. Exception: \r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    DatabaseRestore.Program.OptionsClass options = new DatabaseRestore.Program.OptionsClass()
                    {
                        SQLServerName = txtSQLServerName.Text.Trim(),
                        ServerPort = (int)nudSQLPort.Value,
                        SSPI = !chkSQLUseCredentials.Checked,
                        SQLUsername = txtSQLUsername.Text.Trim(),
                        SQLPassword = txtSQLPassword.Text,
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
                    MessageBox.Show(string.Format("Couldn't get a list of database files. Exception: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    DatabaseRestore.Program.OptionsClass options = new DatabaseRestore.Program.OptionsClass()
                    {
                        SQLServerName = txtSQLServerName.Text.Trim(),
                        ServerPort = (int)nudSQLPort.Value,
                        SSPI = !chkSQLUseCredentials.Checked,
                        SQLUsername = txtSQLUsername.Text.Trim(),
                        SQLPassword = txtSQLPassword.Text,
                        DatabaseName = cmbSQLDatabaseName.Text
                    };
                    List<string> logins = DatabaseRestore.Program.GetSQLLogins(options);
                    RightsUsers.Clear();
                    foreach (var m in logins)
                    {
                        RightsUsers.Rows.Add(new[] { m });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Couldn't get a list of database files. Exception: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chkRightsEnable_CheckedChanged(object sender, EventArgs e)
        {
            cmdRightsImportLogins.Visible = chkRightsEnable.Checked;
            dgvRightsUsers.Visible = chkRightsEnable.Checked;
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
            if (chkSMTPAuth.Checked && (string.IsNullOrWhiteSpace(txtSMTPUser.Text) || string.IsNullOrWhiteSpace(txtSMTPPass.Text)))
            {
                MessageBox.Show("Missing SMTP Credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (!DatabaseRestore.Program.SaveSMTPProfile(profile, saveFileDialog.FileName))
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
                    DatabaseRestore.Program.SMTPProfileClass profile = DatabaseRestore.Program.LoadSMTPProfile(openFileDialog.FileName);
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
    }
}
