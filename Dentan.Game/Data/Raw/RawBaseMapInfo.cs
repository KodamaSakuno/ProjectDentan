using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawBaseMapInfo : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_maparea_id")]
        public int MapAreaID { get; set; }

        [JsonProperty("api_no")]
        public int MapNo { get; set; }

        [JsonProperty("api_name")]
        public string Name { get; set; }

        [JsonProperty("api_level")]
        public int Level { get; set; }

        [JsonProperty("api_opetext")]
        public string ApiOpetext { get; set; }

        [JsonProperty("api_infotext")]
        public string ApiInfotext { get; set; }

        [JsonProperty("api_item")]
        public int[] ApiItem { get; set; }

        [JsonProperty("api_max_maphp")]
        public object ApiMaxMaphp { get; set; }

        [JsonProperty("api_required_defeat_count")]
        public int? RequiredDefeatCount { get; set; }

        [JsonProperty("api_sally_flag")]
        public int[] ApiSallyFlag { get; set; }
    }

}
