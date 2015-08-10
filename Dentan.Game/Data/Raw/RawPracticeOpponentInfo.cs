using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawPracticeOpponentInfo
    {
        [JsonProperty("api_member_id")]
        public int ID { get; set; }

        [JsonProperty("api_nickname")]
        public string Name { get; set; }

        [JsonProperty("api_cmt")]
        public string Comment { get; set; }

        [JsonProperty("api_level")]
        public int Level { get; set; }

        [JsonProperty("api_rank")]
        public int Rank { get; set; }

        [JsonProperty("api_experience")]
        public int[] Experience { get; set; }

        [JsonProperty("api_friend")]
        public int FleetCount { get; set; }

        [JsonProperty("api_ship")]
        public int[] Ship { get; set; }

        [JsonProperty("api_slotitem")]
        public int[] ApiSlotitem { get; set; }

        [JsonProperty("api_furniture")]
        public int FurnitureCount { get; set; }

        [JsonProperty("api_deckname")]
        public string FleetName { get; set; }

        [JsonProperty("api_deck")]
        public RawFleet Fleet { get; set; }

        public class RawFleet
        {
            [JsonProperty("api_ships")]
            public RawShip[] Ships { get; set; }
        }
        public class RawShip
        {

            [JsonProperty("api_id")]
            public int ID { get; set; }

            [JsonProperty("api_ship_id")]
            public int ShipID { get; set; }

            [JsonProperty("api_level")]
            public int Level { get; set; }

            [JsonProperty("api_star")]
            public int Star { get; set; }
        }
    }
}
