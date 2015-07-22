using Moen.KanColle.Dentan.Data.Raw;
using System;

namespace Moen.KanColle.Dentan.Data
{
    public enum RepairDockState { Locked = -1, Idle, Repairing }

    public class RepairDock : CountdownModelBase, IID
    {
        public int ID { get; private set; }
        
        RepairDockState r_State;
        public RepairDockState State
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

        Ship r_Ship;
        public Ship Ship
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

        public event Action<RepairDockCompletedEventArgs> RepairCompleted = delegate { };

        public RepairDock(int rpID)
        {
            ID = rpID;
        }

        public void Update(RawRepairDock rpDock)
        {
            State = rpDock.State;
            Ship = State == RepairDockState.Repairing ? KanColleGame.Current.Ships[rpDock.ShipID] : null;
            CompleteTime = State == RepairDockState.Repairing ? new DateTimeOffset?(new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(rpDock.CompleteTime)) : null;
        }

        protected override void TimeOut()
        {
            if (!IsNotificated)
            {
                RepairCompleted(new RepairDockCompletedEventArgs(Ship.Name));
                IsNotificated = true;
            }
        }
    }
}
