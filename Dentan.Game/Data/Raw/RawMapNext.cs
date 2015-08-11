using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawMapNext
    {
        [JsonProperty("api_rashin_flg")]
        public int ApiRashinFlg { get; set; }

        [JsonProperty("api_rashin_id")]
        public int ApiRashinId { get; set; }

        [JsonProperty("api_maparea_id")]
        public int MapAreaID { get; set; }

        [JsonProperty("api_mapinfo_no")]
        public int MapInfoNo { get; set; }

        [JsonProperty("api_no")]
        public int Cell { get; set; }

        [JsonProperty("api_color_no")]
        public int ColorNo { get; set; }

        [JsonProperty("api_event_id")]
        public MapCellEvent CellEvent { get; set; }

        [JsonProperty("api_event_kind")]
        public int CellEventSubID { get; set; }

        [JsonProperty("api_next")]
        public int NextRouteCount { get; set; }

        [JsonProperty("api_bosscell_no")]
        public int BossCellNo { get; set; }

        [JsonProperty("api_bosscomp")]
        public int ApiBosscomp { get; set; }

        [JsonProperty("api_comment_kind")]
        public int CommentKind { get; set; }

        [JsonProperty("api_production_kind")]
        public int ProductionKind { get; set; }

        [JsonProperty("api_happening")]
        public WhirlpoolEvent Whirlpool { get; set; }

        //[JsonProperty("api_enemy")]
        //public RawEnemyID Enemy { get; set; }

        [JsonProperty("api_eventmap")]
        public RawEventMap EventMap { get; set; }

        [JsonProperty("api_itemget")]
        public RawItemGet ItemGet { get; set; }
        [JsonProperty("api_itemget_eo_comment")]
        public RawItemGet ItemGetEO { get; set; }

        [JsonProperty("api_airsearch")]
        public RawAviationReconnaissance AviationReconnaissance { get; set; }

        public class RawEnemyID
        {
            [JsonProperty("api_enemy_id")]
            public int ID { get; set; }
        }

        public class WhirlpoolEvent
        {
            /*
            [JsonProperty("api_type")]
            public int Type { get; set; }
            [JsonProperty("api_usemst")]
            public int ApiUsemst { get; set; }
            //*/
            [JsonProperty("api_count")]
            public int Count { get; set; }

            [JsonProperty("api_mst_id")]
            public int Type { get; set; }

            [JsonProperty("api_icon_id")]
            public int IconID { get; set; }

            [JsonProperty("api_dentan")]
            public bool HasDentan { get; set; }
        }

        public class RawEventMap
        {

            [JsonProperty("api_now_maphp")]
            public int NowHP { get; set; }

            [JsonProperty("api_max_maphp")]
            public int MaxHP { get; set; }
        }

        public class RawItemGet
        {
            [JsonProperty("api_id")]
            public int ID { get; set; }

            [JsonProperty("api_getcount")]
            public int Count { get; set; }
        }

        public class RawAviationReconnaissance
        {
            [JsonProperty("api_plane_type")]
            public AviationReconnaissancePlaneType PlaneType { get; set; }

            [JsonProperty("api_result")]
            public AviationReconnaissanceResult Result { get; set; }
        }
    }
}
