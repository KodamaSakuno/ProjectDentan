namespace Moen.KanColle.Dentan.Data
{
    public class RepairDockCompletedEventArgs
    {
        public string ShipName { get; private set; }

        public RepairDockCompletedEventArgs(string rpShipName)
        {
            ShipName = rpShipName;
        }
    }
}
