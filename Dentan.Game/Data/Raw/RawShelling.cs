using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawShelling
    {
        [JsonProperty("api_at_list")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] ActionOrder { get; set; }

        [JsonProperty("api_at_type")]
        public int[] ActionType { get; set; }

        [JsonProperty("api_df_list")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[][] Defense { get; set; }

        [JsonProperty("api_si_list")]
        public object[] ApiSiList { get; set; }

        [JsonProperty("api_cl_list")]
        public object[] ApiClList { get; set; }

        [JsonProperty("api_damage")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[][] Damage { get; set; }
    }
}
