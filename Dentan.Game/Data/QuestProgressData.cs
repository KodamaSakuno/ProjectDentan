using Moen.KanColle.Dentan.Record;
using System;

namespace Moen.KanColle.Dentan.Data
{
    public class QuestProgressData : ModelBase, IID
    {
        static readonly TimeSpan TimeZoneOffset = TimeSpan.FromHours(9.0);
        static readonly TimeSpan ResetHour = TimeSpan.FromHours(5.0);

        public static bool DebugDataMode { get; set; }

        public int ID { get; internal set; }
        public QuestType Type { get; internal set; }
        internal int StartFrom { get; set; }

        QuestState r_State;
        public QuestState State
        {
            get { return r_State; }
            set
            {
                if (r_State != value)
                {
                    r_State = value;
                    RecordManager.Instance.Quest.UpdateStatus(this);

                    DebugUtil.Log(string.Format("任务 {0}: State={1}", ID, value.ToString()));
                }
            }
        }
        internal QuestState StateInternal
        {
            get { return r_State; }
            set { r_State = value; }
        }

        public int Total { get; internal set; }
        int r_Current;
        public virtual int Current
        {
            get { return r_Current; }
            internal set
            {
                if (State != QuestState.Progress) return;

                var rValue = Math.Min(Total, Math.Max(StartFrom, value));
                if (r_Current != rValue)
                {
                    r_Current = rValue;

                    Update();
                    OnPropertyChanged();

                    RecordManager.Instance.Quest.UpdateProgress(this);
                    DebugUtil.Log(string.Format("任务 {0}: Progress={1}", ID, rValue));
                }
            }
        }
        internal virtual int CurrentInternal
        {
            get { return r_Current; }
            set
            {
                r_Current = Math.Max(StartFrom, value);
                Update();
                OnPropertyChanged(() => Current);
            }
        }

        double r_Percent;
        public double Percent
        {
            get { return r_Percent; }
            protected set
            {
                if (r_Percent != value)
                {
                    r_Percent = value;
                    if (r_Percent > 100.0)
                        r_Percent = 100.0;

                    OnPropertyChanged();
                }
            }
        }

        DateTimeOffset? r_UpdateTime;
        public DateTimeOffset? UpdateTime
        {
            get
            {
                if (r_UpdateTime.HasValue)
                    return r_UpdateTime.Value.ToOffset(TimeZoneOffset);
                return null;
            }
            set
            {
                if (value.HasValue)
                    r_UpdateTime = value.Value.ToOffset(TimeZoneOffset);
                else
                    r_UpdateTime = null;
            }
        }

        internal virtual void Update()
        {
            Percent = Current / (double)Total * 100;
            UpdateTime = DateTimeOffset.Now;
        }
        internal void ResetByDate()
        {
            var rResetState = false;
            var rNow = DateTimeOffset.Now.ToOffset(TimeZoneOffset);

            if (!UpdateTime.HasValue)
                Current = 0;
            else
            {
                var rUpdateTime = UpdateTime.Value;
                DateTimeOffset rResetTime;
                switch (Type)
                {
                    case QuestType.Daily:
                    case QuestType.Special1:
                    case QuestType.Special2:
                        rResetTime = new DateTimeOffset(rNow.Year, rNow.Month, rNow.Day, 5, 0, 0, TimeZoneOffset);
                        if (rNow.Hour < 5)
                            rResetTime -= TimeSpan.FromDays(1.0);

                        rResetState = rUpdateTime < rResetTime;

                        break;

                    case QuestType.Weekly: 
                        var rOffset = rNow.DayOfWeek - DayOfWeek.Monday;
                        if (rOffset < 0)
                            rOffset += 7;

                        rResetTime = rNow.AddDays(-1 * rOffset) - rNow.TimeOfDay + ResetHour;

                        rResetState = rUpdateTime < rResetTime;

                        break;

                    case QuestType.Monthly:
                        rResetTime = new DateTimeOffset(rNow.Year, rNow.Month, 1, 5, 0, 0, TimeZoneOffset);
                        if (rNow.Hour < 5)
                            rResetTime = rResetTime.AddMonths(-1);

                        rResetState = rUpdateTime < rResetTime;
                        break;
                }
            }

            if (rResetState)
            {
                DebugUtil.Log(string.Format("任务 {0}: 被重置 原时间={1}", ID, UpdateTime.Value));
                StateInternal = QuestState.None;
                CurrentInternal = 0;
                UpdateTime = rNow;
                RecordManager.Instance.Quest.UpdateProgress(this);
                RecordManager.Instance.Quest.UpdateStatus(this);
            }
        }
        internal void ResetByProgress(QuestState rpState, QuestProgress rpProgress)
        {
            if (ID == 214)
                return;

            var rLimit = (int)Math.Ceiling(Total * 0.5);
            if (rpProgress == QuestProgress.Progress50 && Current < rLimit)
                CurrentInternal = rLimit;

            rLimit = (int)Math.Ceiling(Total * 0.8);
            if (rpProgress == QuestProgress.Progress80 && Current < rLimit)
                CurrentInternal = rLimit;

            rLimit = Total;
            if (rpState == QuestState.Completed && Current < rLimit)
                CurrentInternal = rLimit;

            State = rpState;
        }
    }
}
