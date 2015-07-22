using Moen.KanColle.Dentan.Data;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class QuestViewModel : ViewModel<Quest>
    {
        bool r_IsStarted;
        public bool IsStarted
        {
            get { return r_IsStarted; }
            set
            {
                if (r_IsStarted != value)
                {
                    r_IsStarted = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ID { get { return Model.ID; } }
        public string Title { get { return Model.Title; } }
        public string Description { get { return Model.Description; } }
        public QuestCategory Category { get { return Model.Category; } }
        public QuestType Type { get { return Model.Type; } }
        public QuestState State { get { return Model.State; } }
        public QuestProgress Progress { get { return Model.Progress; } }

        public bool CanCompleted { get { return Model.CanCompleted; } }
        public QuestProgressData ProgressData { get { return Model.ProgressData; } }

        public QuestViewModel(Quest rpQuest)
            : base(rpQuest)
        {
            IsStarted = rpQuest.State == QuestState.Progress || rpQuest.State == QuestState.Completed;
        }
    }
}
