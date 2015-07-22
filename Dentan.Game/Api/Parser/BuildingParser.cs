using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_get_member/kdock")]
    class BuildingParser : ApiParser<RawBuildingDock[]>
    {
        public override void Process(RawBuildingDock[] rpData)
        {
            ApiStartParser.SuccessEvent.Wait();
            Game.UpdateBuildingDocks(rpData);
        }
    }

    [Api("api_req_kousyou/getship")]
    class BuildingGetShipParser : ApiParser<RawBuildingGetShip>
    {
        public override void Process(RawBuildingGetShip rpData)
        {
            Game.UpdateBuildingDocks(rpData.BuildingDocks);
            Game.AddEquipments(rpData.Equipments);
            Game.AddShip(rpData.ID, rpData.ShipID, rpData.Ship);
        }
    }
}
