using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data
{
    public class AbyssalFleet : IID
    {
        public int ID { get { return Map * 100 + Cell; } }

        [JsonProperty("map")]
        public int Map { get; set; }
        [JsonProperty("cell")]
        public int Cell { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("ships")]
        //public int[] ShipIDs { get; set; }

        //[JsonProperty("formation")]
        //public Formation Formation { get; set; }

        [JsonIgnore]
        public bool IsDirty { get; internal set; }
    }
}
