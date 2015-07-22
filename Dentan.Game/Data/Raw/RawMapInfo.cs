using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawMapInfo : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_cleared")]
        public bool IsCleared { get; set; }

        [JsonProperty("api_defeat_count")]
        public int? DefeatCount { get; set; }

        [JsonProperty("api_eventmap")]
        public RawEventMap EventMap { get; set; }

        public class RawEventMap
        {

            [JsonProperty("api_now_maphp")]
            public int NowHP { get; set; }

            [JsonProperty("api_max_maphp")]
            public int MaxHP { get; set; }

            [JsonProperty("api_state")]
            public int ApiState { get; set; }

            [JsonProperty("api_selected_rank")]
            public int SelectedRank { get; set; }
        }
    }
}
