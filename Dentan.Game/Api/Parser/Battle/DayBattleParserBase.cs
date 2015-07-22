using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    abstract class DayBattleParserBase : BattleParserBase<RawBattle>
    {
        protected BattleStatus[] FriendStatus { get; set; }
        protected BattleStatus[] EnemyStatus { get; set; }

        protected AerialCombatResult AerialCombatResult { get; set; }

        protected override void ParseEnemyInfo(RawBattle rpData)
        {
            ParseEnemyInfoCore(rpData);
        }

        protected void PostProcessCore(RawBattle rpData)
        {
            Battle.CanNightBattle = rpData.CanNightBattle;

            Battle.FriendFormation = rpData.Formation.Friend;
            Battle.EnemyFormation = rpData.Formation.Enemy;
            Battle.EngagementForm = rpData.Formation.EngagementForm;
            Battle.AerialCombat.Result = AerialCombatResult;

            var rDayBattle = Battle.DayBattle;
            rDayBattle.FriendStatus = FriendStatus;
            rDayBattle.EnemyStatus = EnemyStatus;
            rDayBattle.IsInitializing = false;

            Battle.UpdateDamageRate();
        }

        protected override void ParseStatus(RawBattle rpData)
        {
            AllStatus = rpData.MaxHPs.Zip(rpData.NowHPs, (rpMax, rpNow) => rpMax != -1 ? new BattleStatus(rpMax, rpNow) : null).ToArray();
            FriendStatus = AllStatus.Take(6).TakeWhile(r => r != null).ToArray();
            EnemyStatus = AllStatus.Skip(6).TakeWhile(r => r != null).ToArray();
        }
        protected virtual void ProcessAerial(RawAerialCombat rpData)
        {
            if (rpData == null) return;

            var rFighterCombat = rpData.FighterCombat;
            if (rFighterCombat != null)
            {
                AerialCombatResult = rFighterCombat.Result;

                Battle.AerialCombat.Stage1 = new AerialCombat.Stage()
                {
                    FriendPlaneCount = rFighterCombat.FriendPlaneCount,
                    AfterFriendPlaneCount = rFighterCombat.FriendPlaneCount - rFighterCombat.FriendPlaneLost,
                    EnemyPlaneCount = rFighterCombat.EnemyPlaneCount,
                    AfterEnemyPlaneCount = rFighterCombat.EnemyPlaneCount - rFighterCombat.EnemyPlaneLost,
                };
            }

            var rFleetAntiAirDefense = rpData.FleetAntiAirDefense;
            if (rFleetAntiAirDefense != null)
                Battle.AerialCombat.Stage2 = new AerialCombat.Stage()
                {
                    FriendPlaneCount = rFleetAntiAirDefense.FriendPlaneCount,
                    AfterFriendPlaneCount = rFleetAntiAirDefense.FriendPlaneCount - rFleetAntiAirDefense.FriendPlaneLost,
                    EnemyPlaneCount = rFleetAntiAirDefense.EnemyPlaneCount,
                    AfterEnemyPlaneCount = rFleetAntiAirDefense.EnemyPlaneCount - rFleetAntiAirDefense.EnemyPlaneLost,
                };

            var rBombing = rpData.Bombing;
            if (rBombing != null)
            {
                for (var i = 0; i < FriendStatus.Length; i++)
                    FriendStatus[i].NowHP -= rBombing.AlliedDamage[i];

                for (var i = 0; i < EnemyStatus.Length; i++)
                    EnemyStatus[i].NowHP -= rBombing.EnemyDamage[i];
            }
        }
        protected void ProcessSupportAttack(RawSupportInfo rpData)
        {
            if (rpData == null) return;

            if (rpData.Shelling != null)
                for (var i = 0; i < EnemyStatus.Length; i++)
                    EnemyStatus[i].NowHP -= rpData.Shelling.Damage[i];
        }
        protected void ProcessTorpedoSalvo(RawTorpedoSalvo rpData)
        {
            if (rpData == null) return;

            for (var i = 0; i < FriendStatus.Length; i++)
                FriendStatus[i].NowHP -= rpData.FriendDamage[i];

            for (var i = 0; i < EnemyStatus.Length; i++)
                EnemyStatus[i].NowHP -= rpData.EnemyDamage[i];
        }
    }
}
