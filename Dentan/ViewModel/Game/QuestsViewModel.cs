using System.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class QuestsViewModel : ModelBase
    {
        QuestViewModel[] r_Quests;
        public QuestViewModel[] Quests
        {
            get { return r_Quests; }
            set
            {
                if (r_Quests != value)
                {
                    r_Quests = value;
                    OnPropertyChanged();
                }
            }
        }
        QuestViewModel[] r_StartedQuests;
        public QuestViewModel[] StartedQuests
        {
            get { return r_StartedQuests; }
            set
            {
                if (r_StartedQuests != value)
                {
                    r_StartedQuests = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_StartedQuestCount;
        public int StartedQuestCount
        {
            get { return r_StartedQuestCount; }
            set
            {
                if (r_StartedQuestCount != value)
                {
                    r_StartedQuestCount = value;
                    OnPropertyChanged();
                }
            }
        }

        internal void Update()
        {
            var rQuests = KanColleGame.Current.Quests.Select(r => new QuestViewModel(r)).ToLookup(r => r.IsStarted);

            Quests = rQuests[false].ToArray();
            StartedQuests = rQuests[true].ToArray();
            StartedQuestCount = StartedQuests.Length;
        }
    }
}
