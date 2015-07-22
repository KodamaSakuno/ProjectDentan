using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_req_member/get_practice_enemyinfo")]
    class GetPracticeEnemyInfoParser : ApiParser<RawPracticeEnemyInfo>
    {
        public override void Process(RawPracticeEnemyInfo rpData)
        {
            var rLevel = rpData.Fleet.Ships[0].ID != -1 ? rpData.Fleet.Ships[0].Level : 1;
            var rLevel2 = rpData.Fleet.Ships[1].ID != -1 ? rpData.Fleet.Ships[1].Level : 1;
            var rExperience = ExperienceTable.Ship[rLevel].Total / 100.0 + ExperienceTable.Ship[rLevel2].Total / 300.0;
            if (rExperience >= 500.0)
                rExperience = 500.0 + Math.Sqrt(rExperience - 500.0);

            var rCompassData = new CompassData(-1, -1)
            {
                IsBattleCell = true,
                Cell = -1,
            };

            var rEnemyFleet = new EnemyFleet()
            {
                Name = rpData.FleetName,
                ShipIDs = rpData.Fleet.Ships.TakeWhile(r => r.ID != -1).Select(r => r.ShipID).ToArray(),
                IsPracticeFleet = true,
                PracticeExperience = (int)rExperience,
            };
            rEnemyFleet.UpdateShips();
            rCompassData.EnemyFleet = rEnemyFleet;

            Game.CompassData = rCompassData;
        }
    }
}
