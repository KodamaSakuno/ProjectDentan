using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public class EquipmentType : RawDataWrapper<RawEquipmentType>, IID
    {
        public int ID { get { return RawData.ID; } }
        public string Name { get { return RawData.Name; } }

        public EquipmentType(RawEquipmentType rpRawData)
            : base(rpRawData) { }
    }
}
