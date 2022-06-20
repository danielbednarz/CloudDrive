namespace FileWatcherWinForms
{
    partial class CloudDrive
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CloudDrive));
            this.button_save = new System.Windows.Forms.Button();
            this.Cloud = new System.Windows.Forms.NotifyIcon(this.components);
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.observedPath = new System.Windows.Forms.TextBox();
            this.button_edit = new System.Windows.Forms.Button();
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.login = new System.Windows.Forms.Button();
            this.nameApp = new System.Windows.Forms.Label();
            this.checkBoxRemember = new System.Windows.Forms.CheckBox();
            this.buttonLogOut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(424, 29);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(150, 23);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "Zapisz";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Visible = false;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // Cloud
            // 
            this.Cloud.Icon = ((System.Drawing.Icon)(resources.GetObject("Cloud.Icon")));
            this.Cloud.Text = "notifyIcon1";
            this.Cloud.Visible = true;
            this.Cloud.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.IncludeSubdirectories = true;
            this.fileSystemWatcher1.Path = "E:\\test";
            this.fileSystemWatcher1.SynchronizingObject = this;
            this.fileSystemWatcher1.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Changed);
            this.fileSystemWatcher1.Created += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Created);
            this.fileSystemWatcher1.Deleted += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Deleted);
            this.fileSystemWatcher1.Error += new System.IO.ErrorEventHandler(this.fileSystemWatcher1_Error);
            this.fileSystemWatcher1.Renamed += new System.IO.RenamedEventHandler(this.fileSystemWatcher1_Renamed);
            // 
            // observedPath
            // 
            this.observedPath.Location = new System.Drawing.Point(56, 29);
            this.observedPath.Name = "observedPath";
            this.observedPath.ReadOnly = true;
            this.observedPath.Size = new System.Drawing.Size(340, 23);
            this.observedPath.TabIndex = 1;
            // 
            // button_edit
            // 
            this.button_edit.Location = new System.Drawing.Point(424, 29);
            this.button_edit.Name = "button_edit";
            this.button_edit.Size = new System.Drawing.Size(150, 23);
            this.button_edit.TabIndex = 2;
            this.button_edit.Text = "Edytuj";
            this.button_edit.UseVisualStyleBackColor = true;
            this.button_edit.Click += new System.EventHandler(this.button_edit_Click);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(410, 179);
            this.password.Name = "password";
            this.password.PlaceholderText = "hasło";
            this.password.Size = new System.Drawing.Size(160, 23);
            this.password.TabIndex = 3;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(150, 179);
            this.username.Name = "username";
            this.username.PlaceholderText = "nazwa użytkownika";
            this.username.Size = new System.Drawing.Size(160, 23);
            this.username.TabIndex = 4;
            // 
            // login
            // 
            this.login.FlatAppearance.BorderSize = 0;
            this.login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.login.Location = new System.Drawing.Point(280, 229);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(160, 23);
            this.login.TabIndex = 5;
            this.login.Text = "Zaloguj się";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // nameApp
            // 
            this.nameApp.AutoSize = true;
            this.nameApp.Font = new System.Drawing.Font("Segoe UI", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nameApp.ForeColor = System.Drawing.Color.White;
            this.nameApp.Location = new System.Drawing.Point(150, 29);
            this.nameApp.Name = "nameApp";
            this.nameApp.Size = new System.Drawing.Size(436, 106);
            this.nameApp.TabIndex = 6;
            this.nameApp.Text = "CloudDrive";
            this.nameApp.Click += new System.EventHandler(this.label1_Click);
            // 
            // checkBoxRemember
            // 
            this.checkBoxRemember.AutoSize = true;
            this.checkBoxRemember.ForeColor = System.Drawing.Color.White;
            this.checkBoxRemember.Location = new System.Drawing.Point(313, 258);
            this.checkBoxRemember.Name = "checkBoxRemember";
            this.checkBoxRemember.Size = new System.Drawing.Size(102, 19);
            this.checkBoxRemember.TabIndex = 7;
            this.checkBoxRemember.Text = "Pamiętaj mnie";
            this.checkBoxRemember.UseVisualStyleBackColor = true;
            this.checkBoxRemember.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // buttonLogOut
            // 
            this.buttonLogOut.Location = new System.Drawing.Point(626, -3);
            this.buttonLogOut.Name = "buttonLogOut";
            this.buttonLogOut.Size = new System.Drawing.Size(75, 23);
            this.buttonLogOut.TabIndex = 8;
            this.buttonLogOut.Text = "Wyloguj";
            this.buttonLogOut.UseVisualStyleBackColor = true;
            this.buttonLogOut.Visible = false;
            this.buttonLogOut.Click += new System.EventHandler(this.buttonLogOut_Click);
            // 
            // CloudDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(704, 301);
            this.Controls.Add(this.buttonLogOut);
            this.Controls.Add(this.checkBoxRemember);
            this.Controls.Add(this.nameApp);
            this.Controls.Add(this.login);
            this.Controls.Add(this.username);
            this.Controls.Add(this.password);
            this.Controls.Add(this.button_edit);
            this.Controls.Add(this.observedPath);
            this.Controls.Add(this.button_save);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CloudDrive";
            this.Text = "CloudDrive";
            this.Load += new System.EventHandler(this.CloudDrive_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button_save;
        private NotifyIcon Cloud;
        private FileSystemWatcher fileSystemWatcher1;
        private TextBox observedPath;
        private Button button_edit;
        private Button login;
        private TextBox username;
        private TextBox password;
        private Label nameApp;
        private CheckBox checkBoxRemember;
        private Button buttonLogOut;
    }
}