namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class EquipedShipViewModel : ModelBase
    {
        public ShipViewModel Ship { get; set; }
        public int Count { get; set; }

        internal EquipedShipViewModel(ShipViewModel rpShip, int rpCount)
        {
            Ship = rpShip;
            Count = rpCount;
        }
    }
}
