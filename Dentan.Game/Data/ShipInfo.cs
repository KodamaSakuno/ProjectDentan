using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    using System.Linq;
    using AbyssalShipClassEnum = AbyssalShipClass;

    public class ShipInfo : RawDataWrapper<RawShipInfo>, IID
    {
        static ShipInfo r_Default = new ShipInfo(new RawShipInfo() { ID = -1, Name = "?" });
        public static ShipInfo Default { get { return r_Default; } }

        public int ID { get { return RawData.ID; } }
        public int SortID { get { return RawData.SortID; } }
        public int Type { get { return RawData.Type; } }
        public string Name { get { return RawData.Name; } }
        public string YomiName { get { return RawData.Yomi; } }
        public ShipSpeed Speed { get { return RawData.Speed; } }

        public int RemodelAfterLevel { get { return RawData.RemodelAfterLevel; } }
        ShipInfo r_RemodelAfterShipInfo;
        public ShipInfo RemodelAfterShipInfo
        {
            get
            {
                if (r_RemodelAfterShipInfo == null)
                {
                    var rShipInfos = KanColleGame.Current.Base.Ships;

                    ShipInfo rInfo;
                    if (!rShipInfos.TryGetValue(RawData.RemodelAfterShipID, out rInfo))
                        r_RemodelAfterShipInfo = r_Default;
                    else
                        r_RemodelAfterShipInfo = rInfo;
                }
                return r_RemodelAfterShipInfo;
            }
        }

        public int MaxFuel { get { return RawData.MaxFuel; } }
        public int MaxBullet { get { return RawData.MaxBullet; } }

        public int EquipmentCount { get { return RawData.EquipmentCount; } }
        public int[] PlaneCount
        {
            get
            {
                var rResult = RawData.PlaneCount;
                if (rResult == null && !AbyssalDataManager.ShipsPlaneCount.TryGetValue(ID, out rResult))
                    rResult = Enumerable.Repeat(0, RawData.EquipmentCount).ToArray();
                return rResult;
            }
        }

        public AbyssalShipClass? AbyssalShipClass
        {
            get
            {
                if (ID > 500 && ID <= 900)
                {
                    if (Name.Contains("後期型") && YomiName.IsNullOrEmpty())
                        return AbyssalShipClassEnum.LateModel;

                    if (YomiName == "elite")
                        return AbyssalShipClassEnum.Elite;
                    if (YomiName == "flagship")
                        return AbyssalShipClassEnum.Flagship;

                    return AbyssalShipClassEnum.Normal;
                }
                return null;
            }
        }

        public ShipInfo(RawShipInfo rpRawData)
            : base(rpRawData) { }

        public override string ToString()
        {
            return string.Format("ID={0}, Name={1}", ID, Name);
        }
    }
}
