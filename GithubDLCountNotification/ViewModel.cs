using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GithubDLCountNotification
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel(Window mainWindow)
        {
            MainWindow = mainWindow;
            httpClient = new HttpClient();
            StartCommand = new DelegateCommand(StartCommandExecute) { CanExecuteValue = true };
            StopCommand = new DelegateCommand(StopCommandExecute) { CanExecuteValue = false };

            dispatcherTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 90),
            };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        #region INotifyPropertyChangedの実装
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
          => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        DispatcherTimer dispatcherTimer;

        int lastDownloadCount = 0;

        /// <summary>
        /// 唯一のhttpクライアント
        /// </summary>
        HttpClient httpClient;

        internal Window MainWindow { get; set; }

        private string _userName = "shigobu";

        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName == value) { return; }
                _userName = value;
                RaisePropertyChanged();
            }
        }

        private string _repositoryName = "SAPIForVOICEVOX";

        public string RepositoryName
        {
            get => _repositoryName;
            set
            {
                if (_repositoryName == value) { return; }
                _repositoryName = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }

        private async void StartCommandExecute()
        {
            dispatcherTimer.Start();
            lastDownloadCount = await GetDownloadCount(UserName, RepositoryName);
            StartCommand.CanExecuteValue = false;
            StopCommand.CanExecuteValue = true;

        }

        private void StopCommandExecute()
        {
            dispatcherTimer.Stop();
            StartCommand.CanExecuteValue = true;
            StopCommand.CanExecuteValue = false;
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            int currentDownloadCount = await GetDownloadCount(UserName, RepositoryName);
            if (currentDownloadCount > lastDownloadCount)
            {
                new ToastContentBuilder()
                 .AddText($"{RepositoryName}がダウンロードされました。")
                 .AddText($"{currentDownloadCount - lastDownloadCount}回")
                 .Show();
            }
            lastDownloadCount = currentDownloadCount;
        }

        private async Task<int> GetDownloadCount(string userName, string repositoryName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userName}/{repositoryName}/releases");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "shigobu");
            using (var result = await httpClient.SendAsync(request))
            {
                string resBodyStr = await result.Content.ReadAsStringAsync();
                JArray jsonArr = JArray.Parse(resBodyStr);
                int count = 0;
                foreach (JToken release in jsonArr)
                {
                    foreach (JToken asset in release["assets"])
                    {
                        count += asset.Value<int>("download_count");
                    }
                }
                return count;
            }
        }

    }
}
