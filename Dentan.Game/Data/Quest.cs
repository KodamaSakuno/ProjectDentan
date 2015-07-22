using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public enum QuestType { Once = 1, Daily, Weekly, Special1, Special2, Monthly, }
    public enum QuestState { None = 1, Progress, Completed }
    public enum QuestProgress { None, Progress50, Progress80, }

    public partial class Quest : RawDataWrapper<RawQuest>, IID
    {
        static Quest r_Default;
        public static Quest Default { get { return r_Default; } }

        public int ID { get { return RawData.ID; } }
        public string Title { get { return RawData.Title; } }
        public string Description { get { return RawData.Detail; } }
        public QuestCategory Category { get { return RawData.Category; } }
        public QuestType Type { get { return RawData.Type; } }
        public QuestState State { get { return RawData.State; } }
        public QuestProgress Progress { get { return RawData.Progress; } }

        public bool IsStarted { get { return State == QuestState.Progress || State == QuestState.Completed; } }

        bool r_CanCompleted;
        public bool CanCompleted
        {
            get { return r_CanCompleted; }
            set
            {
                if (r_CanCompleted != value)
                {
                    r_CanCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public QuestProgressData ProgressData { get; private set; }

        public Quest(RawQuest rpRawData)
            : base(rpRawData)
        {
            QuestProgressData rData;
            if (Progresses.TryGetValue(ID, out rData))
                ProgressData = rData;
        }

        protected override void OnRawDataUpdated()
        {
            if (ProgressData != null)
                ProgressData.State = State;
        }
    }
}
