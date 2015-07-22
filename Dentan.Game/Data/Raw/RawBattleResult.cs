using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawBattleResult
    {
        [JsonProperty("api_ship_id")]
        public int[] ShipIDs { get; set; }

        [JsonProperty("api_win_rank")]
        public string Rank { get; set; }

        [JsonProperty("api_get_exp")]
        public int AdmiralGetExperience { get; set; }

        [JsonProperty("api_mvp")]
        public int ApiMvp { get; set; }

        [JsonProperty("api_member_lv")]
        public int HeadquarterLevel { get; set; }

        [JsonProperty("api_member_exp")]
        public int AdmiralExperience { get; set; }

        [JsonProperty("api_get_base_exp")]
        public int BaseExperience { get; set; }

        [JsonProperty("api_get_ship_exp")]
        public int[] GetExperience { get; set; }

        [JsonProperty("api_get_exp_lvup")]
        public int[][] ApiGetExpLvup { get; set; }

        [JsonProperty("api_dests")]
        public int ApiDests { get; set; }

        [JsonProperty("api_destsf")]
        public int ApiDestsf { get; set; }

        [JsonProperty("api_lost_flag")]
        public int[] ApiLostFlag { get; set; }

        [JsonProperty("api_quest_name")]
        public string QuestName { get; set; }

        [JsonProperty("api_quest_level")]
        public int QuestLevel { get; set; }

        [JsonProperty("api_enemy_info")]
        public RawEnemyInfo EnemyInfo { get; set; }

        [JsonProperty("api_first_clear")]
        public bool IsFirstClear { get; set; }

        [JsonProperty("api_get_flag")]
        public bool[] Drop { get; set; }

        [JsonProperty("api_get_ship")]
        public RawDropShip DropShip { get; set; }

        [JsonProperty("api_get_eventflag")]
        public int ApiGetEventflag { get; set; }

        [JsonProperty("api_get_exmap_rate")]
        public int ApiGetExmapRate { get; set; }

        [JsonProperty("api_get_exmap_useitem_id")]
        public int ApiGetExmapUseitemId { get; set; }

        public class RawEnemyInfo
        {
            [JsonProperty("api_level")]
            public string Level { get; set; }

            [JsonProperty("api_rank")]
            public string Rank { get; set; }

            [JsonProperty("api_deck_name")]
            public string FleetName { get; set; }
        }
        public class RawDropShip
        {
            [JsonProperty("api_ship_id")]
            public int ID { get; set; }

            [JsonProperty("api_ship_type")]
            public string Type { get; set; }

            [JsonProperty("api_ship_name")]
            public string Name { get; set; }
        }
    }
}
