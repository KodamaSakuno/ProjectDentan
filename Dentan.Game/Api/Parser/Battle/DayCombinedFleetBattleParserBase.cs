using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    abstract class DayCombinedFleetBattleParserBase : DayBattleParserBase
    {
        protected BattleStatus[] FriendCombinedStatus { get; set; }
        protected BattleStatus[] AllCombinedFleetStatus { get; set; }

        protected override void ParseStatus(RawBattle rpData)
        {
            base.ParseStatus(rpData);

            FriendCombinedStatus = rpData.MaxHPsCombined.Zip(rpData.NowHPsCombined, (rpMax, rpNow) => rpMax != -1 ? new BattleStatus(rpMax, rpNow) : null).ToArray();
            AllCombinedFleetStatus = FriendCombinedStatus.Concat(EnemyStatus).ToArray();

            if (rpData.EscapedShipIndex != null && rpData.EscapedCombinedShipIndex != null)
            {
                foreach (var rIndex in rpData.EscapedShipIndex)
                    FriendStatus[rIndex - 1].IsEscaped = true;
                foreach (var rIndex in rpData.EscapedCombinedShipIndex)
                    FriendCombinedStatus[rIndex - 1].IsEscaped = true;
            }
        }

        protected override void PostProcess(RawBattle rpData)
        {
            Battle.ParticipatedFleetIDs.Add(1);
            Battle.ParticipatedFleetIDs.Add(2);
            Battle.DayBattle.FriendStatusCombined = FriendCombinedStatus;
            PostProcessCore(rpData);
        }

        protected virtual void ProcessCombinedAerial(RawAerialCombat rpData)
        {
            if (rpData == null) return;

            ProcessAerial(rpData);

            var rBombing = rpData.BombingCombined;
            if (rBombing != null)
                for (var i = 0; i < FriendStatus.Length; i++)
                    FriendCombinedStatus[i].NowHP -= rBombing.AlliedDamage[i];
        }
        protected void ProcessCombinedFleetTorpedoSalvo(RawTorpedoSalvo rpData)
        {
            if (rpData == null) return;

            for (var i = 0; i < FriendCombinedStatus.Length; i++)
                FriendCombinedStatus[i].NowHP -= rpData.FriendDamage[i];

            for (var i = 0; i < EnemyStatus.Length; i++)
                EnemyStatus[i].NowHP -= rpData.EnemyDamage[i];
        }
        protected void ProcessCombinedFleetShelling(RawShelling rpData)
        {
            if (rpData == null) return;

            var rFrom = rpData.ActionOrder;
            var rTargetList = rpData.Defense;
            var rDamageList = rpData.Damage;
            for (var i = 0; i < rFrom.Length; i++)
            {
                var rTarget = rTargetList[i];
                for (var j = 0; j < rTarget.Length; j++)
                    AllCombinedFleetStatus[rTarget[j] - 1].NowHP -= (int)rDamageList[i][j];
            }
        }
    }
}
