namespace Moen.KanColle.Dentan.Data
{
    public class BuildingDockCompletedEventArgs
    {
        public string ShipName { get; private set; }

        public BuildingDockCompletedEventArgs(string rpShipName)
        {
            ShipName = rpShipName;
        }
    }
}
