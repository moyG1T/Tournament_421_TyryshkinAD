using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament_421_TyryshkinAD.Domain.Contexts;
using Tournament_421_TyryshkinAD.Domain.IServices;
using Tournament_421_TyryshkinAD.Domain.Utilities;

namespace Tournament_421_TyryshkinAD.Domain.Services
{
    public class MainNavService :INavService
    {
        private readonly MainContext _context;
        private readonly Func<ViewModel> _func;

        public MainNavService(MainContext context, Func<ViewModel> func)
        {
            _context = context;
            _func = func;
        }

        public void GoBack()
        {
            _context.Pop();
        }

        public void Navigate()
        {
            if (_func != null)
            {
                _context.Push(_func());
            }
        }

        public void NavigateAndDispose()
        {
            if (_func != null)
            {
                _context.PopAndPush(_func());
            }
        }
    }
}
