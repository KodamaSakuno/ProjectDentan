using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.QuestData;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_practice/battle")]
    class DayPracticeBattleParser : DayBattleParserBase
    {
        protected override void ProcessCore(RawBattle rpData)
        {
            ProcessAerial(rpData.AerialCombat);
            ProcessAerial(rpData.AerialCombatSecondRound);

            ProcessTorpedoSalvo(rpData.OpeningTorpedoSalvo);

            ProcessShelling(rpData.ShellingFirstRound);
            ProcessShelling(rpData.ShellingSecondRound);

            ProcessTorpedoSalvo(rpData.ClosingTorpedoSalvo);
        }

        protected override void ParseEnemyInfo(RawBattle rpData)
        {
            var rSortieFleetID = int.Parse(Request["api_deck_id"]);
            Game.SortieFleet = Game.Fleets[rSortieFleetID];

            var rEnemyFleet = Game.CompassData.EnemyFleet;
            var rEnemyIDs = rpData.EnemyIDs.TakeWhile(r => r != -1).ToArray();
            if (!rEnemyFleet.ShipIDs.SequenceEqual(rEnemyIDs))
            {
                var rFleetName = rEnemyFleet.Name;
                rEnemyFleet = new EnemyFleet()
                {
                    Name = rFleetName,
                    Formation = rpData.Formation.Enemy,
                    ShipIDs = rEnemyIDs,
                };
                rEnemyFleet.UpdateShips();
            }

            var rShips = rEnemyFleet.Ships;
            for (var i = 0; i < rShips.Length; i++)
            {
                var rShip = rShips[i];

                rShip.Level = rpData.EnemyLevels[i];

                var rSlots = rShip.Slots;
                for (var j = 0; j < rSlots.Length; j++)
                {
                    var rID = rpData.EnemyEquipments[i][j];
                    rSlots[j].Equipment = rID != -1 ? Equipment.GetDummyFromID(rID) : Equipment.Default;
                }
            }

            rEnemyFleet.UpdateAA();
        }

        protected override void PostProcess(RawBattle rpData)
        {
            Battle.ParticipatedFleetIDs.Add(int.Parse(Request["api_deck_id"]));
            Battle.CanNightBattle = rpData.CanNightBattle;

            PostProcessCore(rpData);
        }

        protected override void ProcessQuest(BattleData rpData)
        {
            foreach (var rProgress in Quest.Progresses.Values.OfType<PracticeProgress>())
                rProgress.Process(rpData);
        }
    }
}
