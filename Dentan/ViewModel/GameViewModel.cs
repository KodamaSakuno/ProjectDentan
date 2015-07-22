using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.ViewModel.Game;
using System.Linq;

namespace Moen.KanColle.Dentan.ViewModel
{
    public class GameViewModel : ModelBase
    {
        public static GameViewModel Instance { get; private set; }

        public AdmiralViewModel Admiral { get; private set; }
        public ShipsViewModel Ships { get; private set; }
        public FleetsViewModel Fleets { get; private set; }

        RepairDockViewModel[] r_RepairDocks;
        public RepairDockViewModel[] RepairDocks
        {
            get { return r_RepairDocks; }
            private set
            {
                if (r_RepairDocks != value)
                {
                    r_RepairDocks = value;
                    OnPropertyChanged();
                }
            }
        }

        BuildingDockViewModel[] r_BuildingDocks;
        public BuildingDockViewModel[] BuildingDocks
        {
            get { return r_BuildingDocks; }
            private set
            {
                if (r_BuildingDocks != value)
                {
                    r_BuildingDocks = value;
                    OnPropertyChanged();
                }
            }
        }

        public QuestsViewModel Quests { get; private set; }

        public BattleData Battle { get; private set; }
        public EnemyFleet EnemyFleet { get; private set; }
        public EquipmentsViewModel Equipments { get; private set; }
        public SessionsViewModel Sessions { get; private set; }


        CompassDataViewModel r_CompassData;
        public CompassDataViewModel CompassData
        {
            get { return r_CompassData; }
            private set
            {
                if (r_CompassData != value)
                {
                    r_CompassData = value;
                    OnPropertyChanged();
                }
            }
        }

        public GameViewModel()
        {
            Admiral = new AdmiralViewModel();
            Ships = new ShipsViewModel();
            Fleets = new FleetsViewModel(this);
            Equipments = new EquipmentsViewModel();
            Sessions = new SessionsViewModel();
            Quests = new QuestsViewModel();
            CompassData = new CompassDataViewModel();

            KanColleGame.Current.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "RepairDocks")
                    RepairDocks = KanColleGame.Current.RepairDocks.Select(r => new RepairDockViewModel(r.Value)).ToArray();

                if (e.PropertyName == "BuildingDocks")
                    BuildingDocks = KanColleGame.Current.BuildingDocks.Select(r => new BuildingDockViewModel(r.Value)).ToArray();

                if (e.PropertyName == "Quests")
                    Quests.Update();
                
                if (e.PropertyName == "Battle")
                {
                    Battle = KanColleGame.Current.Battle;
                    OnPropertyChanged(() => Battle);
                }
            };
        }
    }
}
