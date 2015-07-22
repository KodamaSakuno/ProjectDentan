using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_req_hokyu/charge")]
    class SupplyParser : ApiParser<RawSupplyResult>
    {
        public override void Process(RawSupplyResult rpData)
        {
            foreach (var rShipInfo in rpData.Ships)
            {
                var rShip = Game.Ships[rShipInfo.ID];
                rShip.BeforeFuel = rShip.Fuel = rShipInfo.Fuel;
                rShip.BeforeBullet = rShip.Bullet = rShipInfo.Bullet;

                var rPlanes = rShipInfo.Planes;
                for (var i = 0; i < rShip.Slots.Length; i++)
                {
                    var rCount = (int)rPlanes[i];
                    if (rCount > 0)
                        rShip.Slots[i].PlaneCount = rCount;
                }
            }

            Quest.Progresses[504].Current++;
        }
    }
}
