namespace Moen.KanColle.Dentan.Api.Parser.Map
{
    [Api("api_req_map/select_eventmap_rank")]
    class EventMapRankSelectionParser : ApiParser
    {
        public override void Process()
        {
            var rMapID = int.Parse(Request["api_maparea_id"]) * 10 + int.Parse(Request["api_map_no"]);
            Game.EventRank[rMapID] = int.Parse(Request["api_rank"]);

            Game.Base.MapInfos[rMapID].MapHP = null;
        }
    }
}
