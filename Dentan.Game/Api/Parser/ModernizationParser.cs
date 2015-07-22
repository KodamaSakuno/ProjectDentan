using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_req_kaisou/powerup")]
    class ModernizationParser : ApiParser<RawModernization>
    {
        public override void Process(RawModernization rpData)
        {
            Ship rShip;
            if (Game.Ships.TryGetValue(rpData.Ship.ID, out rShip))
                rShip.Update(rpData.Ship);

            var rChosenShipIDs = Request["api_id_items"].Split(',').Select(int.Parse).ToArray();
            foreach (var rShipID in rChosenShipIDs)
            {
                rShip = Game.Ships[rShipID];
                foreach (var rSlot in rShip.Slots)
                    if (rSlot.Equipment != Equipment.Default)
                        Game.Equipments.Remove(rSlot.Equipment.ID);
                Game.Ships.Remove(rShipID);
            }
            Game.UpdateEquipments();
            Game.UpdateShips();

            if (rpData.Success)
            {
                Quest.Progresses[702].Current++;
                Quest.Progresses[703].Current++;
            }
        }
    }
    [Api("api_req_kaisou/remodeling")]
    class RemodelingParser : ApiParser
    {
        public override void Process()
        {
        }
    }

}
