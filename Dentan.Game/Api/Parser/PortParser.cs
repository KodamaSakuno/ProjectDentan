using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_port/port")]
    class PortParser : ApiParser<RawPort>
    {
        public override void Process(RawPort rpData)
        {
            ApiStartParser.SuccessEvent.Wait();

            Game.Battle = null;
            Game.CompassData = null;
            Game.SortieFleet = null;
            Game.DroppedShip = 0;

            Game.Headquarter.UpdateAdmiral(rpData.Basic);

            var rMaterial = Game.Headquarter.Material;
            rMaterial.Update(rpData.Materials);
            if (rMaterial.IsDirty)
            {
                RecordManager.Instance.Resource.Update(rMaterial);
                rMaterial.IsDirty = false;
            }

            Game.UpdateShips(rpData.Ships);
            Game.UpdateFleets(rpData.Fleets);
            Game.UpdateRepairDocks(rpData.RepairDocks);
        }
    }
}
