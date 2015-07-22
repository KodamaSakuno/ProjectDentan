using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawShipType
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_sortno")]
        public int SortNo { get; set; }

        [JsonProperty("api_name")]
        public string ApiName { get; set; }

        [JsonProperty("api_scnt")]
        public int ApiScnt { get; set; }

        [JsonProperty("api_kcnt")]
        public int ApiKcnt { get; set; }

        //[JsonProperty("api_equip_type")]
        //public ApiEquipType ApiEquipType { get; set; }
    }

}
