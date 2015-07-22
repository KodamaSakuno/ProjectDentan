using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawShip : IID
    {
        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_sortno")]
        public int SortNo { get; set; }

        [JsonProperty("api_ship_id")]
        public int ShipID { get; set; }

        [JsonProperty("api_lv")]
        public int Level { get; set; }

        [JsonProperty("api_exp")]
        public int[] Experience { get; set; }

        [JsonProperty("api_nowhp")]
        public int NowHP { get; set; }

        [JsonProperty("api_maxhp")]
        public int MaxHP { get; set; }

        [JsonProperty("api_leng")]
        public int ApiLeng { get; set; }

        [JsonProperty("api_slot")]
        public int[] Equipments { get; set; }

        [JsonProperty("api_onslot")]
        public int[] PlaneCount { get; set; }

        [JsonProperty("api_kyouka")]
        public int[] ApiKyouka { get; set; }

        [JsonProperty("api_backs")]
        public int ApiBacks { get; set; }

        [JsonProperty("api_fuel")]
        public int Fuel { get; set; }

        [JsonProperty("api_bull")]
        public int Bullet { get; set; }

        [JsonProperty("api_slotnum")]
        public int EquipmentCount { get; set; }

        [JsonProperty("api_ndock_time")]
        public long ApiNdockTime { get; set; }

        [JsonProperty("api_ndock_item")]
        public int[] ApiNdockItem { get; set; }

        [JsonProperty("api_srate")]
        public int ApiSrate { get; set; }

        [JsonProperty("api_cond")]
        public int Condition { get; set; }

        [JsonProperty("api_karyoku")]
        public int[] ApiKaryoku { get; set; }

        [JsonProperty("api_raisou")]
        public int[] ApiRaisou { get; set; }

        [JsonProperty("api_taiku")]
        public int[] ApiTaiku { get; set; }

        [JsonProperty("api_soukou")]
        public int[] ApiSoukou { get; set; }

        [JsonProperty("api_kaihi")]
        public int[] ApiKaihi { get; set; }

        [JsonProperty("api_taisen")]
        public int[] ApiTaisen { get; set; }

        [JsonProperty("api_sakuteki")]
        public int[] LoS { get; set; }

        [JsonProperty("api_lucky")]
        public int[] ApiLucky { get; set; }

        [JsonProperty("api_locked")]
        public int IsLocked { get; set; }

        [JsonProperty("api_locked_equip")]
        public int HasLockedEquipment { get; set; }
    }
}
