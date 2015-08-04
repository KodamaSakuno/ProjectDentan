using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Model.Game
{
    class ImprovementArsenalModel : IID
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("detail")]
        public ImprovementDetail[] Detail { get; set; }

        public class ImprovementDetail
        {
            [JsonProperty("days")]
            public int Days { get; set; }

            [JsonProperty("update")]
            public int? UpdateTo { get; set; }

            [JsonProperty("assistant")]
            public int[] Assistants { get; set; }
        }
    }
}
