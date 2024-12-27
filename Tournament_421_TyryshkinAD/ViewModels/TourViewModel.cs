using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tournament_421_TyryshkinAD.Data;
using Tournament_421_TyryshkinAD.Domain.Commands;
using Tournament_421_TyryshkinAD.Domain.Contexts;
using Tournament_421_TyryshkinAD.Domain.IServices;
using Tournament_421_TyryshkinAD.Domain.Utilities;
using Tournament_421_TyryshkinAD.Properties;

namespace Tournament_421_TyryshkinAD.ViewModels
{
    public class TourViewModel : ViewModel
    {
        private readonly DbEntities _entities;
        private Team selectedTeam;

        public ICommand GoBackCommand { get; }
        public ICommand RegistCommand { get; }

        public bool IsMultipleGame {get;set;}

        public Tournament Tournament { get; set; } = new Tournament();
        public Team SelectedTeam
        {
            get => selectedTeam; set
            {
                selectedTeam = value; OnPropertyChanged();
            }
        }
        public List<Team> Teams { get; set; }

        public TourViewModel(INavService back, TourContext tourContext, DbEntities entities)
        {
            GoBackCommand = new GoBackCommand(back);
            RegistCommand = new RelayCommand(Regist);

            Tournament = tourContext.Tournament;
            _entities = entities;

            var userId = Settings.Default.UserId;
            Teams = _entities.Team
                .Where(it => it.TeamContent.Select(tc => tc.PlayerId).Contains(userId) && it.Id != 0)
                .ToList();

            IsMultipleGame = Tournament.FormatId != 1;
        }

        public void Regist()
        {
            var userId = Settings.Default.UserId;
            if (Tournament.FormatId == 1)
            {
                var tourTeam = new TourTeam
                {
                    TeamId = 0,
                    PlayerId = userId,
                    Timestamp = DateTime.Now,
                    TourId = Tournament.Id,
                };
                _entities.TourTeam.Add(tourTeam);
                _entities.SaveChanges();

                GoBackCommand?.Execute(null);
                MessageBox.Show("Успешно");
            }
            else
            {
                if (SelectedTeam is null)
                {
                    MessageBox.Show("Выберите команду");
                    return;
                }

                var list = new List<TourTeam>();
                foreach (var item in SelectedTeam.MatchPlayers)
                {
                    var tourTeam = new TourTeam
                    {
                        TeamId = SelectedTeam.Id,
                        PlayerId = item.PlayerId,
                        Timestamp = DateTime.Now,
                        TourId = Tournament.Id,
                    };
                    list.Add(tourTeam);
                }
                _entities.TourTeam.AddRange(list);
                _entities.SaveChanges();

                GoBackCommand?.Execute(null);
                MessageBox.Show("Успешно");
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
