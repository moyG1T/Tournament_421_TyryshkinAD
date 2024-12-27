using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tournament_421_TyryshkinAD.Data;
using Tournament_421_TyryshkinAD.Domain.Commands;
using Tournament_421_TyryshkinAD.Domain.IServices;
using Tournament_421_TyryshkinAD.Domain.Utilities;
using Tournament_421_TyryshkinAD.Properties;

namespace Tournament_421_TyryshkinAD.ViewModels
{
    public class CreateTeamViewModel : ViewModel
    {
        private readonly DbEntities _entities;

        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        public ObservableCollection<Player> SelectedPlayers { get; set; } = new ObservableCollection<Player>();

        private string title;
        public string Title
        {
            get => title; set
            {
                title = value; OnPropertyChanged();
            }
        }

        private Player _selectedPlayer;
        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set { _selectedPlayer = value; OnPropertyChanged(); }
        }


        public ICommand AddPlayerCommand { get; }
        public ICommand RemovePlayerCommand { get; }
        public ICommand CreateTeamCommand { get; }
        public ICommand GoBackCommand { get; }

        public CreateTeamViewModel(INavService back, DbEntities entities)
        {
            _entities = entities;

            AddPlayerCommand = new RelayCommand(AddPlayer);
            RemovePlayerCommand = new RelayCommand(RemovePlayer);
            CreateTeamCommand = new RelayAsyncCommand(CreateTeam);
            GoBackCommand = new GoBackCommand(back);

            var players = _entities.Player.Where(it => it.Id != Settings.Default.UserId).ToList();
            Players = new ObservableCollection<Player>(players);
        }

        private void AddPlayer()
        {
            if (SelectedPlayer is null)
            {
                return;
            }

            SelectedPlayers.Add(SelectedPlayer);
            Players.Remove(SelectedPlayer);
        }
        private void RemovePlayer(object param)
        {
            if (param is Player player)
            {
                Players.Add(player);
                SelectedPlayers.Remove(player);
            }
        }
        private async Task CreateTeam()
        {
            if (string.IsNullOrEmpty(Title) || SelectedPlayers.Count == 0)
            {
                MessageBox.Show("Пустые поля");
                return;
            }

            try
            {
                var team = new Team
                {
                    Title = Title,
                };
                _entities.Team.Add(team);
                await _entities.SaveChangesAsync();

                var teamPlayers = new List<TeamContent>();

                var me = new TeamContent
                {
                    PlayerId = Settings.Default.UserId,
                    TeamId = team.Id,
                    RoleId = 1,
                    Timestamp = DateTime.Now,
                };
                teamPlayers.Add(me);

                foreach (var player in SelectedPlayers)
                {
                    var teamPlayer = new TeamContent
                    {
                        PlayerId = player.Id,
                        TeamId = team.Id,
                        RoleId = 1,
                        Timestamp = DateTime.Now,
                    };

                    teamPlayers.Add(teamPlayer);
                }

                _entities.TeamContent.AddRange(teamPlayers);
                await _entities.SaveChangesAsync();

                GoBackCommand.Execute(null);

                MessageBox.Show("Команда создана");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
