using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawMidnightBattle
    {
        [JsonProperty("api_deck_id")]
        public string FleetID { get; set; }

        [JsonProperty("api_nowhps")]
        public int[] NowHP { get; set; }

        [JsonProperty("api_ship_ke")]
        public int[] EnemyID { get; set; }

        [JsonProperty("api_ship_lv")]
        public int[] EnemyLevel { get; set; }

        [JsonProperty("api_maxhps")]
        public int[] MaxHP { get; set; }

        [JsonProperty("api_eSlot")]
        public int[][] ApiESlot { get; set; }

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

        //[JsonProperty("api_hougeki")]
        //public ApiHougeki ApiHougeki { get; set; }
    }
}
