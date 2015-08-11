using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Map
{
    [Api("api_req_map/next")]
    class MapNextParser : ApiParser<RawMapNext>
    {
        public override void Process(RawMapNext rpData)
        {
            var rCompassData = Game.CompassData;

            rCompassData.Cell = rpData.Cell;
            rCompassData.CellEvent = rpData.CellEvent;
            rCompassData.CellEventSubID = rpData.CellEventSubID;

            rCompassData.IsManualSelection = rpData.CellEvent == MapCellEvent.Nothing && rpData.CellEventSubID == 2;

            if (rpData.CellEvent != MapCellEvent.NormalBattle && rpData.CellEvent != MapCellEvent.BossBattle)
            {
                Game.Battle = null;
                rCompassData.EnemyFleet = null;
                rCompassData.IsBattleCell = false;
            }
            else
            {
                rCompassData.IsBattleCell = true;

                AbyssalFleet rAbyssalFleet;
                if (AbyssalDataManager.Fleets.TryGetValue(rCompassData.MapID * 100 + rCompassData.Cell, out rAbyssalFleet))
                    rCompassData.EnemyFleet = new EnemyFleet() { Name = rAbyssalFleet.Name };
                else
                    rCompassData.EnemyFleet = null;

                Game.Battle = new BattleData()
                {
                    MapID = rCompassData.MapID,
                    IsBossBattle = rpData.CellEvent == MapCellEvent.BossBattle,
                };
                Game.Battle.DayBattle.FriendStatus = Enumerable.Repeat(BattleStatus.Default, Game.SortieFleet.Ships.Length).ToArray();
                Game.Battle.DayBattle.EnemyStatus = Enumerable.Repeat(BattleStatus.Default, 1).ToArray();

                if (rpData.CellEventSubID == 2)
                {
                    Game.Battle.CanNightBattle = false;
                    Game.Battle.DayBattle.Type = BattlePartType.NightSpecial;
                    Game.Battle.NightBattle = null;
                }
            }

            if (rpData.Whirlpool != null)
            {
                var rFleet = Game.SortieFleet;
                var rCountOfShipWithRader = rFleet.Ships.Count(r => r.Slots.Any(rpSlot => rpSlot.Equipment.IconType == EquipmentIconType.Rader));
                double rRad;
                switch (rCountOfShipWithRader)
                {
                    case 0: rRad = 0.4; break;
                    case 1: rRad = 0.3; break;
                    case 2: rRad = 0.24; break;
                    default: rRad = 0.2; break;
                }

                foreach (var rShip in rFleet.Ships)
                    if (rpData.Whirlpool.Type == 1)
                        rShip.Fuel -= (int)(rShip.Fuel * rRad);
                    else
                        rShip.Bullet -= (int)(rShip.Bullet * rRad);
            }

            if (rpData.ItemGet != null)
            {
                rCompassData.GetItemID = rpData.ItemGet.ID;
                rCompassData.GetItemCount = rpData.ItemGet.Count;
            }
            if (rpData.ItemGetEO != null)
            {
                rCompassData.GetItemID = rpData.ItemGetEO.ID;
                rCompassData.GetItemCount = rpData.ItemGetEO.Count;
            }
            if (rpData.AviationReconnaissance != null)
            {
                rCompassData.AviationReconnaissancePlaneType = rpData.AviationReconnaissance.PlaneType;
                rCompassData.AviationReconnaissanceResult = rpData.AviationReconnaissance.Result;
                if (rCompassData.AviationReconnaissanceResult != AviationReconnaissanceResult.Failure)
                {
                    rCompassData.GetItemID = rpData.ItemGet.ID;
                    rCompassData.GetItemCount = rpData.ItemGet.Count;
                }
            }
        }
    }
}
