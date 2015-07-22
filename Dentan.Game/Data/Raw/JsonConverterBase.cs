using Newtonsoft.Json;
using System;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public abstract class JsonConverterBase : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

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
