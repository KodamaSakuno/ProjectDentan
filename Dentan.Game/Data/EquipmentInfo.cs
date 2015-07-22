using Moen.KanColle.Dentan.Data.Raw;
using System.Diagnostics;

namespace Moen.KanColle.Dentan.Data
{
    [DebuggerDisplay("ID={ID}, Name={Name}")]
    public class EquipmentInfo : RawDataWrapper<RawEquipmentInfo>, IID
    {
        static EquipmentInfo r_Default = new EquipmentInfo(new RawEquipmentInfo() { ID = -1, Name = "?", Type = new[] { 0, 0, 0, 0 } });
        public static EquipmentInfo Default { get { return r_Default; } }

        public int ID { get { return RawData.ID; } }
        public string Name { get { return RawData.Name; } }

        public EquipmentType CatagoryType { get { return KanColleGame.Current.Base.EquipmentTypes[RawData.Type[2]]; } }
        public EquipmentIconType IconType { get { return (EquipmentIconType)RawData.Type[3]; } }

        public int AA { get { return RawData.AA; } }
        public int LoS { get { return RawData.LoS; } }

        public EquipmentInfo(RawEquipmentInfo rpRawData)
            : base(rpRawData) { }
    }
}
