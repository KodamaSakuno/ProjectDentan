namespace Moen.KanColle.Dentan.Data
{
    public class ExpeditionReturnedEventArgs
    {
        public string FleetName { get; private set; }
        public string ExpeditionName { get; private set; }

        public ExpeditionReturnedEventArgs(string rpFleetName, string rpMissionName)
        {
            FleetName = rpFleetName;
            ExpeditionName = rpMissionName;
        }
    }
}
