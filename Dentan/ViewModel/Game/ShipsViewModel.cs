using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class ShipsViewModel : ModelBase
    {
        public ManualResetEventSlim WaitShipsEvent { get; private set; }

        ShipViewModel[] r_Ships;
        public ShipViewModel[] Ships
        {
            get { return r_Ships; }
            set
            {
                if (r_Ships != value)
                {
                    r_Ships = value;
                    OnPropertyChanged();
                }
            }
        }

        int r_Count;
        public int Count
        {
            get { return r_Count; }
            set
            {
                if (r_Count != value)
                {
                    r_Count = value;
                    OnPropertyChanged();
                }
            }
        }

        public ShipsViewModel()
        {
            WaitShipsEvent = new ManualResetEventSlim(false);

            KanColleGame.Current.ObservablePropertyChanged.Where(r => r == nameof(KanColleGame.Current.Ships)).Subscribe(_ =>
            {
                Count = KanColleGame.Current.Ships.Count + KanColleGame.Current.DroppedShip;

                WaitShipsEvent.Reset();
                Ships = KanColleGame.Current.Ships.Values.Select(r => new ShipViewModel(r)).ToArray();
                WaitShipsEvent.Set();
            });
        }
    }
}
