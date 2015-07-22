using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawEquipmentInfo : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_sortno")]
        public int SortNo { get; set; }

        [JsonProperty("api_name")]
        public string Name { get; set; }

        [JsonProperty("api_type")]
        public int[] Type { get; set; }

        [JsonProperty("api_taik")]
        public int ApiTaik { get; set; }

        [JsonProperty("api_souk")]
        public int ApiSouk { get; set; }

        [JsonProperty("api_houg")]
        public int ApiHoug { get; set; }

        [JsonProperty("api_raig")]
        public int ApiRaig { get; set; }

        [JsonProperty("api_soku")]
        public int ApiSoku { get; set; }

        [JsonProperty("api_baku")]
        public int ApiBaku { get; set; }

        [JsonProperty("api_tyku")]
        public int AA { get; set; }

        [JsonProperty("api_tais")]
        public int ApiTais { get; set; }

        [JsonProperty("api_atap")]
        public int ApiAtap { get; set; }

        [JsonProperty("api_houm")]
        public int ApiHoum { get; set; }

        [JsonProperty("api_raim")]
        public int ApiRaim { get; set; }

        [JsonProperty("api_houk")]
        public int ApiHouk { get; set; }

        [JsonProperty("api_raik")]
        public int ApiRaik { get; set; }

        [JsonProperty("api_bakk")]
        public int ApiBakk { get; set; }

        [JsonProperty("api_saku")]
        public int LoS { get; set; }

        [JsonProperty("api_sakb")]
        public int ApiSakb { get; set; }

        [JsonProperty("api_luck")]
        public int ApiLuck { get; set; }

        [JsonProperty("api_leng")]
        public int ApiLeng { get; set; }

        [JsonProperty("api_rare")]
        public int ApiRare { get; set; }

        [JsonProperty("api_broken")]
        public int[] ApiBroken { get; set; }

        [JsonProperty("api_info")]
        public string ApiInfo { get; set; }

        [JsonProperty("api_usebull")]
        public string UseBullet { get; set; }
    }
}
