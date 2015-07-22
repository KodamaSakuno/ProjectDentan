using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawBasic
    {

        [JsonProperty("api_member_id")]
        public int ID { get; set; }

        [JsonProperty("api_nickname")]
        public string Nickname { get; set; }

        [JsonProperty("api_active_flag")]
        public int ApiActiveFlag { get; set; }

        [JsonProperty("api_starttime")]
        public long ApiStarttime { get; set; }

        [JsonProperty("api_level")]
        public int Level { get; set; }

        [JsonProperty("api_rank")]
        public int Rank { get; set; }

        [JsonProperty("api_experience")]
        public int Experience { get; set; }

        [JsonProperty("api_fleetname")]
        public object ApiFleetname { get; set; }

        [JsonProperty("api_comment")]
        public string Comment { get; set; }

        [JsonProperty("api_max_chara")]
        public int MaxShipCount { get; set; }

        [JsonProperty("api_max_slotitem")]
        public int MaxEquipmentCount { get; set; }

        [JsonProperty("api_max_kagu")]
        public int ApiMaxKagu { get; set; }

        [JsonProperty("api_playtime")]
        public int ApiPlaytime { get; set; }

        [JsonProperty("api_tutorial")]
        public int ApiTutorial { get; set; }

        [JsonProperty("api_furniture")]
        public int[] ApiFurniture { get; set; }

        [JsonProperty("api_count_deck")]
        public int ApiCountDeck { get; set; }

        [JsonProperty("api_count_kdock")]
        public int ApiCountKdock { get; set; }

        [JsonProperty("api_count_ndock")]
        public int ApiCountNdock { get; set; }

        [JsonProperty("api_fcoin")]
        public int ApiFcoin { get; set; }

        [JsonProperty("api_st_win")]
        public int ApiStWin { get; set; }

        [JsonProperty("api_st_lose")]
        public int ApiStLose { get; set; }

        [JsonProperty("api_ms_count")]
        public int ApiMsCount { get; set; }

        [JsonProperty("api_ms_success")]
        public int ApiMsSuccess { get; set; }

        [JsonProperty("api_pt_win")]
        public int ApiPtWin { get; set; }

        [JsonProperty("api_pt_lose")]
        public int ApiPtLose { get; set; }

        [JsonProperty("api_pt_challenged")]
        public int ApiPtChallenged { get; set; }

        [JsonProperty("api_pt_challenged_win")]
        public int ApiPtChallengedWin { get; set; }

        [JsonProperty("api_firstflag")]
        public int ApiFirstflag { get; set; }

        [JsonProperty("api_tutorial_progress")]
        public int ApiTutorialProgress { get; set; }

        [JsonProperty("api_pvp")]
        public int[] ApiPvp { get; set; }

        [JsonProperty("api_medals")]
        public int Medals { get; set; }

        [JsonProperty("api_large_dock")]
        public int ApiLargeDock { get; set; }
    }
}
