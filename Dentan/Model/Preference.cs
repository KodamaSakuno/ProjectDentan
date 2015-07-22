using Newtonsoft.Json;
using System.IO;

namespace Moen.KanColle.Dentan.Model
{
    public partial class Preference
    {
        const string r_FilePath = @"Preference\Preference.json";

        public static Preference Current { get; private set; }

        static JsonSerializer r_Serializer;

        static Preference()
        {
            r_Serializer = new JsonSerializer() { Formatting = Formatting.Indented };
        }

        public static void Load()
        {
            try
            {
                using (var rReader = new StreamReader(r_FilePath))
                using (var rJsonReader = new JsonTextReader(rReader))
                    Current = r_Serializer.Deserialize<Preference>(rJsonReader);
            }
            catch
            {
                Current = new Preference();
            }
        }
    }
}
