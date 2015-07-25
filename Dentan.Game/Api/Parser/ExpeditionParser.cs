using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.QuestData;
using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_req_mission/result")]
    class ExpeditionParser : ApiParser<RawExpeditionResult>
    {
        public override void Process(RawExpeditionResult rpData)
        {
            var rExpeditionID = Game.Base.Expeditions.Values.Single(r => r.Name == rpData.Name).ID;

            if (rpData.Result != ExpeditionResult.Failure)
                RecordManager.Instance.Expedition.UpdateCount(rpData.Ships.Skip(1), rExpeditionID);
            
            if (rpData.Item1 != null && rpData.GetItem[0] != 4)
                rpData.Item1.ID = rpData.GetItem[0];
            if (rpData.Item2 != null && rpData.GetItem[1] != 4)
                rpData.Item2.ID = rpData.GetItem[1];

            RecordManager.Instance.Expedition.UpdateExpedition(rExpeditionID, rpData);

            foreach (var rProgress in Quest.Progresses.Values.OfType<ExpeditionProgress>())
                rProgress.Process(rpData);
        }
    }
}
