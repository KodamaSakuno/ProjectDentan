using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawAerialCombat
    {
        [JsonProperty("api_plane_from")]
        public int[][] PlanesFrom { get; set; }

        [JsonProperty("api_stage1")]
        public RawFighterCombat FighterCombat { get; set; }

        [JsonProperty("api_stage2")]
        public RawFleetAntiAirDefense FleetAntiAirDefense { get; set; }

        [JsonProperty("api_stage3")]
        public RawBombing Bombing { get; set; }
        [JsonProperty("api_stage3_combined")]
        public RawBombing BombingCombined { get; set; }

        public class RawFighterCombat 
        {
            [JsonProperty("api_f_count")]
            public int FriendPlaneCount { get; set; }

            [JsonProperty("api_f_lostcount")]
            public int FriendPlaneLost { get; set; }

            [JsonProperty("api_e_count")]
            public int EnemyPlaneCount { get; set; }

            [JsonProperty("api_e_lostcount")]
            public int EnemyPlaneLost { get; set; }

            [JsonProperty("api_disp_seiku")]
            public AerialCombatResult Result { get; set; }

            [JsonProperty("api_touch_plane")]
            public bool[] ContactPlane { get; set; }
        }
        public class RawFleetAntiAirDefense
        {
            [JsonProperty("api_f_count")]
            public int FriendPlaneCount { get; set; }

            [JsonProperty("api_f_lostcount")]
            public int FriendPlaneLost { get; set; }

            [JsonProperty("api_e_count")]
            public int EnemyPlaneCount { get; set; }

            [JsonProperty("api_e_lostcount")]
            public int EnemyPlaneLost { get; set; }
            /*
            [JsonProperty("api_air_fire")]
            public ApiAirFire ApiAirFire { get; set; }
            //*/
        }
        public class RawBombing
        {

            [JsonProperty("api_frai_flag")]
            [JsonConverter(typeof(BattleArrayConverter))]
            public int[] ApiFraiFlag { get; set; }

            [JsonProperty("api_erai_flag")]
            [JsonConverter(typeof(BattleArrayConverter))]
            public int[] ApiEraiFlag { get; set; }

            [JsonProperty("api_fbak_flag")]
            [JsonConverter(typeof(BattleArrayConverter))]
            public int[] ApiFbakFlag { get; set; }

            [JsonProperty("api_ebak_flag")]
            [JsonConverter(typeof(BattleArrayConverter))]
            public int[] ApiEbakFlag { get; set; }

            [JsonProperty("api_fcl_flag")]
            [JsonConverter(typeof(BattleArrayConverter))]
            public int[] ApiFclFlag { get; set; }

            [JsonProperty("api_ecl_flag")]
            [JsonConverter(typeof(BattleArrayConverter))]
            public int[] ApiEclFlag { get; set; }

            [JsonProperty("api_fdam")]
            [JsonConverter(typeof(BattleArrayConverter))]
            public int[] AlliedDamage { get; set; }

            [JsonProperty("api_edam")]
            [JsonConverter(typeof(BattleArrayConverter))]
            public int[] EnemyDamage { get; set; }
        }

    }
}
