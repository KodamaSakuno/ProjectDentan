using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Moen.KanColle.Dentan.Data
{
    public abstract class CountdownModelBase : ModelBase, IDisposable
    {
        DateTimeOffset? r_CompleteTime;
        public DateTimeOffset? CompleteTime
        {
            get { return r_CompleteTime; }
            set
            {
                if (r_CompleteTime != value)
                {
                    r_CompleteTime = value;
                    IsNotificated = false;
                    OnPropertyChanged();
                }
            }
        }
        TimeSpan? r_RemainingTime;
        public TimeSpan? RemainingTime
        {
            get { return r_RemainingTime; }
            set
            {
                if (r_RemainingTime != value)
                {
                    r_RemainingTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeSpan TimeToNotificate { get; set; }
        public bool IsNotificated { get; protected set; }

        static IConnectableObservable<long> r_Interval;
        static IDisposable r_IntervalSubscription;

        static CountdownModelBase()
        {
            r_Interval = Observable.Interval(TimeSpan.FromSeconds(1.0)).Publish();
            r_Interval.Connect();
        }
        public CountdownModelBase()
        {
            r_IntervalSubscription = r_Interval.Subscribe(_ => OnTick());
        }

        public void Dispose()
        {
            if (r_IntervalSubscription != null)
            {
                r_IntervalSubscription.Dispose();
                r_IntervalSubscription = null;
            }
        }

        void OnTick()
        {
            if (CompleteTime.HasValue)
            {
                var rTimeSpan = CompleteTime.Value - DateTimeOffset.Now;
                if (rTimeSpan.Ticks < 0L)
                    rTimeSpan = TimeSpan.Zero;
                RemainingTime = CompleteTime.HasValue ? new TimeSpan?(rTimeSpan) : null;

                if (rTimeSpan <= TimeToNotificate && !IsNotificated)
                    TimeOut();
            }
        }

        protected abstract void TimeOut();
    }
}
