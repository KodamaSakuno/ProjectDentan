using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawQuestList
    {
        [JsonProperty("api_count")]
        public int Count { get; set; }

        [JsonProperty("api_page_count")]
        public int PageCount { get; set; }

        [JsonProperty("api_disp_page")]
        public int CurrentPage { get; set; }

        [JsonProperty("api_list")]
        [JsonConverter(typeof(QuestListConverter))]
        public RawQuest[] Quests { get; set; }

        [JsonProperty("api_exec_count")]
        public int ExecutingCount { get; set; }

        [JsonProperty("api_exec_type")]
        public int ExecutingType { get; set; }

        class QuestListConverter : JsonConverter
        {
            public override bool CanWrite { get { return false; } }

            public override object ReadJson(JsonReader rpReader, Type rpObjectType, object rpExistingValue, JsonSerializer rpSerializer)
            {
                try
                {
                    var rList = JArray.Load(rpReader);
                    return rList.TakeWhile(r => r.Type == JTokenType.Object).Select(r => r.ToObject<RawQuest>()).ToArray();
                }
                catch (JsonReaderException)
                {
                    return null;
                }
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotSupportedException();
            }
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }
        }
    }
}
