using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tournament_421_TyryshkinAD.Domain.IServices;

namespace Tournament_421_TyryshkinAD.Domain.Commands
{
    public class GoBackCommand : ICommand
    {
        private readonly INavService _navService;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public GoBackCommand(INavService navService)
        {
            _navService = navService;
        }

        public void Execute(object parameter)
        {
            _navService.GoBack();
        }
    }
}
