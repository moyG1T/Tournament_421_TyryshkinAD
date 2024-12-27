using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament_421_TyryshkinAD.Domain.Utilities;

namespace Tournament_421_TyryshkinAD.Domain.Contexts
{
    public class MainContext
    {
        private readonly Stack<ViewModel> _history = new Stack<ViewModel>();

        public ViewModel CurrentViewModel => _history.Peek();

        public event Action ViewModelChanged;

        public void Pop()
        {
            if (_history.Count > 0)
            {
                _history.Pop();
            }

            ViewModelChanged?.Invoke();
        }

        public void Push(ViewModel viewModel)
        {
            _history.Push(viewModel);

            ViewModelChanged?.Invoke();
        }

        public void PopAndPush(ViewModel viewModel)
        {
            foreach (var item in _history)
            {
                item?.Dispose();
            }
            _history?.Clear();

            _history.Push(viewModel);

            ViewModelChanged?.Invoke();
        }
    }
}
