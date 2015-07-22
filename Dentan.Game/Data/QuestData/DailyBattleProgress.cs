namespace Moen.KanColle.Dentan.Data.QuestData
{
    abstract class DailyBattleProgress : QuestProgressData
    {

    }
    [Quest(201, QuestType.Daily, 1)]
    class DailyBattleFirstProgress : DailyBattleProgress
    {
    }
    [Quest(216, QuestType.Daily, 1)]
    class DailyBattleSecondProgress : DailyBattleProgress
    {
    }
    [Quest(210, QuestType.Daily, 10)]
    class Daily10BattleProgress : DailyBattleProgress
    {
    }
}
