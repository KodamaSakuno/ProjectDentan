using Moen.KanColle.Dentan.Data.Raw;
using System.Diagnostics;
using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    [DebuggerDisplay("ID={ID}, Name={Name}")]
    public class ExpeditionInfo : RawDataWrapper<RawExpeditionInfo>, IID
    {
        public int ID { get { return RawData.ID; } }
        public int MapArea { get { return RawData.MapAreaID; } }
        public string Name { get { return RawData.Name; } }
        public string Description { get { return RawData.Description; } }
        public int Time { get { return RawData.Time; } }
        public double FuelConsumption { get { return RawData.FuelConsumption; } }
        public double BulletConsumption { get { return RawData.BulletConsumption; } }
        public int[] GetItem1 { get { return RawData.GetItem1; } }
        public int[] GetItem2 { get { return RawData.GetItem2; } }
        public bool CanReturn { get { return RawData.CanReturn; } }

        public ExpeditionData Data { get; private set; }

        public ExpeditionFleetInfo[] FleetsInfo { get; private set; }

        public ExpeditionInfo(RawExpeditionInfo rpRawData)
            : base(rpRawData)
        {
            Data = ExpeditionDataManager.Data[rpRawData.ID];
            FleetsInfo = new ExpeditionFleetInfo[3];
            for (var i = 0; i < 3; i++)
                FleetsInfo[i] = new ExpeditionFleetInfo(i + 2);
        }

        public void Update(Fleet rpFleet)
        {
            var rInfo = FleetsInfo[rpFleet.ID - 2];
            var rShips = rpFleet.Ships;

            rInfo.CanSuccess = rShips[0].Level >= Data.FlagshipLevel &&
                (Data.FlagshipType == null ? true : rShips[0].Info.Type == Data.FlagshipType) &&
                rpFleet.TotalLevel >= Data.TotalLevel &&
                rShips.Length >= Data.ShipCount &&
                (Data.Drum == null ? true : (rShips.Count(r => r.Slots.Any(rpSlot => rpSlot.Equipment.IconType == EquipmentIconType.DrumCanister)) >= Data.ShipCount &&
                    rShips.Sum(r => r.Slots.Count(rpSlot => rpSlot.Equipment.IconType == EquipmentIconType.DrumCanister)) >= Data.Drum.Count)) &&
                (Data.RequiredShipTypes == null ? true : Data.RequiredShipTypes.All(r => r.Types.All(rpType => rShips.Count(rpShip => rpShip.Info.Type == rpType) >= r.Count)));
        }
    }
}
