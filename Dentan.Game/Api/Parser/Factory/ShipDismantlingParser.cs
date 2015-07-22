using Moen.KanColle.Dentan.Data;

namespace Moen.KanColle.Dentan.Api.Parser.Factory
{
    [Api("api_req_kousyou/destroyship")]
    class ShipDismantlingParser : ApiParser
    {
        public override void Process()
        {
            var rShipID = int.Parse(Request["api_ship_id"]);

            Game.Ships.Remove(rShipID);
            Game.UpdateShips();

            Quest.Progresses[609].Current++;
        }
    }
}
