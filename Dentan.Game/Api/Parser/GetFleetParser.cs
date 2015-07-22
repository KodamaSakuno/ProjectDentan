using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_get_member/deck")]
    class GetFleetParser : ApiParser<RawFleet[]>
    {
        public override void Process(RawFleet[] rpData)
        {
            Game.UpdateFleets(rpData);
        }
    }
    [Api("api_get_member/ship_deck")]
    class GetSortieFleetParser : ApiParser<RawSortieFleet>
    {
        public override void Process(RawSortieFleet rpData)
        {
            foreach (var rFleet in rpData.Fleets)
                Game.Fleets[rFleet.ID].Update(rFleet);
            foreach (var rShip in rpData.Ships)
                Game.Ships[rShip.ID].Update(rShip);

            Game.UpdateShips();
        }
    }
}
