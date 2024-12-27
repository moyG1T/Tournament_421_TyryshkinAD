using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tournament_421_TyryshkinAD.Data;
using Tournament_421_TyryshkinAD.Domain.Commands;
using Tournament_421_TyryshkinAD.Domain.Contexts;
using Tournament_421_TyryshkinAD.Domain.IServices;
using Tournament_421_TyryshkinAD.Domain.Utilities;
using Tournament_421_TyryshkinAD.Properties;

namespace Tournament_421_TyryshkinAD.ViewModels
{
    public class SelectTourViewModel : ViewModel
    {
        private readonly INavService _tour;
        private readonly TourContext _tourContext;
        private readonly DbEntities _entities;

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Tournaments));
            }
        }

        private int _userId;

        private Format _selectedFormat;
        public Format SelectedFormat
        {
            get => _selectedFormat; set
            {
                _selectedFormat = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Tournaments));
            }
        }

        public List<Format> Formats { get; set; }

        private List<Tournament> _originTours = new List<Tournament>();
        public List<Tournament> Tournaments
        {
            get
            {
                if (string.IsNullOrEmpty(SearchText) && SelectedFormat is null)
                {
                    var list = _originTours;

                    list.ForEach(it => it.IsMember = it.TourTeam.Select(tt => tt.PlayerId).Contains(_userId));

                    return list;
                }
                else if (string.IsNullOrEmpty(SearchText) && SelectedFormat != null)
                {
                    var list = _originTours.Where(it => it.FormatId == SelectedFormat.Id).ToList();

                    list.ForEach(it => it.IsMember = it.TourTeam.Select(tt => tt.PlayerId).Contains(_userId));

                    return list;
                }
                else if (!string.IsNullOrEmpty(SearchText) && SelectedFormat is null)
                {
                    var list = _originTours
                        .Where(it => it.Title.ToLower().Contains(SearchText.ToLower())
                            || it.PrizePool.ToString().ToLower().Contains(SearchText.ToLower())
                            || it.Game.Title.ToLower().Contains(SearchText.ToLower())
                            || it.Format.Title.ToLower().Contains(SearchText.ToLower())
                            )
                        .ToList();

                    list.ForEach(it => it.IsMember = it.TourTeam.Select(tt => tt.PlayerId).Contains(_userId));

                    return list;
                }
                else
                {
                    var list = _originTours
                        .Where(it => it.Title.ToLower().Contains(SearchText.ToLower())
                            || it.PrizePool.ToString().ToLower().Contains(SearchText.ToLower())
                            || it.Game.Title.ToLower().Contains(SearchText.ToLower())
                            || it.Format.Title.ToLower().Contains(SearchText.ToLower())

                            &&

                            it.FormatId == SelectedFormat.Id
                            )
                        .ToList();

                    list.ForEach(it => it.IsMember = it.TourTeam.Select(tt => tt.PlayerId).Contains(_userId));

                    return list;
                }
            }
        }

        public ICommand RemoveFormatFilterCommand { get; }
        public ICommand SelectTourCommand { get; }
        public ICommand GoBackCommand { get; }

        public SelectTourViewModel(INavService tour, TourContext tourContext,  DbEntities entities)
        {
            _userId = Settings.Default.UserId;
            _tour = tour;
            _tourContext = tourContext;
            _entities = entities;

            Formats = _entities.Format.ToList();

            var list = _entities.Tournament.ToList();
            list.ForEach(it => it.IsMember = it.TourTeam.Select(tt => tt.PlayerId).Contains(_userId));
            _originTours = list;

            RemoveFormatFilterCommand = new RelayCommand(RemoveFormatFilter);
            SelectTourCommand = new RelayCommand(SelectTour);
            GoBackCommand = new GoBackCommand(tour);
        }

        private void RemoveFormatFilter()
        {
            SelectedFormat = null;
        }

        private void SelectTour(object param)
        {
            if (param is Tournament tournament)
            {
                _tourContext.Tournament = tournament;
                _tour.Navigate();
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
