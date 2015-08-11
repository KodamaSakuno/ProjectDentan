using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Map
{
    [Api("api_get_member/mapinfo")]
    class MapInfomationParser : ApiParser<RawMapInfo[]>
    {
        public override void Process(RawMapInfo[] rpData)
        {
            Game.EventRank = rpData.Where(r => r.EventMap != null).ToDictionary(r => r.ID, r => r.EventMap.SelectedRank);

            foreach (var rMapInfo in rpData.Where(r => r.DefeatCount.HasValue))
            {
                Game.Base.MapInfos[rMapInfo.ID].IsCleared = rMapInfo.IsCleared;
                Game.Base.MapInfos[rMapInfo.ID].DefeatCount = rMapInfo.DefeatCount;
            }
            foreach (var rMapInfo in rpData.Where(r => r.EventMap != null))
                Game.Base.MapInfos[rMapInfo.ID].MapHP = new MapHP()
                {
                    Max = rMapInfo.EventMap.MaxHP,
                    Now = rMapInfo.EventMap.NowHP,
                };
        }
    }
}
