using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.QuestData;
using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Proxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Moen.KanColle.Dentan
{
    public class KanColleGame : ModelBase
    {
        static KanColleGame r_Instance = new KanColleGame();
        public static KanColleGame Current { get { return r_Instance; } }

        public GameProxy Proxy { get; private set; }

        public BaseInfo Base { get; internal set; }

        public Headquarter Headquarter { get; private set; }

        Table<Fleet> r_Fleets;
        public Table<Fleet> Fleets
        {
            get { return r_Fleets; }
            internal set
            {
                if (r_Fleets != value)
                {
                    r_Fleets = value;
                    OnPropertyChanged();
                }
            }
        }

        public HashSet<int> ShipIDs { get; private set; }
        Table<Ship> r_Ships;
        public Table<Ship> Ships
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
        int r_DroppedShip;
        public int DroppedShip
        {
            get { return r_DroppedShip; }
            set
            {
                if (r_DroppedShip != value)
                {
                    r_DroppedShip = value;
                    OnPropertyChanged();
                }
            }
        }

        CompassData r_CompassData;
        public CompassData CompassData
        {
            get { return r_CompassData; }
            internal set
            {
                if (r_CompassData != value)
                {
                    r_CompassData = value;
                    OnPropertyChanged();
                }
            }
        }

        bool r_IsInBattle;
        public bool IsInBattle
        {
            get { return r_IsInBattle; }
            internal set
            {
                if (r_IsInBattle != value)
                {
                    r_IsInBattle = value;
                    OnPropertyChanged();
                }
            }
        }
        BattleData r_Battle;
        public BattleData Battle
        {
            get { return r_Battle; }
            internal set
            {
                if (r_Battle != value)
                {
                    r_Battle = value;
                    OnPropertyChanged();
                }
            }
        }

        public Table<Quest> QuestTable { get; private set; }
        Quest[] r_Quests;
        public Quest[] Quests
        {
            get { return r_Quests; }
            internal set
            {
                if (r_Quests != value)
                {
                    r_Quests = value;
                    OnPropertyChanged();
                }
            }
        }

        Table<RepairDock> r_RepairDocks;
        public Table<RepairDock> RepairDocks
        {
            get { return r_RepairDocks; }
            internal set
            {
                if (r_RepairDocks != value)
                {
                    r_RepairDocks = value;
                    OnPropertyChanged();
                }
            }
        }

        Table<BuildingDock> r_BuildingDocks;
        public Table<BuildingDock> BuildingDocks
        {
            get { return r_BuildingDocks; }
            internal set
            {
                if (r_BuildingDocks != value)
                {
                    r_BuildingDocks = value;
                    OnPropertyChanged();
                }
            }
        }

        Table<Equipment> r_Equipments;
        public Table<Equipment> Equipments
        {
            get { return r_Equipments; }
            internal set
            {
                if (r_Equipments != value)
                {
                    r_Equipments = value;
                    OnPropertyChanged();
                }
            }
        }

        public Fleet SortieFleet { get; internal set; }
        public CombinedFleetFlag CombinedFleet { get; internal set; }
        public Dictionary<int, int> EventRank { get; internal set; }

        public event Action GameLaunched = delegate { };
        public event Action TokenOutdated = delegate { };
        public event Action<BaseInfo> BaseDataLoaded = delegate { };
        public event Action ShipsUpdated = delegate { };
        public event Action EquipmentsUpdated = delegate { };

        public event Action<string> StatusBarMessage = delegate { };

        public IConnectableObservable<string> ObservablePropertyChanged { get; private set; }
        IDisposable r_PropertyChangedSubscriptions;

        KanColleGame()
        {
            Proxy = new GameProxy();
            Headquarter = new Headquarter();
            QuestTable = new Table<Quest>();

            Fleets = new Table<Fleet>();
            Ships = new Table<Ship>();
            Equipments = new Table<Equipment>();

            ObservablePropertyChanged = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                rpHandler => PropertyChanged += rpHandler,
                rpHandler => PropertyChanged -= rpHandler)
                .Select(r => r.EventArgs.PropertyName)
                .Publish();
            r_PropertyChangedSubscriptions = ObservablePropertyChanged.Connect();
        }

        internal void UpdateBaseInfo(RawApiStart rpData)
        {
            if (Base == null)
                Base = new BaseInfo(rpData);
            else
                Base.Update(rpData);

            Base.UpdateShipModelIDs();
            BaseDataLoaded(Base);
        }

        internal void AddShip(int rpID, int rpShipID, RawShip rpShip)
        {
            r_Ships.Add(new Ship(rpShip));

            ShipIDs.Add(rpShipID);
            CheckQuests();
            ShipsUpdated();
        }
        internal void AddEquipments(RawEquipment[] rpEquipments)
        {
            foreach (var rEquipment in rpEquipments)
                r_Equipments.Add(new Equipment(rEquipment));
        }
        internal void UpdateFleets(RawFleet[] rpFleets)
        {
            if (Fleets.UpdateRawData<RawFleet>(rpFleets, r => new Fleet(r), (rpData, rpRawData) => rpData.Update(rpRawData)))
                OnPropertyChanged(nameof(Fleets));
        }
        internal void UpdateShips()
        {
            ShipsUpdated();
            OnPropertyChanged(nameof(Ships));
        }
        internal void UpdateShips(RawShip[] rpShips)
        {
            if (Ships.UpdateRawData<RawShip>(rpShips, r => new Ship(r), (rpData, rpRawData) => rpData.Update(rpRawData)))
            {
                ShipIDs = new HashSet<int>(r_Ships.Values.Select(r => r.ShipID));
                CheckQuests();
                UpdateShips();
            }
        }

        internal void UpdateEquipments()
        {
            EquipmentsUpdated();
            OnPropertyChanged(nameof(Equipments));
        }
        internal void UpdateEquipments(RawEquipment[] rpEquipments)
        {
            if (Equipments.UpdateRawData<RawEquipment>(rpEquipments, r => new Equipment(r), (rpData, rpRawData) => rpData.Update(rpRawData)))
                UpdateEquipments();
        }
        internal void UpdateRepairDocks(RawRepairDock[] rpDocks)
        {
            if (RepairDocks == null)
                RepairDocks = new Table<RepairDock>(rpDocks.ToDictionary(r => r.ID, r => new RepairDock(r.ID)));

            foreach (var rDock in rpDocks)
                RepairDocks[rDock.ID].Update(rDock);
        }
        internal void UpdateBuildingDocks(RawBuildingDock[] rpDocks)
        {
            if (BuildingDocks == null)
                BuildingDocks = new Table<BuildingDock>(rpDocks.ToDictionary(r => r.ID, r => new BuildingDock(r.ID)));

            foreach (var rDock in rpDocks)
                BuildingDocks[rDock.ID].Update(rDock);
        }

        internal void CheckQuests()
        {
            if (Quests != null)
                foreach (var rQuest in Quests)
                    ShipRequirement.Check(rQuest);
        }

        internal void RaiseTokenOutdatedEvent()
        {
            TokenOutdated();
        }
        internal void RaiseGameLaunchedEvent()
        {
            GameLaunched();
        }

        public void SendMessageToStatusBar(string rpMessage)
        {
            StatusBarMessage(rpMessage);
        }
    }
}
