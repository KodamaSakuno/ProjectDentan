
namespace Moen.KanColle.Dentan.Data.QuestData
{
    abstract class PracticeProgress : QuestProgressData
    {
        public bool NeedWin { get; private set; }

        protected PracticeProgress(bool rpNeedWin)
        {
            NeedWin = rpNeedWin;
        }

        internal void Process(BattleData rpBattle)
        {
            if (rpBattle.QuestCounter.Contains(ID) || NeedWin && rpBattle.Rank < BattleRank.B)
                return;

            Current++;
            rpBattle.QuestCounter.Add(ID);
        }
    }
    [Quest(303, QuestType.Daily, 3)]
    class Practice3DailyProgress : PracticeProgress
    {
        public Practice3DailyProgress()
            : base(false) { }
    }
    [Quest(304, QuestType.Daily, 5)]
    class Practice5DailyProgress : PracticeProgress
    {
        public Practice5DailyProgress()
            : base(true) { }
    }
    [Quest(302, QuestType.Weekly, 20)]
    class PracticeWeeklyProgress : PracticeProgress
    {
        public PracticeWeeklyProgress()
            : base(true) { }
    }

    [Quest(307, QuestType.Daily, 3)]
    class PracticeSpecialProgress : PracticeProgress
    {
        public PracticeSpecialProgress()
            : base(true) { }
    }
    [Quest(308, QuestType.Daily, 4)]
    class PracticeSpecial2Progress : PracticeProgress
    {
        public PracticeSpecial2Progress()
            : base(true) { }
    }
}
