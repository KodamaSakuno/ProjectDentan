using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public abstract class RawBattleBase
    {
        [JsonProperty("api_deck_id")]
        public string FleetIDs { get; set; }

        [JsonProperty("api_nowhps")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] NowHPs { get; set; }
        [JsonProperty("api_maxhps")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] MaxHPs { get; set; }
        [JsonProperty("api_nowhps_combined")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] NowHPsCombined { get; set; }

        [JsonProperty("api_maxhps_combined")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] MaxHPsCombined { get; set; }

        [JsonProperty("api_ship_ke")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] EnemyIDs { get; set; }
        [JsonProperty("api_ship_lv")]
        [JsonConverter(typeof(BattleArrayConverter))]
        public int[] EnemyLevels { get; set; }

        [JsonProperty("api_formation")]
        [JsonConverter(typeof(RawFormationsConverter))]
        public RawFormations Formation { get; set; }

        [JsonProperty("api_eSlot")]
        public int[][] EnemyEquipments { get; set; }

        public class RawFormations
        {
            public Formation Friend { get; set; }
            public Formation Enemy { get; set; }
            public EngagementForm EngagementForm { get; set; }
        }
        public class RawFormationsConverter : JsonConverterBase
        {
            public override object ReadJson(JsonReader rpReader, Type rpObjectType, object rpExistingValue, JsonSerializer rpSerializer)
            {
                var rJson = JArray.Load(rpReader);
                return new RawFormations()
                {
                    Friend = (Formation)(int)rJson[0],
                    Enemy = (Formation)(int)rJson[1],
                    EngagementForm = (EngagementForm)(int)rJson[2],
                };
            }
        }
    }
}
