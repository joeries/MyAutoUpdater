using MyAutoUpdater.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MyAutoUpdater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (null == args || args.Length < 5)
            {
                Logger.Log("ERROR", "启动参数缺失");
                return;
            }
            Constants.MainExeName = args[0].Trim();
            Constants.CurVersion = args[1].Trim();
            Constants.UpdaterUrl = args[2].Trim();
            Constants.MainExePath = args[3].Trim();
            Constants.Silent = bool.Parse(args[4].Trim());

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogException(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogException((Exception)(e.ExceptionObject));
        }

        /// <summary>
        /// 记录代码异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        static void LogException(Exception ex)
        {
            Logger.Log("ERROR", ex.Message);

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
    }
}
