using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.QuestData;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    abstract class NightBattleParserBase : BattleParserBase<RawNightBattle>
    {
        protected override void ProcessCore(RawNightBattle rpData)
        {
            ProcessShelling(rpData.Shelling);
        }

        protected override void ProcessQuest(BattleData rpData)
        {
            foreach (var rProgress in Quest.Progresses.Values.OfType<DestroyTargetProgress>())
                rProgress.ProcessNight(rpData);
            foreach (var rProgress in Quest.Progresses.Values.OfType<BossTargetProgress>())
                rProgress.Process(rpData);
            foreach (var rProgress in Quest.Progresses.Values.OfType<DailyBattleProgress>())
                rProgress.Current++;
            ((CodeAProgress)Quest.Progresses[214]).Process(rpData);
        }
    }
}
