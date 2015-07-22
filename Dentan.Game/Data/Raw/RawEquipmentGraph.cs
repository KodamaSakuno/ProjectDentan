using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawEquipmentGraph
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_sortno")]
        public int SortNo { get; set; }

        [JsonProperty("api_filename")]
        public string Filename { get; set; }

        [JsonProperty("api_version")]
        public string Version { get; set; }
    }
}
