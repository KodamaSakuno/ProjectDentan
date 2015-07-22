using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    [Api("api_req_combined_battle/battle_water")]
    class DayCombinedFleetSTFBattleParser : DayCombinedFleetBattleParserBase
    {
        protected override void ProcessCore(RawBattle rpData)
        {
            ProcessCombinedAerial(rpData.AerialCombat);
            ProcessSupportAttack(rpData.SupportInfo);
            ProcessCombinedFleetTorpedoSalvo(rpData.OpeningTorpedoSalvo);

            ProcessShelling(rpData.ShellingFirstRound);
            ProcessShelling(rpData.ShellingSecondRound);
            ProcessCombinedFleetShelling(rpData.ShellingThirdRound);

            ProcessCombinedFleetTorpedoSalvo(rpData.ClosingTorpedoSalvo);
        }
    }
}
