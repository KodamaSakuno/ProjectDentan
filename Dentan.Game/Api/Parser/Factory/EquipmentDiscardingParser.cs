using Moen.KanColle.Dentan.Data;
using System;

namespace Moen.KanColle.Dentan.Api.Parser.Factory
{
    [Api("api_req_kousyou/destroyitem2")]
    class EquipmentDiscardingParser : ApiParser
    {
        public override void Process()
        {
            var rEquipmentIDs = Array.ConvertAll(Request["api_slotitem_ids"].Split(','), int.Parse);

            foreach (var rID in rEquipmentIDs)
                Game.Equipments.Remove(rID);

            Game.UpdateEquipments();

            Quest.Progresses[613].Current++;
        }
    }
}
