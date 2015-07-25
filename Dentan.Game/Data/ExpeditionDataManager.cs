using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    public static class ExpeditionDataManager
    {
        const string DataFile = @"Data\ExpeditionData.json";

        public static Table<ExpeditionData> Data { get; private set; }

        public static void Load()
        {
            if (!File.Exists(DataFile))
                Data = new Table<ExpeditionData>();
            else
                using (var rReader = File.OpenText(DataFile))
                    Data = new Table<ExpeditionData>(JArray.Load(new JsonTextReader(rReader)).ToObject<ExpeditionData[]>().ToDictionary(r => r.ID));
        }
    }
}
