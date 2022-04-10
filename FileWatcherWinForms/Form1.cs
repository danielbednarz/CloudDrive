using System;
using System.ComponentModel;
using System.IO;

namespace FileWatcherWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bool cursorNotInBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);

            if (this.WindowState == FormWindowState.Minimized && cursorNotInBar)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                this.Hide();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            this.Visible = true;
        }

        static void SaveLog(string log)
        {
 
            string docPath = "E:\\ClientFW\\log";
            DateTime thisDay = DateTime.Now;

            if (!Directory.Exists(docPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(docPath);
            }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "LogFileWathcer_" + thisDay.ToString("d").Replace('/', '_') + ".txt"), true))
            {
                outputFile.WriteLine("[" + thisDay.ToString() + "]" + log);
            }
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            SaveLog($"Changed: {e.FullPath}");

        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            SaveLog(value);

        }

        private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
        {
            SaveLog($"Deleted: {e.FullPath}");
        }

        private void fileSystemWatcher1_Error(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                SaveLog($"Message: {ex.Message}");
                SaveLog("Stacktrace:");
                SaveLog(ex.StackTrace);
                SaveLog(" ");
                PrintException(ex.InnerException);
            }
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            SaveLog($"Renamed:");
            SaveLog($"    Old: {e.OldFullPath}");
            SaveLog($"    New: {e.FullPath}");
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            button_edit.Visible = false;
            button_save.Visible = true;
            textBox1.ReadOnly = false;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            button_save.Visible = false;
            button_edit.Visible = true;
            textBox1.ReadOnly = true;
            fileSystemWatcher1.Path = textBox1.Text;
        }
    }
}