namespace Moen.KanColle.Dentan.Data.QuestData
{
    abstract class ASWProgress : DestroyTargetProgress
    {
        protected ASWProgress()
            : base(new[] { 13 }) { }
    }
    [Quest(228, QuestType.Weekly, 15)]
    class ASWWeeklyProgress : ASWProgress
    {
    }
    [Quest(230, QuestType.Daily, 6)]
    class ASWDailyProgress : ASWProgress
    {
    }
}
