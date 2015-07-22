using Moen.KanColle.Dentan.Data;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_req_kousyou/remodel_slot")]
    class EquipmentImprovementParser : ApiParser
    {
        public override void Process()
        {
            Quest.Progresses[618].Current++;
            Quest.Progresses[619].Current++;
        }
    }
}
