using Moen.KanColle.Dentan.Data.Raw;
using System.Collections.Generic;
using System.Linq;

namespace Moen.KanColle.Dentan.Data.QuestData
{
    abstract class ExpeditionProgress : QuestProgressData
    {
        public HashSet<int> ExpeditionID { get; private set; }

        protected ExpeditionProgress() { }
        protected ExpeditionProgress(int[] rpExpeditionID)
        {
            ExpeditionID = new HashSet<int>(rpExpeditionID);
        }

        public void Process(RawExpeditionResult rpData)
        {
            var rExpeditionID = KanColleGame.Current.Base.Expeditions.Values.Single(r => r.Name == rpData.Name).ID;

            if (rpData.Result != ExpeditionResult.Failure && (ExpeditionID == null || ExpeditionID.Contains(rExpeditionID)))
                Current++;
        }
    }

    [Quest(402, QuestType.Daily, 3)]
    class ExpeditionDaily3Progress : ExpeditionProgress
    {
    }
    [Quest(403, QuestType.Daily, 10)]
    class ExpeditionDaily10Progress : ExpeditionProgress
    {
    }
    [Quest(404, QuestType.Weekly, 30)]
    class ExpeditionWeeklyProgress : ExpeditionProgress
    {
    }

    [Quest(410, QuestType.Weekly, 1)]
    class ExpeditionSouthernPart1Progress : ExpeditionProgress
    {
        public ExpeditionSouthernPart1Progress()
            : base(new[] { 37, 38 }) { }
    }
    [Quest(411, QuestType.Weekly, 7, StartFrom = 1)]
    class ExpeditionSouthernPart2Progress : ExpeditionProgress
    {
        public ExpeditionSouthernPart2Progress()
            : base(new[] { 37, 38 }) { }
    }

    [Quest(418, QuestType.Once, 2)]
    class Expedition7TwiceProgress : ExpeditionProgress
    {
        public Expedition7TwiceProgress()
            : base(new[] { 7 }) { }
    }
}
