using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawCreateEquipment
    {
        [JsonProperty("api_create_flag")]
        public bool Success { get; set; }

        [JsonProperty("api_slot_item")]
        public RawEquipmentResult Result { get; set; }

        [JsonProperty("api_material")]
        public int[] Materials { get; set; }

        public class RawEquipmentResult
        {
            [JsonProperty("api_id")]
            public int ID { get; set; }

            [JsonProperty("api_slotitem_id")]
            public int EquipmentID { get; set; }
        }
    }
}
