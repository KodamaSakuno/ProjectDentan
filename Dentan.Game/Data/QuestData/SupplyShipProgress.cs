using System.Linq;

namespace Moen.KanColle.Dentan.Data.QuestData
{
    abstract class SupplyShipProgress : DestroyTargetProgress
    {
        protected SupplyShipProgress()
            :base(new[] { 15 }) { }
    }
    abstract class SpecialSupplyShipProgress : SupplyShipProgress
    {
        int r_ID;

        protected SpecialSupplyShipProgress(int rpID)
        {
            r_ID = rpID;
        }

        internal override void ProcessDay(BattleData rpBattle)
        {
            var rCount = rpBattle.DayBattle.EnemyStatus.Count(r => r.DamageStatus == ShipDamageStatus.Sink && TargetType.Contains(r.ShipInfo.Info.Type));

            Current += rCount;
            Quest.Progresses[r_ID].Current += rCount;
        }
        internal override void ProcessNight(BattleData rpBattle)
        {
            var rProgress = Current;
            var rProgress2 = Quest.Progresses[r_ID].Current;

            var rCount = rpBattle.DayBattle.EnemyStatus.Count(r => r.DamageStatus == ShipDamageStatus.Sink && TargetType.Contains(r.ShipInfo.Info.Type));
            rProgress -= rCount;
            rProgress2 -= rCount;

            rCount = rpBattle.NightBattle.EnemyStatus.Count(r => r.DamageStatus == ShipDamageStatus.Sink && TargetType.Contains(r.ShipInfo.Info.Type));
            rProgress += rCount;
            rProgress2 += rCount;

            Current = rProgress;
            Quest.Progresses[r_ID].Current = rProgress2;
        }
    }

    [Quest(218, QuestType.Daily, 3)]
    class SupplyShipDailyProgress : SpecialSupplyShipProgress
    {
        public SupplyShipDailyProgress()
            : base(212) { }
    }
    [Quest(213, QuestType.Weekly, 20)]
    class SupplyShipWeeklyProgress : SupplyShipProgress
    {
    }
    [Quest(212, QuestType.Daily, 5)]
    class SupplyShipSpecialProgress : SpecialSupplyShipProgress
    {
        public SupplyShipSpecialProgress()
            : base(218) { }
    }
    [Quest(221, QuestType.Weekly, 50)]
    class CodeRoProgress : SupplyShipProgress
    {
    }
}
