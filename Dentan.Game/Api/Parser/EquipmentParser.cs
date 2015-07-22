using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_get_member/slot_item")]
    class EquipmentParser : ApiParser<RawEquipment[]>
    {
        public override void Process(RawEquipment[] rpData)
        {
            ApiStartParser.SuccessEvent.Wait();

            Game.UpdateEquipments(rpData);
        }
    }
}
