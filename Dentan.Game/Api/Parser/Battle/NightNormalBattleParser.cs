using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_battle_midnight/battle")]
    class NightNormalBattleParser : NightBattleParserBase
    {
        protected BattleStatus[] FriendStatus { get; set; }
        protected BattleStatus[] EnemyStatus { get; set; }

        protected override void ParseStatus(RawNightBattle rpData)
        {
            AllStatus = rpData.MaxHPs.Zip(rpData.NowHPs, (rpMax, rpNow) => rpMax != -1 ? new BattleStatus(rpMax, rpNow) : null).ToArray();
            FriendStatus = AllStatus.Take(6).TakeWhile(r => r != null).ToArray();
            EnemyStatus = AllStatus.Skip(6).TakeWhile(r => r != null).ToArray();
        }
        protected override void PostProcess(RawNightBattle rpData)
        {
            var rNightBattle = Battle.NightBattle;
            rNightBattle.FriendStatus = FriendStatus;
            rNightBattle.EnemyStatus = EnemyStatus;
            rNightBattle.IsInitializing = false;

            Battle.UpdateDamageRate();
        }

        protected override void ParseEnemyInfo(RawNightBattle rpData)
        {
        }
    }
}
