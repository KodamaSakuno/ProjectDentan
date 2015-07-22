using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_get_member/basic")]
    class BasicParser : ApiParser<RawBasic>
    {
        public override void Process(RawBasic rpData)
        {
            RecordManager.Instance.Load(rpData.ID);

            Game.Headquarter.UpdateAdmiral(rpData);
        }
    }
}
