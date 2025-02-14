namespace DatabaseRestoreGUI
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnl0Start = new System.Windows.Forms.Panel();
            this.lbl1Version = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn0Start = new System.Windows.Forms.CheckBox();
            this.btn1Soruce = new System.Windows.Forms.CheckBox();
            this.pnl1Source = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSourceTempHelp = new System.Windows.Forms.Label();
            this.cmdSourceTempFileBrowse = new System.Windows.Forms.Button();
            this.txtSourceTempFilePath = new System.Windows.Forms.TextBox();
            this.chkSourceTempEnable = new System.Windows.Forms.CheckBox();
            this.cmbSourceSelectionMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlSourceAutoSelect = new System.Windows.Forms.Panel();
            this.cmbAutoSourceAttribute = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmdSourceAutoBrowse = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSourceAutoPath = new System.Windows.Forms.TextBox();
            this.pnlSourceManualSelect = new System.Windows.Forms.Panel();
            this.cmdSourceManualBrowse = new System.Windows.Forms.Button();
            this.txtSourceFile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn2SQL = new System.Windows.Forms.CheckBox();
            this.btn3Opts = new System.Windows.Forms.CheckBox();
            this.btn5Log = new System.Windows.Forms.CheckBox();
            this.btn6SMTP = new System.Windows.Forms.CheckBox();
            this.btn7CLI = new System.Windows.Forms.CheckBox();
            this.btn8Run = new System.Windows.Forms.CheckBox();
            this.pnl2SQL = new System.Windows.Forms.Panel();
            this.cmbSQLDatabaseName = new System.Windows.Forms.ComboBox();
            this.lblSQLCredentialInfo = new System.Windows.Forms.Label();
            this.txtSQLPassword = new System.Windows.Forms.TextBox();
            this.lblSQLPassword = new System.Windows.Forms.Label();
            this.txtSQLUsername = new System.Windows.Forms.TextBox();
            this.lblSQLUsername = new System.Windows.Forms.Label();
            this.chkSQLUseCredentials = new System.Windows.Forms.CheckBox();
            this.cmdSQLDatabaseNameQuery = new System.Windows.Forms.Button();
            this.cmdSQLDatabaseNameHelp = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.nudSQLPort = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.cmdSQLServerNameHelp = new System.Windows.Forms.Button();
            this.txtSQLServerName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btn4Rights = new System.Windows.Forms.CheckBox();
            this.pnl3Opts = new System.Windows.Forms.Panel();
            this.cmdOptsMoveFileImport = new System.Windows.Forms.Button();
            this.dgvOptsMoveFile = new System.Windows.Forms.DataGridView();
            this.colLogicalName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkOptsMoveFile = new System.Windows.Forms.CheckBox();
            this.cmdOptsMoveAllPathHelp = new System.Windows.Forms.Button();
            this.cmdOptsMoveAllBrowse = new System.Windows.Forms.Button();
            this.txtOptsMoveallPath = new System.Windows.Forms.TextBox();
            this.lblOptsMoveallPath = new System.Windows.Forms.Label();
            this.chkOptsMoveall = new System.Windows.Forms.CheckBox();
            this.chkOptsCheckdb = new System.Windows.Forms.CheckBox();
            this.chkOptsReplace = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.pnl4Rights = new System.Windows.Forms.Panel();
            this.dgvRightsUsers = new System.Windows.Forms.DataGridView();
            this.cmdRightsImportLogins = new System.Windows.Forms.Button();
            this.chkRightsEnable = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.pnl5Log = new System.Windows.Forms.Panel();
            this.cmdLogAppendBrowse = new System.Windows.Forms.Button();
            this.txtLogAppendFile = new System.Windows.Forms.TextBox();
            this.lblLogAppendFile = new System.Windows.Forms.Label();
            this.chkLogAppendEnable = new System.Windows.Forms.CheckBox();
            this.cmdLogFileBrowse = new System.Windows.Forms.Button();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            this.lblLogFile = new System.Windows.Forms.Label();
            this.chkLogEnable = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.pnl6SMTP = new System.Windows.Forms.Panel();
            this.grpSMTPProfile = new System.Windows.Forms.GroupBox();
            this.lblSMTPPassWarn = new System.Windows.Forms.Label();
            this.cmdSMTPLoadProfile = new System.Windows.Forms.Button();
            this.cmdSMTPSaveProfile = new System.Windows.Forms.Button();
            this.txtSMTPBody = new System.Windows.Forms.TextBox();
            this.chkSMTPAttachLog = new System.Windows.Forms.CheckBox();
            this.label26 = new System.Windows.Forms.Label();
            this.txtSMTPSubject = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtSMTPEmailTo = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtSMTPEmailFrom = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtSMTPPass = new System.Windows.Forms.TextBox();
            this.lblSMTPPass = new System.Windows.Forms.Label();
            this.txtSMTPUser = new System.Windows.Forms.TextBox();
            this.lblSMTPUser = new System.Windows.Forms.Label();
            this.chkSMTPAuth = new System.Windows.Forms.CheckBox();
            this.chkSMTPTLS = new System.Windows.Forms.CheckBox();
            this.nudSMTPPort = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.txtSMTPServer = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cmdSMTPProfileBrowse = new System.Windows.Forms.Button();
            this.txtSMTPProfile = new System.Windows.Forms.TextBox();
            this.lblSMTPProfile = new System.Windows.Forms.Label();
            this.chkSMTPSendEmail = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.pnl7CLI = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.pnl8Run = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.lblRightsHelp = new System.Windows.Forms.Label();
            this.cmdStartLoadSettings = new System.Windows.Forms.Button();
            this.cmdStartSaveSettings = new System.Windows.Forms.Button();
            this.pnl0Start.SuspendLayout();
            this.pnl1Source.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlSourceAutoSelect.SuspendLayout();
            this.pnlSourceManualSelect.SuspendLayout();
            this.pnl2SQL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSQLPort)).BeginInit();
            this.pnl3Opts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptsMoveFile)).BeginInit();
            this.pnl4Rights.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRightsUsers)).BeginInit();
            this.pnl5Log.SuspendLayout();
            this.pnl6SMTP.SuspendLayout();
            this.grpSMTPProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSMTPPort)).BeginInit();
            this.pnl7CLI.SuspendLayout();
            this.pnl8Run.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl0Start
            // 
            this.pnl0Start.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl0Start.Controls.Add(this.cmdStartSaveSettings);
            this.pnl0Start.Controls.Add(this.cmdStartLoadSettings);
            this.pnl0Start.Controls.Add(this.lbl1Version);
            this.pnl0Start.Controls.Add(this.label1);
            this.pnl0Start.Controls.Add(this.label2);
            this.pnl0Start.Location = new System.Drawing.Point(169, 12);
            this.pnl0Start.Margin = new System.Windows.Forms.Padding(4);
            this.pnl0Start.Name = "pnl0Start";
            this.pnl0Start.Size = new System.Drawing.Size(562, 476);
            this.pnl0Start.TabIndex = 0;
            this.pnl0Start.Tag = "Start";
            // 
            // lbl1Version
            // 
            this.lbl1Version.AutoSize = true;
            this.lbl1Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1Version.Location = new System.Drawing.Point(4, 26);
            this.lbl1Version.Name = "lbl1Version";
            this.lbl1Version.Size = new System.Drawing.Size(42, 13);
            this.lbl1Version.TabIndex = 1;
            this.lbl1Version.Text = "Version";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database Restore GUI";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(4, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(555, 229);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // btn0Start
            // 
            this.btn0Start.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn0Start.Location = new System.Drawing.Point(12, 12);
            this.btn0Start.Name = "btn0Start";
            this.btn0Start.Size = new System.Drawing.Size(150, 32);
            this.btn0Start.TabIndex = 1;
            this.btn0Start.Tag = "Start";
            this.btn0Start.Text = "Start";
            this.btn0Start.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn0Start.UseVisualStyleBackColor = true;
            this.btn0Start.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn1Soruce
            // 
            this.btn1Soruce.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn1Soruce.Location = new System.Drawing.Point(12, 50);
            this.btn1Soruce.Name = "btn1Soruce";
            this.btn1Soruce.Size = new System.Drawing.Size(150, 32);
            this.btn1Soruce.TabIndex = 2;
            this.btn1Soruce.Tag = "Source";
            this.btn1Soruce.Text = "Source";
            this.btn1Soruce.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn1Soruce.UseVisualStyleBackColor = true;
            this.btn1Soruce.Click += new System.EventHandler(this.btn_Click);
            // 
            // pnl1Source
            // 
            this.pnl1Source.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl1Source.Controls.Add(this.panel1);
            this.pnl1Source.Controls.Add(this.cmbSourceSelectionMode);
            this.pnl1Source.Controls.Add(this.label4);
            this.pnl1Source.Controls.Add(this.label3);
            this.pnl1Source.Controls.Add(this.pnlSourceAutoSelect);
            this.pnl1Source.Controls.Add(this.pnlSourceManualSelect);
            this.pnl1Source.Location = new System.Drawing.Point(169, 12);
            this.pnl1Source.Margin = new System.Windows.Forms.Padding(4);
            this.pnl1Source.Name = "pnl1Source";
            this.pnl1Source.Size = new System.Drawing.Size(562, 476);
            this.pnl1Source.TabIndex = 3;
            this.pnl1Source.Tag = "Source";
            this.pnl1Source.VisibleChanged += new System.EventHandler(this.pnl1Source_VisibleChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblSourceTempHelp);
            this.panel1.Controls.Add(this.cmdSourceTempFileBrowse);
            this.panel1.Controls.Add(this.txtSourceTempFilePath);
            this.panel1.Controls.Add(this.chkSourceTempEnable);
            this.panel1.Location = new System.Drawing.Point(7, 152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(552, 181);
            this.panel1.TabIndex = 7;
            // 
            // lblSourceTempHelp
            // 
            this.lblSourceTempHelp.Location = new System.Drawing.Point(-3, 54);
            this.lblSourceTempHelp.Name = "lblSourceTempHelp";
            this.lblSourceTempHelp.Size = new System.Drawing.Size(552, 127);
            this.lblSourceTempHelp.TabIndex = 5;
            this.lblSourceTempHelp.Text = resources.GetString("lblSourceTempHelp.Text");
            this.lblSourceTempHelp.Visible = false;
            // 
            // cmdSourceTempFileBrowse
            // 
            this.cmdSourceTempFileBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSourceTempFileBrowse.Location = new System.Drawing.Point(474, 26);
            this.cmdSourceTempFileBrowse.Name = "cmdSourceTempFileBrowse";
            this.cmdSourceTempFileBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdSourceTempFileBrowse.TabIndex = 4;
            this.cmdSourceTempFileBrowse.Text = "Browse";
            this.cmdSourceTempFileBrowse.UseVisualStyleBackColor = true;
            this.cmdSourceTempFileBrowse.Visible = false;
            this.cmdSourceTempFileBrowse.Click += new System.EventHandler(this.cmdSourceTempFileBrowse_Click);
            // 
            // txtSourceTempFilePath
            // 
            this.txtSourceTempFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceTempFilePath.Location = new System.Drawing.Point(0, 26);
            this.txtSourceTempFilePath.Name = "txtSourceTempFilePath";
            this.txtSourceTempFilePath.Size = new System.Drawing.Size(468, 22);
            this.txtSourceTempFilePath.TabIndex = 3;
            this.txtSourceTempFilePath.Visible = false;
            // 
            // chkSourceTempEnable
            // 
            this.chkSourceTempEnable.AutoSize = true;
            this.chkSourceTempEnable.Location = new System.Drawing.Point(0, 0);
            this.chkSourceTempEnable.Name = "chkSourceTempEnable";
            this.chkSourceTempEnable.Size = new System.Drawing.Size(233, 20);
            this.chkSourceTempEnable.TabIndex = 0;
            this.chkSourceTempEnable.Text = "Copy backup file to temp folder first";
            this.chkSourceTempEnable.UseVisualStyleBackColor = true;
            this.chkSourceTempEnable.CheckedChanged += new System.EventHandler(this.chkSourceTempEnable_CheckedChanged);
            // 
            // cmbSourceSelectionMode
            // 
            this.cmbSourceSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceSelectionMode.FormattingEnabled = true;
            this.cmbSourceSelectionMode.Items.AddRange(new object[] {
            "Automatic",
            "Manual"});
            this.cmbSourceSelectionMode.Location = new System.Drawing.Point(158, 35);
            this.cmbSourceSelectionMode.Name = "cmbSourceSelectionMode";
            this.cmbSourceSelectionMode.Size = new System.Drawing.Size(147, 24);
            this.cmbSourceSelectionMode.TabIndex = 4;
            this.cmbSourceSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cmbSourceSelectionMode_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "File Selection Behavior:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Source File Selection";
            // 
            // pnlSourceAutoSelect
            // 
            this.pnlSourceAutoSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSourceAutoSelect.Controls.Add(this.cmbAutoSourceAttribute);
            this.pnlSourceAutoSelect.Controls.Add(this.label6);
            this.pnlSourceAutoSelect.Controls.Add(this.cmdSourceAutoBrowse);
            this.pnlSourceAutoSelect.Controls.Add(this.label5);
            this.pnlSourceAutoSelect.Controls.Add(this.txtSourceAutoPath);
            this.pnlSourceAutoSelect.Location = new System.Drawing.Point(7, 65);
            this.pnlSourceAutoSelect.Name = "pnlSourceAutoSelect";
            this.pnlSourceAutoSelect.Size = new System.Drawing.Size(552, 81);
            this.pnlSourceAutoSelect.TabIndex = 5;
            this.pnlSourceAutoSelect.Visible = false;
            this.pnlSourceAutoSelect.VisibleChanged += new System.EventHandler(this.pnlSourceAutoSelect_VisibleChanged);
            // 
            // cmbAutoSourceAttribute
            // 
            this.cmbAutoSourceAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutoSourceAttribute.FormattingEnabled = true;
            this.cmbAutoSourceAttribute.Items.AddRange(new object[] {
            "Creation Date",
            "Last Modified Date"});
            this.cmbAutoSourceAttribute.Location = new System.Drawing.Point(118, 47);
            this.cmbAutoSourceAttribute.Name = "cmbAutoSourceAttribute";
            this.cmbAutoSourceAttribute.Size = new System.Drawing.Size(147, 24);
            this.cmbAutoSourceAttribute.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-3, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Newest based on:";
            // 
            // cmdSourceAutoBrowse
            // 
            this.cmdSourceAutoBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSourceAutoBrowse.Location = new System.Drawing.Point(474, 19);
            this.cmdSourceAutoBrowse.Name = "cmdSourceAutoBrowse";
            this.cmdSourceAutoBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdSourceAutoBrowse.TabIndex = 2;
            this.cmdSourceAutoBrowse.Text = "Browse";
            this.cmdSourceAutoBrowse.UseVisualStyleBackColor = true;
            this.cmdSourceAutoBrowse.Click += new System.EventHandler(this.cmdSourceAutoBrowse_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(252, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "Choose Newest File From Source Folder:";
            // 
            // txtSourceAutoPath
            // 
            this.txtSourceAutoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceAutoPath.Location = new System.Drawing.Point(0, 19);
            this.txtSourceAutoPath.Name = "txtSourceAutoPath";
            this.txtSourceAutoPath.Size = new System.Drawing.Size(468, 22);
            this.txtSourceAutoPath.TabIndex = 0;
            // 
            // pnlSourceManualSelect
            // 
            this.pnlSourceManualSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSourceManualSelect.Controls.Add(this.cmdSourceManualBrowse);
            this.pnlSourceManualSelect.Controls.Add(this.txtSourceFile);
            this.pnlSourceManualSelect.Controls.Add(this.label7);
            this.pnlSourceManualSelect.Location = new System.Drawing.Point(7, 65);
            this.pnlSourceManualSelect.Name = "pnlSourceManualSelect";
            this.pnlSourceManualSelect.Size = new System.Drawing.Size(552, 81);
            this.pnlSourceManualSelect.TabIndex = 6;
            this.pnlSourceManualSelect.Visible = false;
            // 
            // cmdSourceManualBrowse
            // 
            this.cmdSourceManualBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSourceManualBrowse.Location = new System.Drawing.Point(474, 19);
            this.cmdSourceManualBrowse.Name = "cmdSourceManualBrowse";
            this.cmdSourceManualBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdSourceManualBrowse.TabIndex = 4;
            this.cmdSourceManualBrowse.Text = "Browse";
            this.cmdSourceManualBrowse.UseVisualStyleBackColor = true;
            this.cmdSourceManualBrowse.Click += new System.EventHandler(this.cmdSourceManualBrowse_Click);
            // 
            // txtSourceFile
            // 
            this.txtSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceFile.Location = new System.Drawing.Point(0, 19);
            this.txtSourceFile.Name = "txtSourceFile";
            this.txtSourceFile.Size = new System.Drawing.Size(468, 22);
            this.txtSourceFile.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "Source File";
            // 
            // btn2SQL
            // 
            this.btn2SQL.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn2SQL.Location = new System.Drawing.Point(12, 88);
            this.btn2SQL.Name = "btn2SQL";
            this.btn2SQL.Size = new System.Drawing.Size(150, 32);
            this.btn2SQL.TabIndex = 4;
            this.btn2SQL.Tag = "SQLConnection";
            this.btn2SQL.Text = "SQL Connection";
            this.btn2SQL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn2SQL.UseVisualStyleBackColor = true;
            this.btn2SQL.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn3Opts
            // 
            this.btn3Opts.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn3Opts.Location = new System.Drawing.Point(12, 126);
            this.btn3Opts.Name = "btn3Opts";
            this.btn3Opts.Size = new System.Drawing.Size(150, 32);
            this.btn3Opts.TabIndex = 5;
            this.btn3Opts.Tag = "RestoreOptions";
            this.btn3Opts.Text = "Restore Options";
            this.btn3Opts.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn3Opts.UseVisualStyleBackColor = true;
            this.btn3Opts.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn5Log
            // 
            this.btn5Log.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn5Log.Location = new System.Drawing.Point(12, 202);
            this.btn5Log.Name = "btn5Log";
            this.btn5Log.Size = new System.Drawing.Size(150, 32);
            this.btn5Log.TabIndex = 6;
            this.btn5Log.Tag = "Logging";
            this.btn5Log.Text = "Logging";
            this.btn5Log.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn5Log.UseVisualStyleBackColor = true;
            this.btn5Log.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn6SMTP
            // 
            this.btn6SMTP.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn6SMTP.Location = new System.Drawing.Point(12, 240);
            this.btn6SMTP.Name = "btn6SMTP";
            this.btn6SMTP.Size = new System.Drawing.Size(150, 32);
            this.btn6SMTP.TabIndex = 7;
            this.btn6SMTP.Tag = "SMTPOptions";
            this.btn6SMTP.Text = "SMTP Options";
            this.btn6SMTP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn6SMTP.UseVisualStyleBackColor = true;
            this.btn6SMTP.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn7CLI
            // 
            this.btn7CLI.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn7CLI.Location = new System.Drawing.Point(12, 278);
            this.btn7CLI.Name = "btn7CLI";
            this.btn7CLI.Size = new System.Drawing.Size(150, 32);
            this.btn7CLI.TabIndex = 8;
            this.btn7CLI.Tag = "CLIArguments";
            this.btn7CLI.Text = "CLI Arguments";
            this.btn7CLI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn7CLI.UseVisualStyleBackColor = true;
            this.btn7CLI.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn8Run
            // 
            this.btn8Run.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn8Run.Location = new System.Drawing.Point(12, 316);
            this.btn8Run.Name = "btn8Run";
            this.btn8Run.Size = new System.Drawing.Size(150, 32);
            this.btn8Run.TabIndex = 9;
            this.btn8Run.Tag = "Run";
            this.btn8Run.Text = "Run";
            this.btn8Run.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn8Run.UseVisualStyleBackColor = true;
            this.btn8Run.Click += new System.EventHandler(this.btn_Click);
            // 
            // pnl2SQL
            // 
            this.pnl2SQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl2SQL.Controls.Add(this.cmbSQLDatabaseName);
            this.pnl2SQL.Controls.Add(this.lblSQLCredentialInfo);
            this.pnl2SQL.Controls.Add(this.txtSQLPassword);
            this.pnl2SQL.Controls.Add(this.lblSQLPassword);
            this.pnl2SQL.Controls.Add(this.txtSQLUsername);
            this.pnl2SQL.Controls.Add(this.lblSQLUsername);
            this.pnl2SQL.Controls.Add(this.chkSQLUseCredentials);
            this.pnl2SQL.Controls.Add(this.cmdSQLDatabaseNameQuery);
            this.pnl2SQL.Controls.Add(this.cmdSQLDatabaseNameHelp);
            this.pnl2SQL.Controls.Add(this.label11);
            this.pnl2SQL.Controls.Add(this.nudSQLPort);
            this.pnl2SQL.Controls.Add(this.label9);
            this.pnl2SQL.Controls.Add(this.cmdSQLServerNameHelp);
            this.pnl2SQL.Controls.Add(this.txtSQLServerName);
            this.pnl2SQL.Controls.Add(this.label8);
            this.pnl2SQL.Controls.Add(this.label10);
            this.pnl2SQL.Location = new System.Drawing.Point(169, 12);
            this.pnl2SQL.Margin = new System.Windows.Forms.Padding(4);
            this.pnl2SQL.Name = "pnl2SQL";
            this.pnl2SQL.Size = new System.Drawing.Size(562, 476);
            this.pnl2SQL.TabIndex = 10;
            this.pnl2SQL.Tag = "SQLConnection";
            // 
            // cmbSQLDatabaseName
            // 
            this.cmbSQLDatabaseName.FormattingEnabled = true;
            this.cmbSQLDatabaseName.Location = new System.Drawing.Point(97, 91);
            this.cmbSQLDatabaseName.Name = "cmbSQLDatabaseName";
            this.cmbSQLDatabaseName.Size = new System.Drawing.Size(244, 24);
            this.cmbSQLDatabaseName.TabIndex = 16;
            // 
            // lblSQLCredentialInfo
            // 
            this.lblSQLCredentialInfo.Location = new System.Drawing.Point(3, 200);
            this.lblSQLCredentialInfo.Name = "lblSQLCredentialInfo";
            this.lblSQLCredentialInfo.Size = new System.Drawing.Size(552, 83);
            this.lblSQLCredentialInfo.TabIndex = 15;
            this.lblSQLCredentialInfo.Text = resources.GetString("lblSQLCredentialInfo.Text");
            this.lblSQLCredentialInfo.Visible = false;
            // 
            // txtSQLPassword
            // 
            this.txtSQLPassword.Location = new System.Drawing.Point(97, 173);
            this.txtSQLPassword.Name = "txtSQLPassword";
            this.txtSQLPassword.PasswordChar = '●';
            this.txtSQLPassword.Size = new System.Drawing.Size(244, 22);
            this.txtSQLPassword.TabIndex = 14;
            this.txtSQLPassword.Visible = false;
            // 
            // lblSQLPassword
            // 
            this.lblSQLPassword.AutoSize = true;
            this.lblSQLPassword.Location = new System.Drawing.Point(27, 176);
            this.lblSQLPassword.Name = "lblSQLPassword";
            this.lblSQLPassword.Size = new System.Drawing.Size(67, 16);
            this.lblSQLPassword.TabIndex = 13;
            this.lblSQLPassword.Text = "Password";
            this.lblSQLPassword.Visible = false;
            // 
            // txtSQLUsername
            // 
            this.txtSQLUsername.Location = new System.Drawing.Point(97, 145);
            this.txtSQLUsername.Name = "txtSQLUsername";
            this.txtSQLUsername.Size = new System.Drawing.Size(244, 22);
            this.txtSQLUsername.TabIndex = 12;
            this.txtSQLUsername.Visible = false;
            // 
            // lblSQLUsername
            // 
            this.lblSQLUsername.AutoSize = true;
            this.lblSQLUsername.Location = new System.Drawing.Point(24, 148);
            this.lblSQLUsername.Name = "lblSQLUsername";
            this.lblSQLUsername.Size = new System.Drawing.Size(70, 16);
            this.lblSQLUsername.TabIndex = 11;
            this.lblSQLUsername.Text = "Username";
            this.lblSQLUsername.Visible = false;
            // 
            // chkSQLUseCredentials
            // 
            this.chkSQLUseCredentials.AutoSize = true;
            this.chkSQLUseCredentials.Location = new System.Drawing.Point(97, 119);
            this.chkSQLUseCredentials.Name = "chkSQLUseCredentials";
            this.chkSQLUseCredentials.Size = new System.Drawing.Size(343, 20);
            this.chkSQLUseCredentials.TabIndex = 10;
            this.chkSQLUseCredentials.Text = "Specify Username and Password (no integrated auth)";
            this.chkSQLUseCredentials.UseVisualStyleBackColor = true;
            this.chkSQLUseCredentials.CheckedChanged += new System.EventHandler(this.chkSQLUseCredentials_CheckedChanged);
            // 
            // cmdSQLDatabaseNameQuery
            // 
            this.cmdSQLDatabaseNameQuery.Location = new System.Drawing.Point(347, 91);
            this.cmdSQLDatabaseNameQuery.Name = "cmdSQLDatabaseNameQuery";
            this.cmdSQLDatabaseNameQuery.Size = new System.Drawing.Size(55, 23);
            this.cmdSQLDatabaseNameQuery.TabIndex = 9;
            this.cmdSQLDatabaseNameQuery.Text = "Query";
            this.cmdSQLDatabaseNameQuery.UseVisualStyleBackColor = true;
            this.cmdSQLDatabaseNameQuery.Click += new System.EventHandler(this.cmdSQLDatabaseNameQuery_Click);
            // 
            // cmdSQLDatabaseNameHelp
            // 
            this.cmdSQLDatabaseNameHelp.Location = new System.Drawing.Point(408, 91);
            this.cmdSQLDatabaseNameHelp.Name = "cmdSQLDatabaseNameHelp";
            this.cmdSQLDatabaseNameHelp.Size = new System.Drawing.Size(21, 23);
            this.cmdSQLDatabaseNameHelp.TabIndex = 8;
            this.cmdSQLDatabaseNameHelp.Text = "?";
            this.cmdSQLDatabaseNameHelp.UseVisualStyleBackColor = true;
            this.cmdSQLDatabaseNameHelp.Click += new System.EventHandler(this.cmdSQLDatabaseNameHelp_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(24, 94);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 16);
            this.label11.TabIndex = 6;
            this.label11.Text = "Database";
            // 
            // nudSQLPort
            // 
            this.nudSQLPort.Location = new System.Drawing.Point(97, 63);
            this.nudSQLPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudSQLPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSQLPort.Name = "nudSQLPort";
            this.nudSQLPort.Size = new System.Drawing.Size(82, 22);
            this.nudSQLPort.TabIndex = 5;
            this.nudSQLPort.Value = new decimal(new int[] {
            1433,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(60, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 16);
            this.label9.TabIndex = 4;
            this.label9.Text = "Port";
            // 
            // cmdSQLServerNameHelp
            // 
            this.cmdSQLServerNameHelp.Location = new System.Drawing.Point(347, 31);
            this.cmdSQLServerNameHelp.Name = "cmdSQLServerNameHelp";
            this.cmdSQLServerNameHelp.Size = new System.Drawing.Size(21, 23);
            this.cmdSQLServerNameHelp.TabIndex = 3;
            this.cmdSQLServerNameHelp.Text = "?";
            this.cmdSQLServerNameHelp.UseVisualStyleBackColor = true;
            this.cmdSQLServerNameHelp.Click += new System.EventHandler(this.cmdSQLServerNameHelp_Click);
            // 
            // txtSQLServerName
            // 
            this.txtSQLServerName.Location = new System.Drawing.Point(97, 32);
            this.txtSQLServerName.Name = "txtSQLServerName";
            this.txtSQLServerName.Size = new System.Drawing.Size(244, 22);
            this.txtSQLServerName.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 16);
            this.label8.TabIndex = 1;
            this.label8.Text = "Server Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(269, 24);
            this.label10.TabIndex = 0;
            this.label10.Text = "SQL Server Connection Details";
            // 
            // btn4Rights
            // 
            this.btn4Rights.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn4Rights.Location = new System.Drawing.Point(12, 164);
            this.btn4Rights.Name = "btn4Rights";
            this.btn4Rights.Size = new System.Drawing.Size(150, 32);
            this.btn4Rights.TabIndex = 11;
            this.btn4Rights.Tag = "DatabaseRights";
            this.btn4Rights.Text = "Database Rights";
            this.btn4Rights.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn4Rights.UseVisualStyleBackColor = true;
            this.btn4Rights.Click += new System.EventHandler(this.btn_Click);
            // 
            // pnl3Opts
            // 
            this.pnl3Opts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl3Opts.Controls.Add(this.cmdOptsMoveFileImport);
            this.pnl3Opts.Controls.Add(this.dgvOptsMoveFile);
            this.pnl3Opts.Controls.Add(this.chkOptsMoveFile);
            this.pnl3Opts.Controls.Add(this.cmdOptsMoveAllPathHelp);
            this.pnl3Opts.Controls.Add(this.cmdOptsMoveAllBrowse);
            this.pnl3Opts.Controls.Add(this.txtOptsMoveallPath);
            this.pnl3Opts.Controls.Add(this.lblOptsMoveallPath);
            this.pnl3Opts.Controls.Add(this.chkOptsMoveall);
            this.pnl3Opts.Controls.Add(this.chkOptsCheckdb);
            this.pnl3Opts.Controls.Add(this.chkOptsReplace);
            this.pnl3Opts.Controls.Add(this.label14);
            this.pnl3Opts.Location = new System.Drawing.Point(169, 12);
            this.pnl3Opts.Margin = new System.Windows.Forms.Padding(4);
            this.pnl3Opts.Name = "pnl3Opts";
            this.pnl3Opts.Size = new System.Drawing.Size(562, 476);
            this.pnl3Opts.TabIndex = 12;
            this.pnl3Opts.Tag = "RestoreOptions";
            // 
            // cmdOptsMoveFileImport
            // 
            this.cmdOptsMoveFileImport.Location = new System.Drawing.Point(317, 134);
            this.cmdOptsMoveFileImport.Name = "cmdOptsMoveFileImport";
            this.cmdOptsMoveFileImport.Size = new System.Drawing.Size(112, 23);
            this.cmdOptsMoveFileImport.TabIndex = 10;
            this.cmdOptsMoveFileImport.Text = "Import from SQL";
            this.cmdOptsMoveFileImport.UseVisualStyleBackColor = true;
            this.cmdOptsMoveFileImport.Visible = false;
            this.cmdOptsMoveFileImport.Click += new System.EventHandler(this.cmdOptsMoveFileImport_Click);
            // 
            // dgvOptsMoveFile
            // 
            this.dgvOptsMoveFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOptsMoveFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOptsMoveFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLogicalName,
            this.colOrigPath,
            this.colNewPath});
            this.dgvOptsMoveFile.Location = new System.Drawing.Point(7, 163);
            this.dgvOptsMoveFile.Name = "dgvOptsMoveFile";
            this.dgvOptsMoveFile.Size = new System.Drawing.Size(552, 310);
            this.dgvOptsMoveFile.TabIndex = 9;
            this.dgvOptsMoveFile.Visible = false;
            // 
            // colLogicalName
            // 
            this.colLogicalName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLogicalName.DataPropertyName = "LogicalName";
            this.colLogicalName.FillWeight = 35F;
            this.colLogicalName.HeaderText = "Logical Name";
            this.colLogicalName.MaxInputLength = 256;
            this.colLogicalName.Name = "colLogicalName";
            // 
            // colOrigPath
            // 
            this.colOrigPath.DataPropertyName = "OriginalPath";
            this.colOrigPath.HeaderText = "Original Path";
            this.colOrigPath.Name = "colOrigPath";
            this.colOrigPath.ReadOnly = true;
            this.colOrigPath.Width = 75;
            // 
            // colNewPath
            // 
            this.colNewPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNewPath.DataPropertyName = "OverriddenPath";
            this.colNewPath.HeaderText = "New Path";
            this.colNewPath.Name = "colNewPath";
            // 
            // chkOptsMoveFile
            // 
            this.chkOptsMoveFile.AutoSize = true;
            this.chkOptsMoveFile.Location = new System.Drawing.Point(7, 136);
            this.chkOptsMoveFile.Name = "chkOptsMoveFile";
            this.chkOptsMoveFile.Size = new System.Drawing.Size(295, 20);
            this.chkOptsMoveFile.TabIndex = 8;
            this.chkOptsMoveFile.Text = "Move specific database files to new locations";
            this.chkOptsMoveFile.UseVisualStyleBackColor = true;
            this.chkOptsMoveFile.CheckedChanged += new System.EventHandler(this.chkOptsMoveFile_CheckedChanged);
            // 
            // cmdOptsMoveAllPathHelp
            // 
            this.cmdOptsMoveAllPathHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOptsMoveAllPathHelp.Location = new System.Drawing.Point(534, 106);
            this.cmdOptsMoveAllPathHelp.Name = "cmdOptsMoveAllPathHelp";
            this.cmdOptsMoveAllPathHelp.Size = new System.Drawing.Size(21, 23);
            this.cmdOptsMoveAllPathHelp.TabIndex = 7;
            this.cmdOptsMoveAllPathHelp.Text = "?";
            this.cmdOptsMoveAllPathHelp.UseVisualStyleBackColor = true;
            this.cmdOptsMoveAllPathHelp.Visible = false;
            this.cmdOptsMoveAllPathHelp.Click += new System.EventHandler(this.cmdOptsMoveAllPathHelp_Click);
            // 
            // cmdOptsMoveAllBrowse
            // 
            this.cmdOptsMoveAllBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOptsMoveAllBrowse.Location = new System.Drawing.Point(454, 106);
            this.cmdOptsMoveAllBrowse.Name = "cmdOptsMoveAllBrowse";
            this.cmdOptsMoveAllBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdOptsMoveAllBrowse.TabIndex = 6;
            this.cmdOptsMoveAllBrowse.Text = "Browse";
            this.cmdOptsMoveAllBrowse.UseVisualStyleBackColor = true;
            this.cmdOptsMoveAllBrowse.Visible = false;
            this.cmdOptsMoveAllBrowse.Click += new System.EventHandler(this.cmdOptsMoveAllBrowse_Click);
            // 
            // txtOptsMoveallPath
            // 
            this.txtOptsMoveallPath.Location = new System.Drawing.Point(52, 106);
            this.txtOptsMoveallPath.Name = "txtOptsMoveallPath";
            this.txtOptsMoveallPath.Size = new System.Drawing.Size(396, 22);
            this.txtOptsMoveallPath.TabIndex = 5;
            this.txtOptsMoveallPath.Visible = false;
            // 
            // lblOptsMoveallPath
            // 
            this.lblOptsMoveallPath.AutoSize = true;
            this.lblOptsMoveallPath.Location = new System.Drawing.Point(12, 109);
            this.lblOptsMoveallPath.Name = "lblOptsMoveallPath";
            this.lblOptsMoveallPath.Size = new System.Drawing.Size(34, 16);
            this.lblOptsMoveallPath.TabIndex = 4;
            this.lblOptsMoveallPath.Text = "Path";
            this.lblOptsMoveallPath.Visible = false;
            // 
            // chkOptsMoveall
            // 
            this.chkOptsMoveall.AutoSize = true;
            this.chkOptsMoveall.Location = new System.Drawing.Point(7, 80);
            this.chkOptsMoveall.Name = "chkOptsMoveall";
            this.chkOptsMoveall.Size = new System.Drawing.Size(397, 20);
            this.chkOptsMoveall.TabIndex = 3;
            this.chkOptsMoveall.Text = "Move all restored database files to new location on SQL server";
            this.chkOptsMoveall.UseVisualStyleBackColor = true;
            this.chkOptsMoveall.CheckedChanged += new System.EventHandler(this.chkOptsMoveall_CheckedChanged);
            // 
            // chkOptsCheckdb
            // 
            this.chkOptsCheckdb.AutoSize = true;
            this.chkOptsCheckdb.Location = new System.Drawing.Point(7, 55);
            this.chkOptsCheckdb.Name = "chkOptsCheckdb";
            this.chkOptsCheckdb.Size = new System.Drawing.Size(463, 20);
            this.chkOptsCheckdb.TabIndex = 2;
            this.chkOptsCheckdb.Text = "Run DBCC CHECKDB after restore to verify no database corruption/errors.";
            this.chkOptsCheckdb.UseVisualStyleBackColor = true;
            // 
            // chkOptsReplace
            // 
            this.chkOptsReplace.AutoSize = true;
            this.chkOptsReplace.Location = new System.Drawing.Point(7, 31);
            this.chkOptsReplace.Name = "chkOptsReplace";
            this.chkOptsReplace.Size = new System.Drawing.Size(376, 20);
            this.chkOptsReplace.TabIndex = 1;
            this.chkOptsReplace.Text = "Replace Database if it exists (SQL WITH REPLACE option)";
            this.chkOptsReplace.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(3, 2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(145, 24);
            this.label14.TabIndex = 0;
            this.label14.Text = "Restore Options";
            // 
            // pnl4Rights
            // 
            this.pnl4Rights.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl4Rights.Controls.Add(this.lblRightsHelp);
            this.pnl4Rights.Controls.Add(this.dgvRightsUsers);
            this.pnl4Rights.Controls.Add(this.cmdRightsImportLogins);
            this.pnl4Rights.Controls.Add(this.chkRightsEnable);
            this.pnl4Rights.Controls.Add(this.label12);
            this.pnl4Rights.Location = new System.Drawing.Point(169, 12);
            this.pnl4Rights.Margin = new System.Windows.Forms.Padding(4);
            this.pnl4Rights.Name = "pnl4Rights";
            this.pnl4Rights.Size = new System.Drawing.Size(562, 476);
            this.pnl4Rights.TabIndex = 13;
            this.pnl4Rights.Tag = "DatabaseRights";
            // 
            // dgvRightsUsers
            // 
            this.dgvRightsUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRightsUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRightsUsers.Location = new System.Drawing.Point(7, 56);
            this.dgvRightsUsers.Name = "dgvRightsUsers";
            this.dgvRightsUsers.Size = new System.Drawing.Size(552, 345);
            this.dgvRightsUsers.TabIndex = 3;
            this.dgvRightsUsers.Visible = false;
            // 
            // cmdRightsImportLogins
            // 
            this.cmdRightsImportLogins.Location = new System.Drawing.Point(357, 27);
            this.cmdRightsImportLogins.Name = "cmdRightsImportLogins";
            this.cmdRightsImportLogins.Size = new System.Drawing.Size(113, 23);
            this.cmdRightsImportLogins.TabIndex = 2;
            this.cmdRightsImportLogins.Text = "Import Logins";
            this.cmdRightsImportLogins.UseVisualStyleBackColor = true;
            this.cmdRightsImportLogins.Visible = false;
            this.cmdRightsImportLogins.Click += new System.EventHandler(this.cmdRightsImportLogins_Click);
            // 
            // chkRightsEnable
            // 
            this.chkRightsEnable.AutoSize = true;
            this.chkRightsEnable.Location = new System.Drawing.Point(7, 29);
            this.chkRightsEnable.Name = "chkRightsEnable";
            this.chkRightsEnable.Size = new System.Drawing.Size(344, 20);
            this.chkRightsEnable.TabIndex = 1;
            this.chkRightsEnable.Text = "Add SQL Logins as database Users, and add to roles";
            this.chkRightsEnable.UseVisualStyleBackColor = true;
            this.chkRightsEnable.CheckedChanged += new System.EventHandler(this.chkRightsEnable_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(3, 2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(140, 24);
            this.label12.TabIndex = 0;
            this.label12.Text = "DatabaseRights";
            // 
            // pnl5Log
            // 
            this.pnl5Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl5Log.Controls.Add(this.cmdLogAppendBrowse);
            this.pnl5Log.Controls.Add(this.txtLogAppendFile);
            this.pnl5Log.Controls.Add(this.lblLogAppendFile);
            this.pnl5Log.Controls.Add(this.chkLogAppendEnable);
            this.pnl5Log.Controls.Add(this.cmdLogFileBrowse);
            this.pnl5Log.Controls.Add(this.txtLogFile);
            this.pnl5Log.Controls.Add(this.lblLogFile);
            this.pnl5Log.Controls.Add(this.chkLogEnable);
            this.pnl5Log.Controls.Add(this.label13);
            this.pnl5Log.Location = new System.Drawing.Point(169, 12);
            this.pnl5Log.Margin = new System.Windows.Forms.Padding(4);
            this.pnl5Log.Name = "pnl5Log";
            this.pnl5Log.Size = new System.Drawing.Size(562, 476);
            this.pnl5Log.TabIndex = 14;
            this.pnl5Log.Tag = "Logging";
            // 
            // cmdLogAppendBrowse
            // 
            this.cmdLogAppendBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLogAppendBrowse.Location = new System.Drawing.Point(480, 107);
            this.cmdLogAppendBrowse.Name = "cmdLogAppendBrowse";
            this.cmdLogAppendBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdLogAppendBrowse.TabIndex = 11;
            this.cmdLogAppendBrowse.Text = "Browse";
            this.cmdLogAppendBrowse.UseVisualStyleBackColor = true;
            this.cmdLogAppendBrowse.Visible = false;
            this.cmdLogAppendBrowse.Click += new System.EventHandler(this.cmdLogAppendBrowse_Click);
            // 
            // txtLogAppendFile
            // 
            this.txtLogAppendFile.Location = new System.Drawing.Point(63, 107);
            this.txtLogAppendFile.Name = "txtLogAppendFile";
            this.txtLogAppendFile.Size = new System.Drawing.Size(411, 22);
            this.txtLogAppendFile.TabIndex = 10;
            this.txtLogAppendFile.Visible = false;
            // 
            // lblLogAppendFile
            // 
            this.lblLogAppendFile.AutoSize = true;
            this.lblLogAppendFile.Location = new System.Drawing.Point(4, 110);
            this.lblLogAppendFile.Name = "lblLogAppendFile";
            this.lblLogAppendFile.Size = new System.Drawing.Size(58, 16);
            this.lblLogAppendFile.TabIndex = 9;
            this.lblLogAppendFile.Text = "Log File:";
            this.lblLogAppendFile.Visible = false;
            // 
            // chkLogAppendEnable
            // 
            this.chkLogAppendEnable.AutoSize = true;
            this.chkLogAppendEnable.Location = new System.Drawing.Point(7, 85);
            this.chkLogAppendEnable.Name = "chkLogAppendEnable";
            this.chkLogAppendEnable.Size = new System.Drawing.Size(235, 20);
            this.chkLogAppendEnable.TabIndex = 8;
            this.chkLogAppendEnable.Text = "Append process output to a log file.";
            this.chkLogAppendEnable.UseVisualStyleBackColor = true;
            this.chkLogAppendEnable.CheckedChanged += new System.EventHandler(this.chkLogAppendEnable_CheckedChanged);
            // 
            // cmdLogFileBrowse
            // 
            this.cmdLogFileBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLogFileBrowse.Location = new System.Drawing.Point(480, 57);
            this.cmdLogFileBrowse.Name = "cmdLogFileBrowse";
            this.cmdLogFileBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdLogFileBrowse.TabIndex = 7;
            this.cmdLogFileBrowse.Text = "Browse";
            this.cmdLogFileBrowse.UseVisualStyleBackColor = true;
            this.cmdLogFileBrowse.Visible = false;
            this.cmdLogFileBrowse.Click += new System.EventHandler(this.cmdLogFileBrowse_Click);
            // 
            // txtLogFile
            // 
            this.txtLogFile.Location = new System.Drawing.Point(63, 57);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.Size = new System.Drawing.Size(411, 22);
            this.txtLogFile.TabIndex = 3;
            this.txtLogFile.Visible = false;
            // 
            // lblLogFile
            // 
            this.lblLogFile.AutoSize = true;
            this.lblLogFile.Location = new System.Drawing.Point(4, 60);
            this.lblLogFile.Name = "lblLogFile";
            this.lblLogFile.Size = new System.Drawing.Size(58, 16);
            this.lblLogFile.TabIndex = 2;
            this.lblLogFile.Text = "Log File:";
            this.lblLogFile.Visible = false;
            // 
            // chkLogEnable
            // 
            this.chkLogEnable.AutoSize = true;
            this.chkLogEnable.Location = new System.Drawing.Point(7, 35);
            this.chkLogEnable.Name = "chkLogEnable";
            this.chkLogEnable.Size = new System.Drawing.Size(218, 20);
            this.chkLogEnable.TabIndex = 1;
            this.chkLogEnable.Text = "Write process output to a log file.";
            this.chkLogEnable.UseVisualStyleBackColor = true;
            this.chkLogEnable.CheckedChanged += new System.EventHandler(this.chkLogEnable_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(3, 2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 24);
            this.label13.TabIndex = 0;
            this.label13.Text = "Logging";
            // 
            // pnl6SMTP
            // 
            this.pnl6SMTP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl6SMTP.Controls.Add(this.grpSMTPProfile);
            this.pnl6SMTP.Controls.Add(this.cmdSMTPProfileBrowse);
            this.pnl6SMTP.Controls.Add(this.txtSMTPProfile);
            this.pnl6SMTP.Controls.Add(this.lblSMTPProfile);
            this.pnl6SMTP.Controls.Add(this.chkSMTPSendEmail);
            this.pnl6SMTP.Controls.Add(this.label18);
            this.pnl6SMTP.Controls.Add(this.label15);
            this.pnl6SMTP.Location = new System.Drawing.Point(169, 12);
            this.pnl6SMTP.Margin = new System.Windows.Forms.Padding(4);
            this.pnl6SMTP.Name = "pnl6SMTP";
            this.pnl6SMTP.Size = new System.Drawing.Size(562, 476);
            this.pnl6SMTP.TabIndex = 15;
            this.pnl6SMTP.Tag = "SMTPOptions";
            // 
            // grpSMTPProfile
            // 
            this.grpSMTPProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSMTPProfile.Controls.Add(this.lblSMTPPassWarn);
            this.grpSMTPProfile.Controls.Add(this.cmdSMTPLoadProfile);
            this.grpSMTPProfile.Controls.Add(this.cmdSMTPSaveProfile);
            this.grpSMTPProfile.Controls.Add(this.txtSMTPBody);
            this.grpSMTPProfile.Controls.Add(this.chkSMTPAttachLog);
            this.grpSMTPProfile.Controls.Add(this.label26);
            this.grpSMTPProfile.Controls.Add(this.txtSMTPSubject);
            this.grpSMTPProfile.Controls.Add(this.label25);
            this.grpSMTPProfile.Controls.Add(this.txtSMTPEmailTo);
            this.grpSMTPProfile.Controls.Add(this.label23);
            this.grpSMTPProfile.Controls.Add(this.txtSMTPEmailFrom);
            this.grpSMTPProfile.Controls.Add(this.label24);
            this.grpSMTPProfile.Controls.Add(this.txtSMTPPass);
            this.grpSMTPProfile.Controls.Add(this.lblSMTPPass);
            this.grpSMTPProfile.Controls.Add(this.txtSMTPUser);
            this.grpSMTPProfile.Controls.Add(this.lblSMTPUser);
            this.grpSMTPProfile.Controls.Add(this.chkSMTPAuth);
            this.grpSMTPProfile.Controls.Add(this.chkSMTPTLS);
            this.grpSMTPProfile.Controls.Add(this.nudSMTPPort);
            this.grpSMTPProfile.Controls.Add(this.label20);
            this.grpSMTPProfile.Controls.Add(this.txtSMTPServer);
            this.grpSMTPProfile.Controls.Add(this.label19);
            this.grpSMTPProfile.Location = new System.Drawing.Point(7, 115);
            this.grpSMTPProfile.Name = "grpSMTPProfile";
            this.grpSMTPProfile.Size = new System.Drawing.Size(548, 358);
            this.grpSMTPProfile.TabIndex = 12;
            this.grpSMTPProfile.TabStop = false;
            this.grpSMTPProfile.Text = "SMTP Profile Editor";
            // 
            // lblSMTPPassWarn
            // 
            this.lblSMTPPassWarn.AutoSize = true;
            this.lblSMTPPassWarn.Location = new System.Drawing.Point(4, 333);
            this.lblSMTPPassWarn.Name = "lblSMTPPassWarn";
            this.lblSMTPPassWarn.Size = new System.Drawing.Size(231, 16);
            this.lblSMTPPassWarn.TabIndex = 21;
            this.lblSMTPPassWarn.Text = "*Profile file will contain your password.";
            this.lblSMTPPassWarn.Visible = false;
            // 
            // cmdSMTPLoadProfile
            // 
            this.cmdSMTPLoadProfile.Location = new System.Drawing.Point(297, 329);
            this.cmdSMTPLoadProfile.Name = "cmdSMTPLoadProfile";
            this.cmdSMTPLoadProfile.Size = new System.Drawing.Size(120, 23);
            this.cmdSMTPLoadProfile.TabIndex = 20;
            this.cmdSMTPLoadProfile.Text = "Load Profile File";
            this.cmdSMTPLoadProfile.UseVisualStyleBackColor = true;
            this.cmdSMTPLoadProfile.Click += new System.EventHandler(this.cmdSMTPLoadProfile_Click);
            // 
            // cmdSMTPSaveProfile
            // 
            this.cmdSMTPSaveProfile.Location = new System.Drawing.Point(423, 329);
            this.cmdSMTPSaveProfile.Name = "cmdSMTPSaveProfile";
            this.cmdSMTPSaveProfile.Size = new System.Drawing.Size(119, 23);
            this.cmdSMTPSaveProfile.TabIndex = 19;
            this.cmdSMTPSaveProfile.Text = "Save Profile File";
            this.cmdSMTPSaveProfile.UseVisualStyleBackColor = true;
            this.cmdSMTPSaveProfile.Click += new System.EventHandler(this.cmdSMTPSaveProfile_Click);
            // 
            // txtSMTPBody
            // 
            this.txtSMTPBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSMTPBody.Location = new System.Drawing.Point(3, 177);
            this.txtSMTPBody.Multiline = true;
            this.txtSMTPBody.Name = "txtSMTPBody";
            this.txtSMTPBody.Size = new System.Drawing.Size(540, 146);
            this.txtSMTPBody.TabIndex = 18;
            this.txtSMTPBody.Text = "Database Restore of {$DATABASENAME} on {$SQLSERVER} {$ENDDATETIME}\r\nResults: {$RE" +
    "SULT}\r\nSee attached log file for full details.\r\n\r\n";
            // 
            // chkSMTPAttachLog
            // 
            this.chkSMTPAttachLog.AutoSize = true;
            this.chkSMTPAttachLog.Checked = true;
            this.chkSMTPAttachLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSMTPAttachLog.Location = new System.Drawing.Point(430, 132);
            this.chkSMTPAttachLog.Name = "chkSMTPAttachLog";
            this.chkSMTPAttachLog.Size = new System.Drawing.Size(89, 20);
            this.chkSMTPAttachLog.TabIndex = 17;
            this.chkSMTPAttachLog.Text = "Attach Log";
            this.chkSMTPAttachLog.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(1, 158);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(163, 16);
            this.label26.TabIndex = 16;
            this.label26.Text = "Message Body Template:";
            // 
            // txtSMTPSubject
            // 
            this.txtSMTPSubject.Location = new System.Drawing.Point(85, 130);
            this.txtSMTPSubject.Name = "txtSMTPSubject";
            this.txtSMTPSubject.Size = new System.Drawing.Size(339, 22);
            this.txtSMTPSubject.TabIndex = 15;
            this.txtSMTPSubject.Text = "Database Restore Log {$SQLSERVER} {$DATABASENAME}";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(1, 133);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(83, 16);
            this.label25.TabIndex = 14;
            this.label25.Text = "Subject Line:";
            // 
            // txtSMTPEmailTo
            // 
            this.txtSMTPEmailTo.Location = new System.Drawing.Point(343, 102);
            this.txtSMTPEmailTo.Name = "txtSMTPEmailTo";
            this.txtSMTPEmailTo.Size = new System.Drawing.Size(179, 22);
            this.txtSMTPEmailTo.TabIndex = 13;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(273, 105);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(64, 16);
            this.label23.TabIndex = 12;
            this.label23.Text = "Email To:";
            // 
            // txtSMTPEmailFrom
            // 
            this.txtSMTPEmailFrom.Location = new System.Drawing.Point(85, 102);
            this.txtSMTPEmailFrom.Name = "txtSMTPEmailFrom";
            this.txtSMTPEmailFrom.Size = new System.Drawing.Size(179, 22);
            this.txtSMTPEmailFrom.TabIndex = 11;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(1, 105);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(78, 16);
            this.label24.TabIndex = 10;
            this.label24.Text = "Email From:";
            // 
            // txtSMTPPass
            // 
            this.txtSMTPPass.Location = new System.Drawing.Point(343, 74);
            this.txtSMTPPass.Name = "txtSMTPPass";
            this.txtSMTPPass.PasswordChar = '●';
            this.txtSMTPPass.Size = new System.Drawing.Size(179, 22);
            this.txtSMTPPass.TabIndex = 9;
            this.txtSMTPPass.Visible = false;
            // 
            // lblSMTPPass
            // 
            this.lblSMTPPass.AutoSize = true;
            this.lblSMTPPass.Location = new System.Drawing.Point(270, 78);
            this.lblSMTPPass.Name = "lblSMTPPass";
            this.lblSMTPPass.Size = new System.Drawing.Size(72, 16);
            this.lblSMTPPass.TabIndex = 8;
            this.lblSMTPPass.Text = "Password*";
            this.lblSMTPPass.Visible = false;
            // 
            // txtSMTPUser
            // 
            this.txtSMTPUser.Location = new System.Drawing.Point(85, 74);
            this.txtSMTPUser.Name = "txtSMTPUser";
            this.txtSMTPUser.Size = new System.Drawing.Size(179, 22);
            this.txtSMTPUser.TabIndex = 7;
            this.txtSMTPUser.Visible = false;
            // 
            // lblSMTPUser
            // 
            this.lblSMTPUser.AutoSize = true;
            this.lblSMTPUser.Location = new System.Drawing.Point(6, 77);
            this.lblSMTPUser.Name = "lblSMTPUser";
            this.lblSMTPUser.Size = new System.Drawing.Size(73, 16);
            this.lblSMTPUser.TabIndex = 6;
            this.lblSMTPUser.Text = "Username:";
            this.lblSMTPUser.Visible = false;
            // 
            // chkSMTPAuth
            // 
            this.chkSMTPAuth.AutoSize = true;
            this.chkSMTPAuth.Location = new System.Drawing.Point(9, 53);
            this.chkSMTPAuth.Name = "chkSMTPAuth";
            this.chkSMTPAuth.Size = new System.Drawing.Size(251, 20);
            this.chkSMTPAuth.TabIndex = 5;
            this.chkSMTPAuth.Text = "SMTP Server Requires Authentication";
            this.chkSMTPAuth.UseVisualStyleBackColor = true;
            this.chkSMTPAuth.CheckedChanged += new System.EventHandler(this.chkSMTPAuth_CheckedChanged);
            // 
            // chkSMTPTLS
            // 
            this.chkSMTPTLS.AutoSize = true;
            this.chkSMTPTLS.Location = new System.Drawing.Point(460, 27);
            this.chkSMTPTLS.Name = "chkSMTPTLS";
            this.chkSMTPTLS.Size = new System.Drawing.Size(51, 20);
            this.chkSMTPTLS.TabIndex = 4;
            this.chkSMTPTLS.Text = "TLS";
            this.chkSMTPTLS.UseVisualStyleBackColor = true;
            // 
            // nudSMTPPort
            // 
            this.nudSMTPPort.Location = new System.Drawing.Point(389, 26);
            this.nudSMTPPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudSMTPPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSMTPPort.Name = "nudSMTPPort";
            this.nudSMTPPort.Size = new System.Drawing.Size(59, 22);
            this.nudSMTPPort.TabIndex = 3;
            this.nudSMTPPort.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(349, 29);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(34, 16);
            this.label20.TabIndex = 2;
            this.label20.Text = "Port:";
            // 
            // txtSMTPServer
            // 
            this.txtSMTPServer.Location = new System.Drawing.Point(103, 25);
            this.txtSMTPServer.Name = "txtSMTPServer";
            this.txtSMTPServer.Size = new System.Drawing.Size(240, 22);
            this.txtSMTPServer.TabIndex = 1;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 28);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(91, 16);
            this.label19.TabIndex = 0;
            this.label19.Text = "SMTP Server:";
            // 
            // cmdSMTPProfileBrowse
            // 
            this.cmdSMTPProfileBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSMTPProfileBrowse.Location = new System.Drawing.Point(480, 87);
            this.cmdSMTPProfileBrowse.Name = "cmdSMTPProfileBrowse";
            this.cmdSMTPProfileBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdSMTPProfileBrowse.TabIndex = 11;
            this.cmdSMTPProfileBrowse.Text = "Browse";
            this.cmdSMTPProfileBrowse.UseVisualStyleBackColor = true;
            this.cmdSMTPProfileBrowse.Visible = false;
            // 
            // txtSMTPProfile
            // 
            this.txtSMTPProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSMTPProfile.Location = new System.Drawing.Point(99, 87);
            this.txtSMTPProfile.Name = "txtSMTPProfile";
            this.txtSMTPProfile.Size = new System.Drawing.Size(375, 22);
            this.txtSMTPProfile.TabIndex = 10;
            this.txtSMTPProfile.Visible = false;
            // 
            // lblSMTPProfile
            // 
            this.lblSMTPProfile.AutoSize = true;
            this.lblSMTPProfile.Location = new System.Drawing.Point(4, 90);
            this.lblSMTPProfile.Name = "lblSMTPProfile";
            this.lblSMTPProfile.Size = new System.Drawing.Size(89, 16);
            this.lblSMTPProfile.TabIndex = 9;
            this.lblSMTPProfile.Text = "SMTP Profile:";
            this.lblSMTPProfile.Visible = false;
            // 
            // chkSMTPSendEmail
            // 
            this.chkSMTPSendEmail.AutoSize = true;
            this.chkSMTPSendEmail.Location = new System.Drawing.Point(7, 68);
            this.chkSMTPSendEmail.Name = "chkSMTPSendEmail";
            this.chkSMTPSendEmail.Size = new System.Drawing.Size(153, 20);
            this.chkSMTPSendEmail.TabIndex = 2;
            this.chkSMTPSendEmail.Text = "Enable SMTP profile:";
            this.chkSMTPSendEmail.UseVisualStyleBackColor = true;
            this.chkSMTPSendEmail.CheckedChanged += new System.EventHandler(this.chkSMTPSendEmail_CheckedChanged);
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.Location = new System.Drawing.Point(4, 29);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(551, 41);
            this.label18.TabIndex = 1;
            this.label18.Text = "To email log files, you must first create a SMTP profile and save it somewhere. T" +
    "hen the command line argument can specify that profile file.";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(3, 2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(132, 24);
            this.label15.TabIndex = 0;
            this.label15.Text = "SMTP Options";
            // 
            // pnl7CLI
            // 
            this.pnl7CLI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl7CLI.Controls.Add(this.label16);
            this.pnl7CLI.Location = new System.Drawing.Point(169, 12);
            this.pnl7CLI.Margin = new System.Windows.Forms.Padding(4);
            this.pnl7CLI.Name = "pnl7CLI";
            this.pnl7CLI.Size = new System.Drawing.Size(562, 476);
            this.pnl7CLI.TabIndex = 16;
            this.pnl7CLI.Tag = "CLIArguments";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(3, 2);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(312, 24);
            this.label16.TabIndex = 0;
            this.label16.Text = "Command Line Interface Arguments";
            // 
            // pnl8Run
            // 
            this.pnl8Run.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl8Run.Controls.Add(this.label17);
            this.pnl8Run.Location = new System.Drawing.Point(169, 12);
            this.pnl8Run.Margin = new System.Windows.Forms.Padding(4);
            this.pnl8Run.Name = "pnl8Run";
            this.pnl8Run.Size = new System.Drawing.Size(562, 476);
            this.pnl8Run.TabIndex = 17;
            this.pnl8Run.Tag = "Run";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 2);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(242, 24);
            this.label17.TabIndex = 0;
            this.label17.Text = "Run Database Restore Now";
            // 
            // lblRightsHelp
            // 
            this.lblRightsHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRightsHelp.Location = new System.Drawing.Point(3, 404);
            this.lblRightsHelp.Name = "lblRightsHelp";
            this.lblRightsHelp.Size = new System.Drawing.Size(553, 66);
            this.lblRightsHelp.TabIndex = 4;
            this.lblRightsHelp.Text = resources.GetString("lblRightsHelp.Text");
            this.lblRightsHelp.Visible = false;
            // 
            // cmdStartLoadSettings
            // 
            this.cmdStartLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdStartLoadSettings.Location = new System.Drawing.Point(408, 435);
            this.cmdStartLoadSettings.Name = "cmdStartLoadSettings";
            this.cmdStartLoadSettings.Size = new System.Drawing.Size(148, 32);
            this.cmdStartLoadSettings.TabIndex = 3;
            this.cmdStartLoadSettings.Text = "Load Saved Settings";
            this.cmdStartLoadSettings.UseVisualStyleBackColor = true;
            this.cmdStartLoadSettings.Click += new System.EventHandler(this.cmdStartLoadSettings_Click);
            // 
            // cmdStartSaveSettings
            // 
            this.cmdStartSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdStartSaveSettings.Location = new System.Drawing.Point(254, 435);
            this.cmdStartSaveSettings.Name = "cmdStartSaveSettings";
            this.cmdStartSaveSettings.Size = new System.Drawing.Size(148, 32);
            this.cmdStartSaveSettings.TabIndex = 4;
            this.cmdStartSaveSettings.Text = "Save Current Settings";
            this.cmdStartSaveSettings.UseVisualStyleBackColor = true;
            this.cmdStartSaveSettings.Click += new System.EventHandler(this.cmdStartSaveSettings_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 501);
            this.Controls.Add(this.pnl6SMTP);
            this.Controls.Add(this.btn4Rights);
            this.Controls.Add(this.btn8Run);
            this.Controls.Add(this.btn7CLI);
            this.Controls.Add(this.btn6SMTP);
            this.Controls.Add(this.btn5Log);
            this.Controls.Add(this.btn3Opts);
            this.Controls.Add(this.btn2SQL);
            this.Controls.Add(this.btn1Soruce);
            this.Controls.Add(this.btn0Start);
            this.Controls.Add(this.pnl5Log);
            this.Controls.Add(this.pnl0Start);
            this.Controls.Add(this.pnl3Opts);
            this.Controls.Add(this.pnl2SQL);
            this.Controls.Add(this.pnl1Source);
            this.Controls.Add(this.pnl8Run);
            this.Controls.Add(this.pnl7CLI);
            this.Controls.Add(this.pnl4Rights);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Restore GUI";
            this.pnl0Start.ResumeLayout(false);
            this.pnl0Start.PerformLayout();
            this.pnl1Source.ResumeLayout(false);
            this.pnl1Source.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlSourceAutoSelect.ResumeLayout(false);
            this.pnlSourceAutoSelect.PerformLayout();
            this.pnlSourceManualSelect.ResumeLayout(false);
            this.pnlSourceManualSelect.PerformLayout();
            this.pnl2SQL.ResumeLayout(false);
            this.pnl2SQL.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSQLPort)).EndInit();
            this.pnl3Opts.ResumeLayout(false);
            this.pnl3Opts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptsMoveFile)).EndInit();
            this.pnl4Rights.ResumeLayout(false);
            this.pnl4Rights.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRightsUsers)).EndInit();
            this.pnl5Log.ResumeLayout(false);
            this.pnl5Log.PerformLayout();
            this.pnl6SMTP.ResumeLayout(false);
            this.pnl6SMTP.PerformLayout();
            this.grpSMTPProfile.ResumeLayout(false);
            this.grpSMTPProfile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSMTPPort)).EndInit();
            this.pnl7CLI.ResumeLayout(false);
            this.pnl7CLI.PerformLayout();
            this.pnl8Run.ResumeLayout(false);
            this.pnl8Run.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl0Start;
        private System.Windows.Forms.Label lbl1Version;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox btn0Start;
        private System.Windows.Forms.CheckBox btn1Soruce;
        private System.Windows.Forms.Panel pnl1Source;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox btn2SQL;
        private System.Windows.Forms.CheckBox btn3Opts;
        private System.Windows.Forms.CheckBox btn5Log;
        private System.Windows.Forms.CheckBox btn6SMTP;
        private System.Windows.Forms.CheckBox btn7CLI;
        private System.Windows.Forms.CheckBox btn8Run;
        private System.Windows.Forms.ComboBox cmbSourceSelectionMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlSourceAutoSelect;
        private System.Windows.Forms.Button cmdSourceAutoBrowse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSourceAutoPath;
        private System.Windows.Forms.ComboBox cmbAutoSourceAttribute;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnlSourceManualSelect;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button cmdSourceManualBrowse;
        private System.Windows.Forms.TextBox txtSourceFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkSourceTempEnable;
        private System.Windows.Forms.Label lblSourceTempHelp;
        private System.Windows.Forms.Button cmdSourceTempFileBrowse;
        private System.Windows.Forms.TextBox txtSourceTempFilePath;
        private System.Windows.Forms.Panel pnl2SQL;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSQLServerName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button cmdSQLServerNameHelp;
        private System.Windows.Forms.NumericUpDown nudSQLPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button cmdSQLDatabaseNameQuery;
        private System.Windows.Forms.Button cmdSQLDatabaseNameHelp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSQLPassword;
        private System.Windows.Forms.Label lblSQLPassword;
        private System.Windows.Forms.TextBox txtSQLUsername;
        private System.Windows.Forms.Label lblSQLUsername;
        private System.Windows.Forms.CheckBox chkSQLUseCredentials;
        private System.Windows.Forms.Label lblSQLCredentialInfo;
        private System.Windows.Forms.ComboBox cmbSQLDatabaseName;
        private System.Windows.Forms.CheckBox btn4Rights;
        private System.Windows.Forms.Panel pnl3Opts;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel pnl4Rights;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel pnl5Log;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel pnl6SMTP;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel pnl7CLI;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel pnl8Run;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chkOptsReplace;
        private System.Windows.Forms.CheckBox chkOptsMoveall;
        private System.Windows.Forms.CheckBox chkOptsCheckdb;
        private System.Windows.Forms.Button cmdOptsMoveAllPathHelp;
        private System.Windows.Forms.Button cmdOptsMoveAllBrowse;
        private System.Windows.Forms.TextBox txtOptsMoveallPath;
        private System.Windows.Forms.Label lblOptsMoveallPath;
        private System.Windows.Forms.CheckBox chkOptsMoveFile;
        private System.Windows.Forms.DataGridView dgvOptsMoveFile;
        private System.Windows.Forms.Button cmdOptsMoveFileImport;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogicalName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewPath;
        private System.Windows.Forms.Button cmdRightsImportLogins;
        private System.Windows.Forms.CheckBox chkRightsEnable;
        private System.Windows.Forms.DataGridView dgvRightsUsers;
        private System.Windows.Forms.Button cmdLogFileBrowse;
        private System.Windows.Forms.TextBox txtLogFile;
        private System.Windows.Forms.Label lblLogFile;
        private System.Windows.Forms.CheckBox chkLogEnable;
        private System.Windows.Forms.Button cmdLogAppendBrowse;
        private System.Windows.Forms.TextBox txtLogAppendFile;
        private System.Windows.Forms.Label lblLogAppendFile;
        private System.Windows.Forms.CheckBox chkLogAppendEnable;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button cmdSMTPProfileBrowse;
        private System.Windows.Forms.TextBox txtSMTPProfile;
        private System.Windows.Forms.Label lblSMTPProfile;
        private System.Windows.Forms.CheckBox chkSMTPSendEmail;
        private System.Windows.Forms.GroupBox grpSMTPProfile;
        private System.Windows.Forms.NumericUpDown nudSMTPPort;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtSMTPServer;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox chkSMTPTLS;
        private System.Windows.Forms.CheckBox chkSMTPAuth;
        private System.Windows.Forms.TextBox txtSMTPPass;
        private System.Windows.Forms.Label lblSMTPPass;
        private System.Windows.Forms.TextBox txtSMTPUser;
        private System.Windows.Forms.Label lblSMTPUser;
        private System.Windows.Forms.TextBox txtSMTPEmailTo;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtSMTPEmailFrom;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.CheckBox chkSMTPAttachLog;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtSMTPSubject;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtSMTPBody;
        private System.Windows.Forms.Button cmdSMTPLoadProfile;
        private System.Windows.Forms.Button cmdSMTPSaveProfile;
        private System.Windows.Forms.Label lblSMTPPassWarn;
        private System.Windows.Forms.Label lblRightsHelp;
        private System.Windows.Forms.Button cmdStartLoadSettings;
        private System.Windows.Forms.Button cmdStartSaveSettings;
    }
}