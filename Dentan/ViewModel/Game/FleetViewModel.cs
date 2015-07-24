using Moen.KanColle.Dentan.Data;
using System.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class FleetViewModel : ViewModel<Fleet>
    {
        GameViewModel r_Game;

        public int ID { get { return Model.ID; } }
        public string Name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Model.Name))
                    return Model.Name;
                return "(第 " + ID + " 艦隊)";
            }
        }

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

        public ExpeditionStatusViewModel Expedition { get; private set; }

        public FleetViewModel(GameViewModel rpGame, Fleet rpFleet)
            : base(rpFleet)
        {
            r_Game = rpGame;

            Expedition = new ExpeditionStatusViewModel(rpFleet.Expedition);

            rpFleet.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Ships")
                    UpdateShips();
                if (e.PropertyName == "Name")
                    OnPropertyChanged(() => Name);
            };

            UpdateShips();
        }

        void UpdateShips()
        {
            r_Game.Ships.WaitShipsEvent.Wait();

            Ships = Model.Ships.Select(r => r_Game.Ships.Ships.Single(rpShip => rpShip.Model == r)).ToArray();
        }
    }
}
