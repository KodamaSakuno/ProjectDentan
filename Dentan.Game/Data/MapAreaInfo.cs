using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public class MapAreaInfo : RawDataWrapper<RawMapArea>, IID
    {
        public int ID { get { return RawData.ID; } }
        public string Name { get { return RawData.Name; } }
        public bool IsEventMap { get { return RawData.IsEventMap; } }

        public MapAreaInfo(RawMapArea rpRawData)
            : base(rpRawData) { }
    }
}
