using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawExpeditionInfo : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_maparea_id")]
        public int MapAreaID { get; set; }

        [JsonProperty("api_name")]
        public string Name { get; set; }

        [JsonProperty("api_details")]
        public string Description { get; set; }

        [JsonProperty("api_time")]
        public int Time { get; set; }

        [JsonProperty("api_difficulty")]
        public int Difficulty { get; set; }

        [JsonProperty("api_use_fuel")]
        public double UseFuel { get; set; }

        [JsonProperty("api_use_bull")]
        public double UseBullet { get; set; }

        [JsonProperty("api_win_item1")]
        public int[] ApiWinItem1 { get; set; }

        [JsonProperty("api_win_item2")]
        public int[] ApiWinItem2 { get; set; }

        [JsonProperty("api_return_flag")]
        public int ReturnFlag { get; set; }
    }

}
