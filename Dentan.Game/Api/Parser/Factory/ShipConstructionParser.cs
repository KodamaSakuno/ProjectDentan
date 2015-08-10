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

            Game.SendMessageToStatusBar($"开始建造「{rDock.Ship.Name}」 投入资材：{rDock.Fuel}/{rDock.Bullet}/{rDock.Steel}/{rDock.Bauxite}/{rDock.DevelopmentMaterial}");

            Quest.Progresses[606].Current++;
            Quest.Progresses[608].Current++;
        }
    }
}
