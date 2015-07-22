using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawApiStart
    {
        [JsonProperty("api_mst_ship")]
        public RawShipInfo[] Ships { get; set; }

        [JsonProperty("api_mst_shipgraph")]
        public RawShipGraphInfo[] ShipGraphes { get; set; }

        [JsonProperty("api_mst_slotitem_equiptype")]
        public RawEquipmentType[] EquipmentTypes { get; set; }

        [JsonProperty("api_mst_stype")]
        public RawShipType[] ShipTypes { get; set; }

        [JsonProperty("api_mst_slotitem")]
        public RawEquipmentInfo[] Equipments { get; set; }

        [JsonProperty("api_mst_slotitemgraph")]
        public RawEquipmentGraph[] EquipmentGraphes { get; set; }

        //[JsonProperty("api_mst_furniture")]
        //public RawFurnitureInfo[] Furnitures { get; set; }

        //[JsonProperty("api_mst_furnituregraph")]
        //public RawFurnitureGraphInfo[] FurnitureGraphs { get; set; }

        [JsonProperty("api_mst_useitem")]
        public RawUseItemInfo[] UseItems { get; set; }

        /*
        [JsonProperty("api_mst_payitem")]
        public RawPayItem[] PayItems { get; set; }

        [JsonProperty("api_mst_item_shop")]
        public RawItemShop ItemShop { get; set; }
        //*/

        [JsonProperty("api_mst_maparea")]
        public RawMapArea[] MapAreas { get; set; }

        [JsonProperty("api_mst_mapinfo")]
        public RawBaseMapInfo[] MapInfos { get; set; }

        [JsonProperty("api_mst_mapbgm")]
        public RawMapBgm[] ApiMstMapbgm { get; set; }

        [JsonProperty("api_mst_mapcell")]
        public RawMapCell[] ApiMstMapcell { get; set; }

        [JsonProperty("api_mst_mission")]
        public RawExpeditionInfo[] Expeditions { get; set; }

        /*
        [JsonProperty("api_mst_const")]
        public RawConstants Constants { get; set; }

        [JsonProperty("api_mst_shipupgrade")]
        public RawShipUpgradeInfo[] ShipUpgradeInfo { get; set; }

        [JsonProperty("api_mst_bgm")]
        public RawMainBGM[] MainBGM { get; set; }
        //*/
    }
}
