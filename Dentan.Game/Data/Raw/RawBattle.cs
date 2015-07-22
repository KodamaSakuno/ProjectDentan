using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawBattle : RawBattleBase
    {
        [JsonProperty("api_midnight_flag")]
        public bool CanNightBattle { get; set; }

        [JsonProperty("api_eKyouka")]
        public int[][] ApiEKyouka { get; set; }

        [JsonProperty("api_fParam")]
        public int[][] ApiFParam { get; set; }

        [JsonProperty("api_eParam")]
        public int[][] ApiEParam { get; set; }

        [JsonProperty("api_escape_idx")]
        public int[] EscapedShipIndex { get; set; }
        [JsonProperty("api_escape_idx_combined")]
        public int[] EscapedCombinedShipIndex { get; set; }

        [JsonProperty("api_search")]
        public int[] Detection { get; set; }

        [JsonProperty("api_stage_flag")]
        public bool[] AerialCombatFlag { get; set; }
        [JsonProperty("api_kouku")]
        public RawAerialCombat AerialCombat { get; set; }

        [JsonProperty("api_stage_flag2")]
        public bool[] AerialCombatSecondRoundFlag { get; set; }
        [JsonProperty("api_kouku2")]
        public RawAerialCombat AerialCombatSecondRound { get; set; }

        [JsonProperty("api_support_flag")]
        public int ApiSupportFlag { get; set; }

        [JsonProperty("api_support_info")]
        public RawSupportInfo SupportInfo { get; set; }

        [JsonProperty("api_opening_flag")]
        public bool OpeningTorpedoSalvoFlag { get; set; }

        [JsonProperty("api_opening_atack")]
        public RawTorpedoSalvo OpeningTorpedoSalvo { get; set; }

        [JsonProperty("api_hourai_flag")]
        public bool[] ShellingFlag { get; set; }

        [JsonProperty("api_hougeki1")]
        public RawShelling ShellingFirstRound { get; set; }

        [JsonProperty("api_hougeki2")]
        public RawShelling ShellingSecondRound { get; set; }

        [JsonProperty("api_hougeki3")]
        public RawShelling ShellingThirdRound { get; set; }

        [JsonProperty("api_raigeki")]
        public RawTorpedoSalvo ClosingTorpedoSalvo { get; set; }
    }
}
