using Newtonsoft.Json;
using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    public class EnemyData : IID
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
                if (r_Equipments == null && EquipmentIDs != null)
                    r_Equipments = EquipmentIDs.Select(r => Equipment.GetDummyFromID(r)).ToArray();
                return r_Equipments;
            }
            set { r_Equipments = value; }
        }

        [JsonIgnore]
        public bool IsDirty { get; internal set; }
    }
}
