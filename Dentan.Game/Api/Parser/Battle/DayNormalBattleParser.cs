using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_sortie/battle")]
    [Api("api_req_sortie/airbattle")]
    class DayNormalBattleParser : DayBattleParserBase
    {
        protected override void ProcessCore(RawBattle rpData)
        {
            ProcessAerial(rpData.AerialCombat);
            ProcessAerial(rpData.AerialCombatSecondRound);
            ProcessSupportAttack(rpData.SupportInfo);
            ProcessTorpedoSalvo(rpData.OpeningTorpedoSalvo);

            ProcessShelling(rpData.ShellingFirstRound);
            ProcessShelling(rpData.ShellingSecondRound);

            ProcessTorpedoSalvo(rpData.ClosingTorpedoSalvo);
        }

        protected override void PostProcess(RawBattle rpData)
        {
            PostProcessCore(rpData);
        }
    }
}
