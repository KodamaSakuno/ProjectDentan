using Moen.KanColle.Dentan.Data.Raw;
using System.Diagnostics;

namespace Moen.KanColle.Dentan.Data
{
    [DebuggerDisplay("ID={ID}, Name={Name}")]
    public class ExpeditionInfo : RawDataWrapper<RawExpeditionInfo>, IID
    {
        public int ID { get { return RawData.ID; } }
        public string Name { get { return RawData.Name; } }
        public string Description { get { return RawData.Description; } }

        public ExpeditionInfo(RawExpeditionInfo rpRawData)
            : base(rpRawData) { }
    }
}
