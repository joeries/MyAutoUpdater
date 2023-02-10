using Demo.Updater;
using System;
using System.Windows.Forms;

namespace demo
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IUpdater updater = UpdaterHelper.GetInstance("MyAuto");
            if (updater.Run())
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
