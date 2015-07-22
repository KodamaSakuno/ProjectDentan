using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class BattleArrayConverter : JsonConverterBase
    {
        public override object ReadJson(JsonReader rpReader, Type rpObjectType, object rpExistingValue, JsonSerializer rpSerializer)
        {
            if (rpObjectType == typeof(int[][]))
                return JArray.Load(rpReader).Skip(1).Select(rpToken => rpToken.ToObject<int[]>()).ToArray();
            return JArray.Load(rpReader).Skip(1).Select(rpToken =>
                {
                    if (rpToken.Type == JTokenType.Array)
                        return rpToken.Max(r => (int)r);
                    else
                        return (int)rpToken;
                }).ToArray();
        }
    }
}
