using Newtonsoft.Json;
using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    public class AbyssalShip : IID
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("equipments")]
        public int[] EquipmentIDs { get; set; }

        Equipment[] r_Equipments;
        [JsonIgnore]
        public Equipment[] Equipments
        {
            get
            {
                if (KanColleGame.Current.Base == null || KanColleGame.Current.Base.Equipments == null)
                    return null;

                if (r_Equipments == null)
                    r_Equipments = EquipmentIDs.Select(r => Equipment.GetDummyFromID(r)).ToArray();
                return r_Equipments;
            }
            set { r_Equipments = value; }
        }

        [JsonIgnore]
        public bool IsDirty { get; internal set; }

        public AbyssalShip() { }

        public static AbyssalShip FromID(int rpID)
        {
            AbyssalShip rResult;
            AbyssalDataManager.Ships.TryGetValue(rpID, out rResult);
            return rResult;
        }

        public override string ToString()
        {
            return ID.ToString();
        }
    }
}
