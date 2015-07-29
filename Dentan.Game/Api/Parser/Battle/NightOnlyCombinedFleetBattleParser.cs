using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_combined_battle/sp_midnight")]
    class NightOnlyCombinedFleetBattleParser : NightBattleParserBase
    {
        protected BattleStatus[] EnemyStatus { get; set; }
        protected BattleStatus[] FriendCombinedStatus { get; set; }

        protected override void ParseEnemyInfo(RawNightBattle rpData)
        {
            ParseEnemyInfoCore(rpData);
        }

        protected override void ParseStatus(RawNightBattle rpData)
        {
            EnemyStatus = rpData.MaxHPs.Zip(rpData.NowHPs, (rpMax, rpNow) => rpMax != -1 ? new BattleStatus(rpMax, rpNow) : null).Skip(6).TakeWhile(r => r != null).ToArray();
            FriendCombinedStatus = rpData.MaxHPsCombined.Zip(rpData.NowHPsCombined, (rpMax, rpNow) => new BattleStatus(rpMax, rpNow)).ToArray();
            AllStatus = FriendCombinedStatus.Concat(EnemyStatus).ToArray();
        }

        protected override void PostProcess(RawNightBattle rpData)
        {
            Battle.ParticipatedFleetIDs.Add(2);
            Battle.CanNightBattle = false;
            Battle.DayBattle.Type = BattlePartType.NightSpecial;
            Battle.NightBattle = null;

            if (rpData.Formation != null)
            {
                Battle.FriendFormation = rpData.Formation.Friend;
                Battle.EnemyFormation = rpData.Formation.Enemy;
                Battle.EngagementForm = rpData.Formation.EngagementForm;
            }

            var rNightBattle = Battle.DayBattle;
            rNightBattle.FriendStatus = FriendCombinedStatus;
            rNightBattle.EnemyStatus = EnemyStatus;
            rNightBattle.IsInitializing = false;
            Battle.UpdateDamageRate();
        }
    }
}
