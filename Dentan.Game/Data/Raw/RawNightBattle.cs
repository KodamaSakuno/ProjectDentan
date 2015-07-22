using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawNightBattle : RawBattleBase
    {
        [JsonProperty("api_eKyouka")]
        public int[][] ApiEKyouka { get; set; }

        [JsonProperty("api_fParam")]
        public int[][] ApiFParam { get; set; }

        [JsonProperty("api_eParam")]
        public int[][] ApiEParam { get; set; }

        [JsonProperty("api_touch_plane")]
        public int[] ApiTouchPlane { get; set; }

        [JsonProperty("api_flare_pos")]
        public int[] ApiFlarePos { get; set; }

        [JsonProperty("api_hougeki")]
        public RawShelling Shelling { get; set; }

    }
}
