﻿using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_sortie/battleresult")]
    [Api("api_req_combined_battle/battleresult")]
    class BattleResultParser : ApiParser<RawBattleResult>
    {
        public override void Process(RawBattleResult rpData)
        {
            var rCompassData = Game.CompassData;
            var rEnemyFleet = rCompassData.EnemyFleet;

            if (rpData.EnemyInfo != null)
            {
                rEnemyFleet.Name = rpData.EnemyInfo.FleetName;
                if (rEnemyFleet.AbyssalFleet == null && !AbyssalDataManager.Fleets.ContainsKey(rCompassData.MapID * 100 + rCompassData.Cell))
                    AbyssalDataManager.Fleets.Add(new AbyssalFleet() { Map = rCompassData.MapID, Cell = rCompassData.Cell, Name = rEnemyFleet.Name, IsDirty = true });
            }

            if (rpData.DropShip != null)
            {
                Game.DroppedShip++;

                Game.SendMessageToStatusBar($"获得「{rpData.DropShip.Name}」");
            }

            AbyssalDataManager.Update();
            RecordManager.Instance.Sortie.Update(rEnemyFleet.AbyssalFleet, rpData);

            RecordManager.Instance.Battle.UpdateSortie(rCompassData, Game.Battle, rpData.Rank);
        }
    }
}
