using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawQuest
    {
        [JsonProperty("api_no")]
        public int ID { get; set; }

        [JsonProperty("api_category")]
        public QuestCategory Category { get; set; }

        [JsonProperty("api_type")]
        public QuestType Type { get; set; }

        [JsonProperty("api_state")]
        public QuestState State { get; set; }

        [JsonProperty("api_title")]
        public string Title { get; set; }

        [JsonProperty("api_detail")]
        public string Detail { get; set; }

        [JsonProperty("api_get_material")]
        public int[] GetMaterial { get; set; }

        [JsonProperty("api_bonus_flag")]
        public int BonusFlag { get; set; }

        [JsonProperty("api_progress_flag")]
        public QuestProgress Progress { get; set; }

        [JsonProperty("api_invalid_flag")]
        public int ApiInvalidFlag { get; set; }
    }
}
