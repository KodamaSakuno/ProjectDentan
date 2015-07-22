using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawEquipment : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_slotitem_id")]
        public int EquipmentID { get; set; }

        [JsonProperty("api_locked")]
        public int IsLocked { get; set; }

        [JsonProperty("api_level")]
        public int Level { get; set; }
    }
}
