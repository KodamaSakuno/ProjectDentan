using System;
using System.Linq;
using System.Reactive.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class FleetsViewModel : ModelBase
    {
        public GameViewModel Owner { get; private set; }

        FleetViewModel[] r_Fleets;
        public FleetViewModel[] Fleets
        {
            get { return r_Fleets; }
            set
            {
                if (r_Fleets != value)
                {
                    r_Fleets = value;
                    OnPropertyChanged();
                }
            }
        }

        FleetViewModel r_SelectedFleet;
        public FleetViewModel SelectedFleet
        {
            get { return r_SelectedFleet; }
            set
            {
                if (r_SelectedFleet != value)
                {
                    r_SelectedFleet = value;
                    OnPropertyChanged();
                }
            }
        }

        public FleetsViewModel(GameViewModel rpOwner)
        {
            Owner = rpOwner;

            KanColleGame.Current.ObservablePropertyChanged.Where(r => r == "Fleets").Subscribe(_ =>
            {
                Fleets = KanColleGame.Current.Fleets.Values.Select(r => new FleetViewModel(Owner, r)).ToArray();
                SelectedFleet = Fleets.FirstOrDefault();
            });
        }
    }
}
