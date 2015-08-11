﻿using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawPort
    {
        [JsonProperty("api_material")]
        public RawMaterial[] Materials { get; set; }

        [JsonProperty("api_deck_port")]
        public RawFleet[] Fleets { get; set; }

        [JsonProperty("api_ndock")]
        public RawRepairDock[] RepairDocks { get; set; }

        [JsonProperty("api_ship")]
        public RawShip[] Ships { get; set; }

        [JsonProperty("api_basic")]
        public RawBasic Basic { get; set; }

        [JsonProperty("api_combined_flag")]
        public CombinedFleetFlag CombinedFleetFlag { get; set; }

        [JsonProperty("api_p_bgm_id")]
        public int BgmID { get; set; }
    }
}
