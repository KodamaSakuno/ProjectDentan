using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public class BaseMapInfo : RawDataWrapper<RawBaseMapInfo>, IID
    {
        public int ID { get { return RawData.ID; } }
        public bool IsCleared { get; set; }
        public int? RequiredDefeatCount { get { return RawData.RequiredDefeatCount; } }
        public int? DefeatCount { get; set; }
        
        public MapHP? MapHP { get; internal set; }

        public BaseMapInfo(RawBaseMapInfo rpRawData)
            : base(rpRawData) { }

        protected override void OnRawDataUpdated()
        {
            DefeatCount = null;
        }
    }
}
