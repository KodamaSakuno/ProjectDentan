using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Browser
{
    public class LayoutEngine
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayname")]
        public string DisplayName { get; set; }

        [JsonProperty("entry")]
        public string EntryFile { get; set; }
    }
}
