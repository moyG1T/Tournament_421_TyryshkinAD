using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tournament_421_TyryshkinAD.Data;
using Tournament_421_TyryshkinAD.Domain.Commands;
using Tournament_421_TyryshkinAD.Domain.Contexts;
using Tournament_421_TyryshkinAD.Domain.IServices;
using Tournament_421_TyryshkinAD.Domain.Utilities;
using Tournament_421_TyryshkinAD.Properties;

namespace Tournament_421_TyryshkinAD.ViewModels
{
    public class PlayerViewModel : ViewModel
    {
        private readonly DbEntities _entities;

        public Player Player { get; set; } = new Player();

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsLoginBlocked)); }
        }
        public bool IsLoginBlocked => !IsLoading;

        private bool _isLogged = false;
        public bool IsLogged
        {
            get { return _isLogged; }
            set { _isLogged = value; OnPropertyChanged(); OnPropertyChanged(nameof(LoginHidden)); }
        }
        public bool LoginHidden => !IsLogged;

        public ICommand LoginCommand { get; }
        public ICommand CreateTeamCommand { get; }
        public ICommand SelectTourCommand { get; }
        public ICommand ExitCommand { get; }

        public PlayerViewModel(INavService createTeam, INavService selectTour, DbEntities entities)
        {
            CreateTeamCommand = new NavCommand(createTeam);
            SelectTourCommand = new NavCommand(selectTour);
            ExitCommand = new GoBackCommand(createTeam);

            LoginCommand = new RelayAsyncCommand(LoginAsync);

            _entities = entities;
        }

        public async Task LoginAsync(object param)
        {
            try
            {
                IsLoading = true;

                if (param is PasswordBox passwordBox)
                {
                    var password = passwordBox.Password;

                    if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(Player.Login))
                    {
                        MessageBox.Show("Пустые поля");
                        return;
                    }

                    var user = await _entities.Player
                        .FirstOrDefaultAsync(it => it.Login == Player.Login && it.Password == password);

                    if (user is null)
                    {
                        MessageBox.Show("Неверные данные");
                        passwordBox.Password = string.Empty;
                    }

                    Player = user;
                    OnPropertyChanged(nameof(Player));

                    Settings.Default.UserId = user.Id;
                    Settings.Default.ModId = 1;
                    Settings.Default.Save();

                    IsLogged = true;
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
