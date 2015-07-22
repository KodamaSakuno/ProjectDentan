using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_get_member/ship2")]
    class GetShip2Parser : ApiParser<RawShip[]>
    {
        public override void Process(RawShip[] rpData)
        {
            Game.UpdateShips(rpData);
            Game.UpdateFleets(ResponseJson["api_data_deck"].ToObject<RawFleet[]>());
        }
    }
    [Api("api_get_member/ship3")]
    class GetShip3Parser : ApiParser<RawGetShip3>
    {
        public override void Process(RawGetShip3 rpData)
        {
            foreach (var rShip in rpData.Ships)
                Game.Ships[rShip.ID].Update(rShip);
            Game.UpdateFleets(rpData.Fleets);
        }
    }
}
