using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FileWatcherWinForms
{
    
    public partial class CloudDrive : Form
    {
        string currentUser;
        string currentTokenUser;
        public CloudDrive()
        {
            InitializeComponent();
            AutoRunOnWindowsStartup();
            WindowAppearance();
        }



        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bool cursorNotInBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);

            if (this.WindowState == FormWindowState.Minimized && cursorNotInBar)
            {
                this.ShowInTaskbar = false;
                Cloud.Visible = true;
                this.Hide();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            Cloud.Visible = false;
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

        private async void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            String filename = Path.GetFileName(e.FullPath);
            var res = await RestHelper.UploadFile(e.FullPath, currentTokenUser, filename);
            SaveLog(value);

        }

        private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
        {
            SaveLog($"Deleted: {e.FullPath}");
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            SaveLog($"Renamed:");
            SaveLog($"    Old: {e.OldFullPath}");
            SaveLog($"    New: {e.FullPath}");
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

        private void button_edit_Click(object sender, EventArgs e)
        {
            button_edit.Visible = false;
            button_save.Visible = true;
            observedPath.ReadOnly = false;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            button_save.Visible = false;
            button_edit.Visible = true;
            observedPath.ReadOnly = true;
            fileSystemWatcher1.Path = observedPath.Text;
        }

        private void AutoRunOnWindowsStartup()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            reg.SetValue("Cloud Drive", Application.ExecutablePath.ToString());
        }

        private void CloudDrive_Load(object sender, EventArgs e)
        {

        }

        private void WindowAppearance()
        {
            this.BackColor = Color.FromArgb(6, 37, 63);
            button_edit.Visible = false;
            observedPath.Visible = false;
            login.BackColor = Color.FromArgb(89, 159, 216);
            login.ForeColor = Color.FromArgb(29, 29, 29);
            password.PasswordChar = '*';
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            username.Select();
            //this.StartPosition = FormStartPosition.CenterParent;

        }

        private async void login_Click(object sender, EventArgs e)
        {
            User user = new User();
            UserDTO userDTO=null;
            user.username = username.Text;
            user.password = password.Text;
            var response = await RestHelper.Login(user);
            //Debug.WriteLine(response);
            if (!response.Contains("Nieprawid³owa nazwa u¿ytkownika"))
            {
                userDTO = JsonSerializer.Deserialize<UserDTO>(response);
            }
            //Debug.WriteLine(userDTO.token);
            if (userDTO != null && userDTO.token != null && userDTO.username != null)
            {
                nameApp.Visible = false;
                username.Visible = false;
                password.Visible = false;
                login.Visible = false;
                button_edit.Visible = true;
                observedPath.Visible = true;
                fileSystemWatcher1.EnableRaisingEvents = true;
                currentUser = userDTO.username;
                currentTokenUser = userDTO.token;
                //var res = await RestHelper.UploadFile("E:\\PlikTestowy.txt", userDTO.token);
                //Debug.WriteLine("Witaj");
                //Debug.WriteLine(res);
            }
            else MessageBox.Show(this, "B³êdna nazwa u¿ytkownika, lub has³o.");


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}