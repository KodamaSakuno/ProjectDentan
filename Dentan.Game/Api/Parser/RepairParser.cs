using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_req_nyukyo/start")]
    class RepairParser : ApiParser
    {
        public override void Process()
        {
            var rDockID = int.Parse(Request["api_ndock_id"]);
            var rShipID = int.Parse(Request["api_ship_id"]);

            if (Request["api_highspeed"] == "1")
            {
                var rShip = Game.Ships[rShipID];
                rShip.NowHP = rShip.MaxHP;
                if (rShip.Condition < 40)
                    rShip.Condition = 40;
            }

            Quest.Progresses[503].Current++;
        }
    }

    [Api("api_req_nyukyo/speedchange")]
    class RepairHighSpeedParser : ApiParser
    {
        public override void Process()
        {
            var rDockID = int.Parse(Request["api_ndock_id"]);

            //Game.RepairDocks[rDockID].RepairingComplete();
        }
    }

    [Api("api_get_member/ndock")]
    class GetRepairDockParser : ApiParser<RawRepairDock[]>
    {
        public override void Process(RawRepairDock[] rpDocks)
        {
            Game.UpdateRepairDocks(rpDocks);
        }
    }
}
