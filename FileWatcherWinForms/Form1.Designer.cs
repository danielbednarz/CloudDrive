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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.observedPath = new System.Windows.Forms.TextBox();
            this.button_edit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(485, 39);
            this.button_save.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(171, 31);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "Zapisz";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Visible = false;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
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
            this.observedPath.Location = new System.Drawing.Point(64, 39);
            this.observedPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.observedPath.Name = "observedPath";
            this.observedPath.ReadOnly = true;
            this.observedPath.Size = new System.Drawing.Size(388, 27);
            this.observedPath.TabIndex = 1;
            this.observedPath.Text = "E:\\test";
            // 
            // button_edit
            // 
            this.button_edit.Location = new System.Drawing.Point(485, 39);
            this.button_edit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_edit.Name = "button_edit";
            this.button_edit.Size = new System.Drawing.Size(171, 31);
            this.button_edit.TabIndex = 2;
            this.button_edit.Text = "Edytuj";
            this.button_edit.UseVisualStyleBackColor = true;
            this.button_edit.Click += new System.EventHandler(this.button_edit_Click);
            // 
            // CloudDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 405);
            this.Controls.Add(this.button_edit);
            this.Controls.Add(this.observedPath);
            this.Controls.Add(this.button_save);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CloudDrive";
            this.Text = "CloudDrive";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button_save;
        private NotifyIcon notifyIcon1;
        private FileSystemWatcher fileSystemWatcher1;
        private TextBox observedPath;
        private Button button_edit;
    }
}