using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GithubDLCountNotification
{
    /// <summary>
    /// プリズムのコードを参考に、デリゲートコマンドを作成。
    /// </summary>
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action executeMethod)
        {
            ExecuteMethod = executeMethod;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteValue;
        }

        public void Execute(object parameter)
        {
            ExecuteMethod();
        }

        private Action ExecuteMethod { get; set; }

        private bool _CanExecuteValue = true;
        public bool CanExecuteValue
        {
            get => _CanExecuteValue;
            set
            {
                if (_CanExecuteValue == value) { return; }
                _CanExecuteValue = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
