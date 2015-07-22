using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawRepairDock : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_state")]
        public RepairDockState State { get; set; }

        [JsonProperty("api_ship_id")]
        public int ShipID { get; set; }

        [JsonProperty("api_complete_time")]
        public long CompleteTime { get; set; }

        [JsonProperty("api_item1")]
        public int Fuel { get; set; }

        [JsonProperty("api_item2")]
        public int Bullet { get; set; }

        [JsonProperty("api_item3")]
        public int Steel { get; set; }

        [JsonProperty("api_item4")]
        public int Bauxite { get; set; }
    }
}
