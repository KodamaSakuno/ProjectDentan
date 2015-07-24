using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public class UseItemInfo : RawDataWrapper<RawUseItemInfo>, IID
    {
        public int ID { get { return RawData.ID; } }
        public string Name { get { return RawData.Name; } }

        public UseItemInfo(RawUseItemInfo rpRawData)
            : base(rpRawData) { }

        public override string ToString()
        {
            return string.Format("{0}:{1}", ID, Name);
        }
    }
}
