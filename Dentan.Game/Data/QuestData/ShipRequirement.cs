using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Moen.KanColle.Dentan.Data.QuestData
{
    static class ShipRequirement
    {
        internal static Dictionary<int, int[]> Table { get; private set; }

        static ShipRequirement()
        {
            if (!File.Exists(@"Data\QuestShipRequirement.json"))
                Table = new Dictionary<int, int[]>();
            else
                using (var rReader = File.OpenText(@"Data\QuestShipRequirement.json"))
                    Table = JArray.Load(new JsonTextReader(rReader)).ToDictionary(r => (int)r["id"], r => r["ships"].ToObject<int[]>());
        }

        public static void Check(Quest rpQuest)
        {
            rpQuest.CanCompleted = Check(rpQuest.ID);
        }
        public static bool Check(int rpID)
        {
            int[] rShips;
            if (Table.TryGetValue(rpID, out rShips))
            {
                if (rpID != 152 && rpID != 273)
                    return rShips.Select(r => KanColleGame.Current.Base.ShipIDsGroupByModel[r]).All(r => KanColleGame.Current.ShipIDs.Intersect(r).Any());
                else
                    return KanColleGame.Current.ShipIDs.Intersect(KanColleGame.Current.Base.ShipIDsGroupByModel[rShips[0]]).Any()
                        && rShips.Skip(1).Select(r => KanColleGame.Current.Base.ShipIDsGroupByModel[r]).All(r => KanColleGame.Current.ShipIDs.Intersect(r).Any());
            }
            return false;
        }
    }
}
