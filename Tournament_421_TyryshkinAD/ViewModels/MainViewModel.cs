using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tournament_421_TyryshkinAD.Domain.Commands;
using Tournament_421_TyryshkinAD.Domain.Contexts;
using Tournament_421_TyryshkinAD.Domain.IServices;
using Tournament_421_TyryshkinAD.Domain.Utilities;

namespace Tournament_421_TyryshkinAD.ViewModels
{
    public class MainViewModel : ViewModel
    {//
        private readonly MainContext _mainContext;

        public ICommand GoViewerCommand { get; }
        public ICommand GoPlayerCommand { get; }
        public ICommand GoModCommand { get; }

        public bool IsNavDisabled => _mainContext.CurrentViewModel is null;
        public ViewModel CurrentViewModel => _mainContext.CurrentViewModel;

        public MainViewModel(INavService viewer, INavService player, INavService mod, MainContext mainContext)
        {
            _mainContext = mainContext;

            GoViewerCommand = new NavCommand(viewer);
            GoPlayerCommand = new NavCommand(player);
            GoModCommand = new NavCommand(mod);

            _mainContext.ViewModelChanged += OnViewModelChanged;
        }

        private void OnViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            OnPropertyChanged(nameof(IsNavDisabled));
        }

        public override void Dispose()
        {
            _mainContext.ViewModelChanged -= OnViewModelChanged;

            GC.SuppressFinalize(this);
        }
    }
}
