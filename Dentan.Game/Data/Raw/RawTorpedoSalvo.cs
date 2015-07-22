using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawTorpedoSalvo
    {
        [JsonProperty("api_frai")]
        public int[] ApiFrai { get; set; }

        [JsonProperty("api_erai")]
        public int[] ApiErai { get; set; }

        [JsonProperty("api_fdam")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] FriendDamage { get; set; }

        [JsonProperty("api_edam")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] EnemyDamage { get; set; }

        [JsonProperty("api_fydam")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] AlliedGivenDamage { get; set; }

        [JsonProperty("api_eydam")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] EnemyGivenDamage { get; set; }

        [JsonProperty("api_fcl")]
        public int[] ApiFcl { get; set; }

        [JsonProperty("api_ecl")]
        public int[] ApiEcl { get; set; }

    }
}
