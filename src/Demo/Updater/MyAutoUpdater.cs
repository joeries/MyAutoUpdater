using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Demo.Updater
{
    public class MyAutoUpdater : IUpdater
    {
        /// <summary>
        /// 运行自动升级程序
        /// </summary>
        public bool Run()
        {
            string updaterFile = Path.Combine(Application.StartupPath, "MyAutoUpdater.exe");
            if (!File.Exists(updaterFile))
            {
                return false;
            }

            string updaterUrl = "http://127.0.0.1/server/updater.xml";//此URL为升级配置文件的存放位置
            string appName = "Demo";//应用软件名称
            string curVersion = "1.0";//当前版本号
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = updaterFile;
            processInfo.WorkingDirectory = Application.StartupPath;
            processInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", appName, curVersion, updaterUrl, Application.ExecutablePath, "true");

            Process process = new Process();
            process.StartInfo = processInfo;
            process.Start();
            
            return true;
        }
    }
}