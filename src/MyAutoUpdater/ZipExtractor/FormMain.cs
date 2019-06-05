using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace ZipExtractor
{
    public partial class FormMain : Form
    {
        private BackgroundWorker _backgroundWorker;
        private string zipFile;
        private string mianExeFile;

        public FormMain(string _zipFile, string _mianExeFile)
        {
            InitializeComponent();
            this.zipFile = _zipFile;
            this.mianExeFile = _mianExeFile;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            //foreach (var process in Process.GetProcesses())
            //{
            //    try
            //    {
            //        if (process.MainModule.FileName.Equals(mianExeFile))
            //        {
            //            labelInformation.Text = @"Waiting for application to Exit...";
            //            process.WaitForExit();
            //        }
            //    }
            //    catch (Exception exception)
            //    {
            //        Debug.WriteLine(exception.Message);
            //    }
            //}

            // Extract all the files.
            _backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _backgroundWorker.DoWork += (o, eventArgs) =>
            {
                var path = Path.GetDirectoryName(mianExeFile);

                // Open an existing zip file for reading.
                ZipStorer zip = ZipStorer.Open(zipFile, FileAccess.Read);

                // Read the central directory collection.
                List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();

                for (var index = 0; index < dir.Count; index++)
                {
                    if (_backgroundWorker.CancellationPending)
                    {
                        eventArgs.Cancel = true;
                        zip.Close();
                        return;
                    }
                    ZipStorer.ZipFileEntry entry = dir[index];
                    zip.ExtractFile(entry, Path.Combine(path, entry.FilenameInZip));
                    _backgroundWorker.ReportProgress((index + 1) * 100 / dir.Count, string.Format(MyAutoUpdater.Properties.Resources.CurrentFileExtracting, entry.FilenameInZip));
                }

                zip.Close();
            };

            _backgroundWorker.ProgressChanged += (o, eventArgs) =>
            {
                progressBar.Value = eventArgs.ProgressPercentage;
                labelInformation.Text = eventArgs.UserState.ToString();
            };

            _backgroundWorker.RunWorkerCompleted += (o, eventArgs) =>
            {
                if (!eventArgs.Cancelled)
                {
                    labelInformation.Text = @"Finished";
                    //try
                    //{
                    //    ProcessStartInfo processStartInfo = new ProcessStartInfo(args[2]);
                    //    if (args.Length > 3)
                    //    {
                    //        processStartInfo.Arguments = args[3];
                    //    }
                    //    Process.Start(processStartInfo);
                    //}
                    //catch (Win32Exception exception)
                    //{
                    //    if (exception.NativeErrorCode != 1223)
                    //        throw;
                    //}
                    Close();
                }
            };
            _backgroundWorker.RunWorkerAsync();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _backgroundWorker?.CancelAsync();
            DialogResult = DialogResult.OK;
        }
    }
}