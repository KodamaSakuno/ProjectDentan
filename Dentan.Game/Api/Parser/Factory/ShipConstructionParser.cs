using Moen.KanColle.Dentan.Data;

namespace Moen.KanColle.Dentan.Api.Parser.Factory
{
    [Api("api_req_kousyou/createship")]
    class ShipConstructionParser : ApiParser
    {
        public override void Process()
        {
            var rDock = Game.BuildingDocks[int.Parse(Request["api_kdock_id"])];
            rDock.PostRecord();

            Quest.Progresses[606].Current++;
            Quest.Progresses[608].Current++;
        }
    }
}
