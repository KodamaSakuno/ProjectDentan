using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.QuestData;
using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Api.Parser.Map
{
    [Api("api_req_map/start")]
    class MapStartParser : MapNextParser
    {
        public override void Process(RawMapNext rpData)
        {
            var rSortieFleetID = int.Parse(Request["api_deck_id"]);
            Game.SortieFleet = Game.Fleets[rSortieFleetID];
            Game.SortieFleet.State = FleetState.Sortie;

            var rCompassData = new CompassData(rpData.MapAreaID, rpData.MapInfoNo);

            var rMapInfo = Game.Base.MapInfos[rCompassData.MapID];
            if (!rMapInfo.IsCleared && rMapInfo.RequiredDefeatCount.HasValue && rMapInfo.DefeatCount.HasValue && rMapInfo.DefeatCount.Value < rMapInfo.RequiredDefeatCount.Value)
                rCompassData.MapHP = new MapHP()
                {
                    Max = rMapInfo.RequiredDefeatCount.Value,
                    Now = rMapInfo.DefeatCount.Value,
                };

            Game.CompassData = rCompassData;

            var rCodeAProgress = Quest.Progresses[214] as CodeAProgress;
            if (rCodeAProgress != null)
                rCodeAProgress.IncrementSortie();

            Game.SendMessageToStatusBar($"舰队「{Game.SortieFleet.Name}」向「{rCompassData.MapName}」出击");

            base.Process(rpData);
        }
    }
}
