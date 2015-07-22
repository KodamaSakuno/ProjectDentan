
namespace Moen.KanColle.Dentan.Data.QuestData
{
    [Quest(258, QuestType.Once, 2)]
    class SecondSquadronSortieProgress : BossTargetWithRequirement
    {
        public SecondSquadronSortieProgress()
            : base(new[] { 42 }, BattleRank.S) { }
    }
}
