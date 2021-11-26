using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        internal Window MainWindow { get; set; }

        private string _userName = "";

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

        private string _repositoryName = "";

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

        private void StartCommandExecute()
        {
            dispatcherTimer.Start();
            StartCommand.CanExecuteValue = false;
            StopCommand.CanExecuteValue = true;
        }

        private void StopCommandExecute()
        {
            dispatcherTimer.Stop();
            StartCommand.CanExecuteValue = true;
            StopCommand.CanExecuteValue = false;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            
        }

    }
}
