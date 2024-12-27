using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tournament_421_TyryshkinAD.Domain.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _executor;
        private readonly Action<object> _paramExecutor;

        public RelayCommand(Action action)
        {
            _executor = action;
        }

        public RelayCommand(Action<object> action)
        {
            _paramExecutor = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _executor?.Invoke();
            _paramExecutor?.Invoke(parameter);
        }
    }
}
