using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawUseItemInfo
    {

        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_usetype")]
        public int UseType { get; set; }

        [JsonProperty("api_category")]
        public int Category { get; set; }

        [JsonProperty("api_name")]
        public string Name { get; set; }

        [JsonProperty("api_description")]
        public string[] Description { get; set; }

        [JsonProperty("api_price")]
        public int Price { get; set; }
    }
}
