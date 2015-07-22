using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawModernization
    {
        [JsonProperty("api_powerup_flag")]
        public bool Success { get; set; }
        [JsonProperty("api_ship")]
        public RawShip Ship { get; set; }
        [JsonProperty("api_deck")]
        public RawFleet[] Fleets { get; set; }
    }
}
