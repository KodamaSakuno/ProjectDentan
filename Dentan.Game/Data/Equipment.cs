using Moen.KanColle.Dentan.Data.Raw;
using System.Collections.Generic;

namespace Moen.KanColle.Dentan.Data
{
    public class Equipment : RawDataWrapper<RawEquipment>, IID
    {
        static Equipment r_Default = new Equipment(new RawEquipment() { ID = -1, EquipmentID = -1 });
        public static Equipment Default { get { return r_Default; } }

        static Dictionary<int, Equipment> r_Cache = new Dictionary<int, Equipment>();

        EquipmentInfo r_Info;
        public EquipmentInfo Info
        {
            get { return r_Info; }
            set
            {
                if (r_Info != value)
                {
                    r_Info = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ID { get { return RawData.ID; } }
        public string Name { get { return Info.Name; } }
        public int Level { get { return RawData.Level; } }

        public EquipmentIconType IconType { get { return Info.IconType; } }

        public bool IsPlane { get { return Info.RawData.Type[1] == 5 || Info.RawData.Type[1] == 7; } }

        internal Equipment(RawEquipment rpRawData)
            : base(rpRawData) { }

        protected override void OnRawDataUpdated()
        {
            if (Info == null || Info.ID != RawData.EquipmentID)
            {
                EquipmentInfo rInfo;
                if (!KanColleGame.Current.Base.Equipments.TryGetValue(RawData.EquipmentID, out rInfo))
                    rInfo = EquipmentInfo.Default;
                Info = rInfo;
            }

            OnPropertyChanged(() => Name);
            OnPropertyChanged(() => Level);
            OnPropertyChanged(() => IconType);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1} ({2})", ID, Name, Level);
        }

        public static Equipment GetDummyFromID(int rpID)
        {
            Equipment rResult;
            if (!r_Cache.TryGetValue(rpID, out rResult))
                rResult = new Equipment(new RawEquipment() { EquipmentID = rpID });
            return rResult;
        }
    }
}
