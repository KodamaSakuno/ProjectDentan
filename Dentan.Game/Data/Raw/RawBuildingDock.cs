using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawBuildingDock : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_state")]
        public BuildingDockState State { get; set; }

        [JsonProperty("api_created_ship_id")]
        public int ShipID { get; set; }

        [JsonProperty("api_complete_time")]
        public long CompleteTime { get; set; }

        [JsonProperty("api_complete_time_str")]
        public string CompleteTimeStr { get; set; }

        [JsonProperty("api_item1")]
        public int Fuel { get; set; }

        [JsonProperty("api_item2")]
        public int Bullet { get; set; }

        [JsonProperty("api_item3")]
        public int Steel { get; set; }

        [JsonProperty("api_item4")]
        public int Bauxite { get; set; }

        [JsonProperty("api_item5")]
        public int DevelopmentMaterial { get; set; }
    }
}
