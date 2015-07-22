using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_get_member/material")]
    class MaterialParser : ApiParser<RawMaterial[]>
    {
        public override void Process(RawMaterial[] rpData)
        {
            Game.Headquarter.Material.Update(rpData);
        }
    }
}
