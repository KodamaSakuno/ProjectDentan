using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawShipInfo : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_sortno")]
        public int SortID { get; set; }

        [JsonProperty("api_name")]
        public string Name { get; set; }

        [JsonProperty("api_yomi")]
        public string Yomi { get; set; }

        [JsonProperty("api_stype")]
        public int Type { get; set; }

        [JsonProperty("api_afterlv")]
        public int RemodelAfterLevel { get; set; }

        [JsonProperty("api_aftershipid")]
        public int RemodelAfterShipID { get; set; }

        [JsonProperty("api_taik")]
        public int[] ApiTaik { get; set; }

        [JsonProperty("api_souk")]
        public int[] ApiSouk { get; set; }

        [JsonProperty("api_houg")]
        public int[] ApiHoug { get; set; }

        [JsonProperty("api_raig")]
        public int[] ApiRaig { get; set; }

        [JsonProperty("api_tyku")]
        public int[] ApiTyku { get; set; }

        [JsonProperty("api_luck")]
        public int[] ApiLuck { get; set; }

        [JsonProperty("api_soku")]
        public ShipSpeed Speed { get; set; }

        [JsonProperty("api_leng")]
        public int ApiLeng { get; set; }

        [JsonProperty("api_slot_num")]
        public int EquipmentCount { get; set; }

        [JsonProperty("api_maxeq")]
        public int[] PlaneCount { get; set; }

        [JsonProperty("api_buildtime")]
        public int ApiBuildtime { get; set; }

        [JsonProperty("api_broken")]
        public int[] ApiBroken { get; set; }

        [JsonProperty("api_powup")]
        public int[] ApiPowup { get; set; }

        [JsonProperty("api_backs")]
        public int ApiBacks { get; set; }

        [JsonProperty("api_getmes")]
        public string ApiGetmes { get; set; }

        [JsonProperty("api_afterfuel")]
        public int ApiAfterfuel { get; set; }

        [JsonProperty("api_afterbull")]
        public int ApiAfterbull { get; set; }

        [JsonProperty("api_fuel_max")]
        public int MaxFuel { get; set; }

        [JsonProperty("api_bull_max")]
        public int MaxBullet { get; set; }

        [JsonProperty("api_voicef")]
        public int ApiVoicef { get; set; }
    }
}
