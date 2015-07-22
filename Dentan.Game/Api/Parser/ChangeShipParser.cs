namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_req_hensei/change")]
    class ChangeShipParser : ApiParser
    {
        public override void Process()
        {
            var rFleetID = int.Parse(Request["api_id"]);
            var rIndex = int.Parse(Request["api_ship_idx"]);
            var rShipID = int.Parse(Request["api_ship_id"]);

            var rFleet = Game.Fleets[rFleetID];
            var rShips = rFleet.ShipList;

            if (rShipID == -1)
                rShips.RemoveAt(rIndex);
            else if (rShipID == -2)
                rShips.RemoveRange(1, rShips.Count - 1);
            else
            {
                var rShip = Game.Ships[rShipID];

                var rOriginalFleet = rShip.OwnerFleet;
                if (rOriginalFleet != null)
                {
                    var rOriginalIndex = rOriginalFleet.ShipList.IndexOf(rShip);
                    if (rIndex < rShips.Count)
                        rOriginalFleet.ShipList[rOriginalIndex] = rShips[rIndex];
                    else
                        rOriginalFleet.ShipList.RemoveAt(rOriginalIndex);

                    rOriginalFleet.UpdateShips();
                }

                if (rIndex >= rShips.Count)
                    rShips.Add(rShip);
                else
                    rShips[rIndex] = rShip;
            }

            rFleet.UpdateShips();
        }
    }
}
