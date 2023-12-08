using System;
using System.Threading.Tasks;
using System.IO;
using System.Threading.Channels;
using System.Windows.Forms;

namespace DiplomskiRad
{
    class Watcher
    {

        Cipher c;
        private RichTextBox txtLog;
        private readonly string inputFolder;
        private readonly Channel<string> createdFilesChannel = Channel.CreateUnbounded<string>();

        public Watcher(string inputFolder, RichTextBox txtLog)
        {
            this.inputFolder = inputFolder;
            this.txtLog = txtLog;
            c = new Cipher();
        }

        public async Task WatchForNewFilesAsync()
        {
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher(inputFolder);
                watcher.NotifyFilter = NotifyFilters.FileName;
                watcher.EnableRaisingEvents = true;
                watcher.IncludeSubdirectories = false;
                watcher.Created += Watcher_Created;

                await ProcessNewFilesAsync();
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error while watching for new files: {ex.Message}";
                AppendTextToLog(errorMessage);
            }
        }

        private async Task ProcessNewFilesAsync()
        {
            while (await createdFilesChannel.Reader.WaitToReadAsync())
            {
                while (createdFilesChannel.Reader.TryRead(out var filePath))
                {
                    await FileProcessingAsync(filePath);
                }
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                string message = $"New file: {e.FullPath}{Environment.NewLine}";
                AppendTextToLog(message);
                createdFilesChannel.Writer.TryWrite(e.FullPath);
            }
        }

        private async Task FileProcessingAsync(string filePath)
        {
            {
                try
                {
                    await c.EncryptFileAsync(filePath);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Error while processing file: {filePath}{Environment.NewLine}{ex.Message}";
                    AppendTextToLog(errorMessage);
                }
            }

        }

        private void AppendTextToLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke(new MethodInvoker(delegate
                {
                    txtLog.AppendText(message);
                }));
            }
            else
            {
                txtLog.AppendText(message);
            }
        }

    }
}
