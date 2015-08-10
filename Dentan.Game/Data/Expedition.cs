using System;

namespace Moen.KanColle.Dentan.Data
{
    public class Expedition : CountdownModelBase
    {
        public Fleet Fleet { get; private set; }

        int r_ExpeditonID;
        public int ExpeditionID
        {
            get { return r_ExpeditonID; }
            set
            {
                if (r_ExpeditonID != value)
                {
                    r_ExpeditonID = value;
                    OnPropertyChanged();
                }
            }
        }

        ExpeditionInfo r_Info;
        public ExpeditionInfo Info
        {
            get { return r_Info; }
            private set
            {
                if (r_Info != value)
                {
                    r_Info = value;
                    OnPropertyChanged();
                }
            }
        }

        public event Action<ExpeditionReturnedEventArgs> ExpeditionReturned = delegate { };

        public Expedition(Fleet rpFleet)
        {
            Fleet = rpFleet;
            ExpeditionID = -1;
            TimeToNotificate = TimeSpan.FromMinutes(1.0);
        }

        public void Update(int rpState, int rpExpeditonID, long rpCompleteTime)
        {
            if (rpState != 0)
            {
                ExpeditionID = rpExpeditonID;
                Info = KanColleGame.Current.Base.Expeditions[rpExpeditonID];
                CompleteTime = new DateTimeOffset?(new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(rpCompleteTime));
            }
            else
            {
                ExpeditionID = -1;
                Info = null;
                CompleteTime = null;
            }
        }

        protected override void TimeOut()
        {
            if (!IsNotificated && Info != null)
            {
                ExpeditionReturned(new ExpeditionReturnedEventArgs(Fleet.Name, Info.Name));
                IsNotificated = true;
            }
        }
    }
}
