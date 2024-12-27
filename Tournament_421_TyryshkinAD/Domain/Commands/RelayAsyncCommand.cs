using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tournament_421_TyryshkinAD.Domain.Commands
{
    public class RelayAsyncCommand : ICommand
    {
        private readonly Func<Task> _executor;
        private readonly Func<object, Task> _paramExecutor;

        public RelayAsyncCommand(Func<Task> func)
        {
            _executor = func;
        }

        public RelayAsyncCommand(Func<object, Task> func)
        {
            _paramExecutor = func;
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
