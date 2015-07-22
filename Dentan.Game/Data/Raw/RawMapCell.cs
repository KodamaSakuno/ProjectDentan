using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawMapCell
    {
        [JsonProperty("api_map_no")]
        public int MapNo { get; set; }

        [JsonProperty("api_maparea_id")]
        public int MapAreaID { get; set; }

        [JsonProperty("api_mapinfo_no")]
        public int MapInfoNo { get; set; }

        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_no")]
        public int No { get; set; }

        [JsonProperty("api_color_no")]
        public int ColorNo { get; set; }
    }
}
