using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Updater
{
    internal class Updater
    {
        private readonly Version _currentVersion;
        private readonly string _fileLink;
        private readonly WebClient _webClient;
        private bool _isDownloaded;
        public bool Update { get; private set; }

        public int CurrentUpdProgress { get; private set; }

        public Updater(Version currentVersion, string fileLink)
        {
            this._currentVersion = currentVersion;
            this._fileLink = fileLink;
            this._webClient = new WebClient { Encoding = Encoding.UTF8 };
            this._webClient.DownloadProgressChanged += this._webClient_DownloadProgressChanged;
            this._webClient.DownloadFileCompleted += this._webClient_DownloadFileCompleted;
        }

        /// <summary>
        /// Fires when a file has downloaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this._isDownloaded = true;
        }

        /// <summary>
        /// Срабатывает во время изменения прогресса скачивания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.CurrentUpdProgress = e.ProgressPercentage;
        }

        /// <summary>
        /// Asynchronous string download.
        /// </summary>
        /// <param name="url">Link</param>
        /// <returns></returns>
        private async Task<string> DownloadStringAsync(string url)
        {
            return await Task.Run(() =>
            {
                string result;
                try
                {
                    result = this._webClient.DownloadString(url);
                }
                catch
                {
                    result = "";
                }
                return result;
            });
        }

        /// <summary>
        /// Gets a link to download the latest version.
        /// </summary>
        /// <returns>Link to the latest version or an empty string if the update is not required</returns>
        private async Task<string> GetReleaseLink()
        {
            string fileContent = await this.DownloadStringAsync(this._fileLink);
            if (string.IsNullOrEmpty(fileContent))
            {
                return string.Empty;
            }
            string[] dataArr = fileContent.Split('|');
            var fileVersion = new Version(dataArr[0]);
            string releaseLink = dataArr[1];
            if (this._currentVersion >= fileVersion)
            {
                return "";
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show("New update found. Would you like to update the program now?",
                        "Update detected", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        var fr = (Dialog_Builder_2.frСheckingForUpdates)Dialog_Builder_2.frСheckingForUpdates.ActiveForm;
                        fr.Message.Text = "Download update...";
                    }
                    catch { }
                    Update = true;
                    return releaseLink;
                    
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Checks for updates.
        /// </summary>
        /// <returns></returns>
        public async Task CheckForUpdates()
        {
            string releaseLink = await this.GetReleaseLink();
            if (string.IsNullOrEmpty(releaseLink))
            {
                return;
            }
            string fileName = Path.GetTempPath() + "ItemCreatorSetup.msi";
            this._isDownloaded = false;
            try
            {
                this._webClient.DownloadFileAsync(new Uri(releaseLink), fileName);
            }
            catch
            {
                return;
            }
            Process[] waControllers = Process.GetProcessesByName("ItemCreator.exe");
            Array.ForEach(waControllers, p => p.CloseMainWindow());
            while (!this._isDownloaded)
            {
                await Task.Delay(100);
            }
            Process.Start(fileName, "/qr");
            
        }
    }
}