using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GithubDLCountNotification
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel(Window mainWindow)
        {
            MainWindow = mainWindow;
            StartCommand = new DelegateCommand(StartCommandExecute) { CanExecuteValue = true };
            StopCommand = new DelegateCommand(StopCommandExecute) { CanExecuteValue = false };
        }

        #region INotifyPropertyChangedの実装
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
          => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

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

        public ICommand StartCommand { get; set; }

        public ICommand StopCommand { get; set; }

        private void StartCommandExecute()
        {

        }

        private void StopCommandExecute()
        {

        }

    }
}
