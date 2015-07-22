using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawBuildingGetShip
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_ship_id")]
        public int ShipID { get; set; }

        [JsonProperty("api_kdock")]
        public RawBuildingDock[] BuildingDocks { get; set; }

        [JsonProperty("api_ship")]
        public RawShip Ship { get; set; }

        [JsonProperty("api_slotitem")]
        public RawEquipment[] Equipments { get; set; }
    }
}
