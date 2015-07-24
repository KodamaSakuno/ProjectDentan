using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data
{
    public class ExpeditionData : IID
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("get_fuel")]
        public int GetFuel { get; set; }

        [JsonProperty("get_bullet")]
        public int GetBullet { get; set; }

        [JsonProperty("get_steel")]
        public int GetSteel { get; set; }

        [JsonProperty("get_bauxite")]
        public int GetBauxite { get; set; }

        [JsonProperty("flagship_lv")]
        public int FlagshipLevel { get; set; }

        [JsonProperty("flagship_type")]
        public int? FlagshipType { get; set; }

        [JsonProperty("total_level")]
        public int TotalLevel { get; set; }

        [JsonProperty("ship_count")]
        public int ShipCount { get; set; }

        [JsonProperty("drum")]
        public DrumInfo Drum { get; set; }

        [JsonProperty("ship_types")]
        public ShipType[] RequiredShipTypes { get; set; }

        public class DrumInfo
        {
            [JsonProperty("ship_count")]
            public int ShipCount { get; set; }
            [JsonProperty("count")]
            public int Count { get; set; }
        }
        public class ShipType
        {
            [JsonProperty("type")]
            public int[] Types { get; set; }
            [JsonProperty("count")]
            public int Count { get; set; }
        }
    }
}
