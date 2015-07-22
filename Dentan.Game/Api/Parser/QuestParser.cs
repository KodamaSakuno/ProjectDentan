using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;
using System.Collections.Generic;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_get_member/questlist")]
    class QuestListParser : ApiParser<RawQuestList>
    {
        static IEqualityComparer<Quest> Comparer;

        static QuestListParser()
        {
            Comparer = new DelegatedEqualityComparer<Quest>((x, y) => x.ID == y.ID, r => r.ID);
        }

        public override void Process(RawQuestList rpData)
        {
            if (rpData.Quests == null)
                return;

            using (var rTransaction = RecordManager.Instance.Quest.Connection.BeginTransaction())
            {
                ILookup<bool, Quest> rQuests;
                IEnumerable<Quest> rExecutingQuests, rOtherQuests;
                IEnumerable<Quest> rCurrentPageQuests = rpData.Quests.Select(GetObject);
                if (Game.Quests == null)
                {
                    rQuests = rCurrentPageQuests.ToLookup(r => r.IsStarted);
                    rExecutingQuests = rQuests[true].OrderBy(r => r.ID);
                    rOtherQuests = rQuests[false].OrderBy(r => r.ID);
                }
                else
                {
                    rQuests = Game.Quests.Where(r => r != Quest.Default && !rpData.Quests.Any(rpQuest => rpQuest.ID == r.ID))
                                         .Union(rCurrentPageQuests).ToLookup(r => r.IsStarted);
                    rExecutingQuests = rQuests[true].OrderBy(r => r.ID);
                    rOtherQuests = rQuests[false].OrderBy(r => r.ID);
                }

                var rExecutingQuestCount = rExecutingQuests.Count();
                if (rExecutingQuestCount < rpData.ExecutingCount)
                    rExecutingQuests = rExecutingQuests.Concat(Enumerable.Repeat(Quest.Default, rpData.ExecutingCount - rExecutingQuestCount));

                Game.Quests = rExecutingQuests.Concat(rOtherQuests).ToArray();
                Game.CheckQuests();

                foreach (var rQuest in rCurrentPageQuests.Where(r => r.ProgressData != null))
                {
                    var rProgressData = rQuest.ProgressData;

                    rProgressData.ResetByDate();
                    rProgressData.ResetByProgress(rQuest.State, rQuest.Progress);

                    rProgressData.Update();
                }

                rTransaction.Commit();
            }
        }

        Quest GetObject(RawQuest rpData)
        {
            Quest rQuest;
            if (!Game.QuestTable.TryGetValue(rpData.ID, out rQuest))
                Game.QuestTable.Add(rQuest = new Quest(rpData));
            else
                rQuest.Update(rpData);

            return rQuest;
        }
    }

    [Api("api_req_quest/clearitemget")]
    class QuestClearParser : ApiParser
    {
        public override void Process()
        {
            var rQuestID = int.Parse(Request["api_quest_id"]);

            Game.QuestTable.Remove(rQuestID);
            Game.Quests = Game.Quests.Where(r => r.ID != rQuestID).ToArray();
        }
    }
}
