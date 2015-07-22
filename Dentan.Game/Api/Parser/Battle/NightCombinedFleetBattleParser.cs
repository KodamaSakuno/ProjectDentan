using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_combined_battle/midnight_battle")]
    class NightCombinedFleetBattleParser : NightBattleParserBase
    {
        protected BattleStatus[] EnemyStatus { get; set; }
        protected BattleStatus[] FriendCombinedStatus { get; set; }

        protected override void ParseEnemyInfo(RawNightBattle rpData)
        {
        }

        protected override void ParseStatus(RawNightBattle rpData)
        {
            EnemyStatus = rpData.MaxHPs.Zip(rpData.NowHPs, (rpMax, rpNow) => rpMax != -1 ? new BattleStatus(rpMax, rpNow) : null).Skip(6).TakeWhile(r => r != null).ToArray();
            FriendCombinedStatus = rpData.MaxHPsCombined.Zip(rpData.NowHPsCombined, (rpMax, rpNow) => new BattleStatus(rpMax, rpNow)).ToArray();
            AllStatus = FriendCombinedStatus.Concat(EnemyStatus).ToArray();
        }

        protected override void PostProcess(RawNightBattle rpData)
        {
            var rNightBattle = Battle.NightBattle;

            rNightBattle.FriendStatus = FriendCombinedStatus;
            rNightBattle.EnemyStatus = EnemyStatus;
            rNightBattle.IsInitializing = false;

            Battle.UpdateDamageRate();
        }
    }
}
