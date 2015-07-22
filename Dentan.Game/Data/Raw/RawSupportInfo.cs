using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawSupportInfo
    {
        [JsonProperty("api_support_hourai")]
        public RawSupportShelling Shelling { get; set; }

        public class RawSupportShelling
        {
            [JsonProperty("api_deck_id")]
            public int FleetID { get; set; }

            [JsonProperty("api_damage")]
            [JsonConverter(typeof(BattleArrayConverter))]
            public int[] Damage { get; set; }
        }
    }
}
