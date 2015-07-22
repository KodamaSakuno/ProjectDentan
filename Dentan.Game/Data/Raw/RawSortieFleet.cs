using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawSortieFleet
    {
        [JsonProperty("api_ship_data")]
        public RawShip[] Ships { get; set; }
        [JsonProperty("api_deck_data")]
        public RawFleet[] Fleets { get; set; }
    }
}
