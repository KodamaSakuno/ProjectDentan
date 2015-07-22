using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.Data
{
    public enum BuildingDockState { Locked = -1, Idle, Building = 2, Completed }

    public class BuildingDock : CountdownModelBase, IID
    {
        public int ID { get; private set; }

        BuildingDockState r_State;
        public BuildingDockState State
        {
            get { return r_State; }
            set
            {
                if (r_State != value)
                {
                    r_State = value;
                    OnPropertyChanged();
                }
            }
        }

        ShipInfo r_Ship;
        public ShipInfo Ship
        {
            get { return r_Ship; }
            set
            {
                if (r_Ship != value)
                {
                    r_Ship = value;
                    OnPropertyChanged();
                }
            }
        }

        int r_Fuel;
        public int Fuel
        {
            get { return r_Fuel; }
            set
            {
                if (r_Fuel != value)
                {
                    r_Fuel = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_Bullet;
        public int Bullet
        {
            get { return r_Bullet; }
            set
            {
                if (r_Bullet != value)
                {
                    r_Bullet = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_Steel;
        public int Steel
        {
            get { return r_Steel; }
            set
            {
                if (r_Steel != value)
                {
                    r_Steel = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_Bauxite;
        public int Bauxite
        {
            get { return r_Bauxite; }
            set
            {
                if (r_Bauxite != value)
                {
                    r_Bauxite = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_DevelopmentMaterial;
        public int DevelopmentMaterial
        {
            get { return r_DevelopmentMaterial; }
            set
            {
                if (r_DevelopmentMaterial != value)
                {
                    r_DevelopmentMaterial = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool? IsLargeShipConstruction
        {
            get
            {
                if (State == BuildingDockState.Idle || State == BuildingDockState.Locked)
                    return null;

                return Fuel >= 1000 && Bullet >= 1000 && Steel >= 1000 & Bauxite >= 1000;
            }
        }

        ManualResetEventSlim r_WaitToRecord;

        public event Action<BuildingDockCompletedEventArgs> BuildingCompleted = delegate { };

        public BuildingDock(int rpID)
        {
            ID = rpID;
        }

        public void Update(RawBuildingDock rpDock)
        {
            State = rpDock.State;
            Ship = State == BuildingDockState.Building || State == BuildingDockState.Completed ? KanColleGame.Current.Base.Ships[rpDock.ShipID] : null;
            CompleteTime = State == BuildingDockState.Building ? new DateTimeOffset?(DateTimeUtil.UnixEpoch.AddMilliseconds(rpDock.CompleteTime)) : null;

            Fuel = rpDock.Fuel;
            Bullet = rpDock.Bullet;
            Steel = rpDock.Steel;
            Bauxite = rpDock.Bauxite;
            DevelopmentMaterial = rpDock.DevelopmentMaterial;

            if (r_WaitToRecord != null)
            {
                r_WaitToRecord.Set();
                r_WaitToRecord.Dispose();
                r_WaitToRecord = null;
            }
        }

        internal void CompleteConstruction()
        {
            IsNotificated = true;
            State = BuildingDockState.Completed;
            CompleteTime = null;
        }

        protected override void TimeOut()
        {
            if (!IsNotificated)
            {
                BuildingCompleted(new BuildingDockCompletedEventArgs(Ship.Name));
                CompleteConstruction();
            }
        }

        internal void PostRecord()
        {
            r_WaitToRecord = new ManualResetEventSlim(false);

            Task.Run(() =>
            {
                r_WaitToRecord.Wait();

                RecordManager.Instance.Construction.Update(this);
            });
        }
    }
}
