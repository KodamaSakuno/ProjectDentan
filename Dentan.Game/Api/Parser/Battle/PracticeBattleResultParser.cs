using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_practice/battle_result")]
    class PracticeBattleResultParser : ApiParser<RawBattleResult>
    {
        public override void Process(RawBattleResult rpData)
        {
            RecordManager.Instance.Battle.UpdatePractice(Game.CompassData, Game.Battle, rpData.Rank);
        }
    }
}
