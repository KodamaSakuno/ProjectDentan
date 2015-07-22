namespace Moen.KanColle.Dentan.Api.Parser.Factory
{
    [Api("api_req_kousyou/createship_speedchage")]
    class ConstructionChangeSpeedParser : ApiParser
    {
        public override void Process()
        {
            var rDock = Game.BuildingDocks[int.Parse(Request["api_kdock_id"])];

            if (Request["api_highspeed"] == "1")
                rDock.CompleteConstruction();
        }
    }
}
