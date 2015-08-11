using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_combined_battle/battle")]
    [Api("api_req_combined_battle/airbattle")]
    class DayCombinedFleetCTFBattleParser : DayCombinedFleetBattleParserBase
    {
        protected override void ProcessCore(RawBattle rpData)
        {
            ProcessCombinedAerial(rpData.AerialCombat);
            ProcessCombinedAerial(rpData.AerialCombatSecondRound);
            ProcessSupportAttack(rpData.SupportInfo);
            ProcessCombinedFleetTorpedoSalvo(rpData.OpeningTorpedoSalvo);

            ProcessCombinedFleetShelling(rpData.ShellingFirstRound);
            ProcessShelling(rpData.ShellingSecondRound);
            ProcessShelling(rpData.ShellingThirdRound);

            ProcessCombinedFleetTorpedoSalvo(rpData.ClosingTorpedoSalvo);
        }
    }
}
