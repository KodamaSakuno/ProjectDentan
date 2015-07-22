using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public class ShipGraphInfo : RawDataWrapper<RawShipGraphInfo>, IID
    {
        public ShipInfo ShipInfo { get; private set; }

        public int ID { get { return RawData.ID; } }
        public string Name { get { return ShipInfo.Name; } }
        public string Filename { get { return RawData.Filename; } }
        public int Version { get { return RawData.Version; } }

        public ShipGraphInfo(RawShipGraphInfo rpRawData, ShipInfo rpShipInfo)
            : base(rpRawData)
        {
            ShipInfo = rpShipInfo;
        }
    }
}
