namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_get_member/mapinfo")]
    [Api("api_get_member/practice")]
    [Api("api_get_member/mission")]
    class ClearEnemyFleetInfoParser : ApiParser
    {
        public override void Process()
        {
            Game.CompassData = null;
        }
    }
}
