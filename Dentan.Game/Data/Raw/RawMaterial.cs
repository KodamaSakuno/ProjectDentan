using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawMaterial
    {
        [JsonProperty("api_id")]
        public MaterialType Type { get; set; }

        [JsonProperty("api_value")]
        public int Value { get; set; }
    }
}
