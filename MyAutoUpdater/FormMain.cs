using AutoUpdaterDotNET;
using MyAutoUpdater.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MyAutoUpdater
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.Text = string.Format("{0} {1}", this.Text, Assembly.GetEntryAssembly().GetName().Version);
            labelVersion.Text = string.Format("{0}{1}", labelVersion.Text, Constants.CurVersion);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Uncomment below lines to handle parsing logic of non XML AppCast file.
            //AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
            //AutoUpdater.Start("https://rbsoft.org/updates/AutoUpdater.json");

            //Uncomment below line to run update process using non administrator account.
            AutoUpdater.RunUpdateAsAdmin = false;

            //Uncomment below line to see russian version
            //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ru");

            //If you want to open download page when user click on download button uncomment below line.
            //AutoUpdater.OpenDownloadPage = true;

            //Don't want user to select remind later time in AutoUpdater notification window then uncomment 3 lines below so default remind later time will be set to 2 days.
            AutoUpdater.LetUserSelectRemindLater = false;
            AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Minutes;
            AutoUpdater.RemindLaterAt = 1;

            //Don't want to show Skip button then uncomment below line.
            AutoUpdater.ShowSkipButton = false;

            //Don't want to show Remind Later button then uncomment below line.
            AutoUpdater.ShowRemindLaterButton = false;

            //Want to show custom application title then uncomment below line.
            AutoUpdater.AppTitle = Constants.MainExeName;

            //Want to show errors then uncomment below line.
            //AutoUpdater.ReportErrors = true;

            //Want to handle how your application will exit when application finished downloading then uncomment below line.
            AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;

            if (Constants.Silent)
            {
                //Want to handle update logic yourself then uncomment below line.
                AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            }

            //Want to use XML and Update file served only through Proxy.
            //var proxy = new WebProxy("localproxyIP:8080", true) {Credentials = new NetworkCredential("domain\\user", "password")};
            //AutoUpdater.Proxy = proxy;

            //Want to check for updates frequently then uncomment following lines.
            //System.Timers.Timer timer = new System.Timers.Timer
            //{
            //    Interval = 2 * 60 * 1000,
            //    SynchronizingObject = this
            //};
            //timer.Elapsed += delegate
            //{
            //    AutoUpdater.Start("https://rbsoft.org/updates/AutoUpdater.xml");
            //};
            //timer.Start();            
        }

        private void AutoUpdater_ApplicationExitEvent()
        {
            if (File.Exists(Constants.MainExePath))
            {
                Process.Start(Constants.MainExePath);
            }
            else
            {
                Logger.Log("WARN", "主程序不存在");
            }
            Application.Exit();
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args == null)
            {
                Logger.Log("INFO", "网络错误或服务器错误");
            }
            else if (!args.IsUpdateAvailable)
            {
                Logger.Log("INFO", "未检测到更新");
            }
            else
            {
                try
                {
                    AutoUpdater.DownloadUpdate();
                }
                catch (Exception exception)
                {
                    Logger.Log("ERROR", exception.Message);
                }
            }
            AutoUpdater_ApplicationExitEvent();
        }

        private void ButtonCheckForUpdate_Click(object sender, EventArgs e)
        {
            //Uncomment below lines to select download path where update is saved.
            //FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            //if (folderBrowserDialog.ShowDialog().Equals(DialogResult.OK))
            //{
            //    AutoUpdater.DownloadPath = folderBrowserDialog.SelectedPath;
            //    AutoUpdater.Mandatory = true;
            //    AutoUpdater.Start("https://rbsoft.org/updates/AutoUpdater.xml");
            //}

            buttonCheckForUpdate.Enabled = false;
            buttonCheckForUpdate.Text = "正在检测新版本...";
            AutoUpdater.Mandatory = true;
            AutoUpdater.Start(Constants.UpdaterUrl);
        }

        private void timerInit_Tick(object sender, EventArgs e)
        {
            timerInit.Stop();
            buttonCheckForUpdate.PerformClick();
        }

        //private void MyAutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        //{
        //    dynamic json = JsonConvert.DeserializeObject(args.RemoteData);
        //    args.UpdateInfo = new UpdateInfoEventArgs
        //    {
        //        CurrentVersion = json.version,
        //        ChangelogURL = json.changelog,
        //        Mandatory = json.mandatory,
        //        DownloadURL = json.url
        //    };
        //}
    }
}
