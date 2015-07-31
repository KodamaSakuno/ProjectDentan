using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public class EquipmentTypeInfo : RawDataWrapper<RawEquipmentTypeInfo>, IID
    {
        public int ID { get { return RawData.ID; } }
        public string Name { get { return RawData.Name; } }

        public EquipmentTypeInfo(RawEquipmentTypeInfo rpRawData)
            : base(rpRawData) { }
    }
}
