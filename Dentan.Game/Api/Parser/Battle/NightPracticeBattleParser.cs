using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.QuestData;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_practice/midnight_battle")]
    class NightPracticeBattleParser : NightNormalBattleParser
    {
        protected override void ProcessCore(RawNightBattle rpData)
        {
            ProcessShelling(rpData.Shelling);
        }

        protected override void ProcessQuest(BattleData rpData)
        {
            foreach (var rProgress in Quest.Progresses.Values.OfType<PracticeProgress>())
                rProgress.Process(rpData);
        }
    }
}
