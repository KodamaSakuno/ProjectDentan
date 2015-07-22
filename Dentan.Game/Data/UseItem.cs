using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public class UseItem : RawDataWrapper<RawUseItem>
    {
        public int ID { get { return RawData.ID; } }
        public string Name { get { return RawData.Name; } }
        public int Count { get { return RawData.Count; } }

        public UseItem(RawUseItem rpRawData)
            : base(rpRawData) { }

        public override string ToString()
        {
            return string.Format("{0}:{1} ({2})", ID, Name, Count);
        }
    }
}
