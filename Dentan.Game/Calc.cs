using Moen.KanColle.Dentan.Data;
using System;
using System.Linq;

namespace Moen.KanColle.Dentan
{
    public static class Calc
    {
        public static double CalcLos(Fleet rpFleet)
        {
            var rSpotter = (from rShip in rpFleet.Ships
                            from rSlot in rShip.Slots
                            where rSlot.Equipment.Info.RawData.Type[1] == 7
                            where rSlot.PlaneCount > 0
                            select rSlot.Equipment.Info.LoS).Sum();
            var rRadar = (from rShip in rpFleet.Ships
                          from rSlot in rShip.Slots
                          where rSlot.Equipment.Info.RawData.Type[1] == 8
                          select rSlot.Equipment.Info.LoS).Sum();

            return rSpotter * 2 + rRadar + Math.Sqrt(rpFleet.Ships.Sum(r => r.LoS) - rSpotter - rRadar);
        }
    }
}
