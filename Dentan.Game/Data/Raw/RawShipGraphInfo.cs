using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawShipGraphInfo : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_sortno")]
        public int SortID { get; set; }

        [JsonProperty("api_filename")]
        public string Filename { get; set; }

        [JsonProperty("api_version")]
        public int Version { get; set; }

        [JsonProperty("api_boko_n")]
        public int[] ApiBokoN { get; set; }

        [JsonProperty("api_boko_d")]
        public int[] ApiBokoD { get; set; }

        [JsonProperty("api_kaisyu_n")]
        public int[] ApiKaisyuN { get; set; }

        [JsonProperty("api_kaisyu_d")]
        public int[] ApiKaisyuD { get; set; }

        [JsonProperty("api_kaizo_n")]
        public int[] ApiKaizoN { get; set; }

        [JsonProperty("api_kaizo_d")]
        public int[] ApiKaizoD { get; set; }

        [JsonProperty("api_map_n")]
        public int[] ApiMapN { get; set; }

        [JsonProperty("api_map_d")]
        public int[] ApiMapD { get; set; }

        [JsonProperty("api_ensyuf_n")]
        public int[] ApiEnsyufN { get; set; }

        [JsonProperty("api_ensyuf_d")]
        public int[] ApiEnsyufD { get; set; }

        [JsonProperty("api_ensyue_n")]
        public int[] ApiEnsyueN { get; set; }

        [JsonProperty("api_battle_n")]
        public int[] ApiBattleN { get; set; }

        [JsonProperty("api_battle_d")]
        public int[] ApiBattleD { get; set; }

        [JsonProperty("api_weda")]
        public int[] ApiWeda { get; set; }

        [JsonProperty("api_wedb")]
        public int[] ApiWedb { get; set; }
    }
}
