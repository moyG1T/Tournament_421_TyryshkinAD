using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament_421_TyryshkinAD.Domain.Utilities;

namespace Tournament_421_TyryshkinAD.Data
{
    public partial class Player : ObservableObject
    {
		private string _login;

		public string Login
		{
			get { return _login; }
			set { _login = value; OnPropertyChanged(); }
		}

	}
}
