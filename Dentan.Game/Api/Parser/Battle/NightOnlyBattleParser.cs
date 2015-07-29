using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_battle_midnight/sp_midnight")]
    class NightOnlyBattleParser : NightNormalBattleParser
    {
        protected override void ParseEnemyInfo(RawNightBattle rpData)
        {
            ParseEnemyInfoCore(rpData);
        }

        protected override void PostProcess(RawNightBattle rpData)
        {
            Battle.ParticipatedFleetIDs.Add(rpData.FleetID);
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
            rNightBattle.FriendStatus = FriendStatus;
            rNightBattle.EnemyStatus = EnemyStatus;
            rNightBattle.IsInitializing = false;

            Battle.UpdateDamageRate();
        }
    }
}
