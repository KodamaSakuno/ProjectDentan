using System.Collections.Generic;
using System.Linq;

namespace Moen.KanColle.Dentan.Data.QuestData
{
    abstract class DestroyTargetProgress : QuestProgressData
    {
        public HashSet<int> TargetType { get; private set; }

        protected DestroyTargetProgress(int[] rpTargetType)
        {
            TargetType = new HashSet<int>(rpTargetType);
        }

        internal virtual void ProcessDay(BattleData rpBattle)
        {
            Current += rpBattle.DayBattle.EnemyStatus.Count(r => r.DamageStatus == ShipDamageStatus.Sink && TargetType.Contains(r.ShipInfo.Info.Type));
        }
        internal virtual void ProcessNight(BattleData rpBattle)
        {
            Current += rpBattle.NightBattle.EnemyStatus.Count(r => r.DamageStatus == ShipDamageStatus.Sink && TargetType.Contains(r.ShipInfo.Info.Type)) -
                rpBattle.DayBattle.EnemyStatus.Count(r => r.DamageStatus == ShipDamageStatus.Sink && TargetType.Contains(r.ShipInfo.Info.Type));
        }
    }
}
