
namespace Moen.KanColle.Dentan.Data
{
    public class EnemyShip : IBattleShipInfo
    {
        public ShipInfo Info { get; internal set; }
        public int Type { get { return Info.Type; } }

        public string Name { get { return !CustomName.IsNullOrEmpty() ? CustomName : Info.Name; } }
        public string CustomName { get; internal set; }

        public int Level { get; internal set; }
        public Slot[] Slots { get; private set; }

        public EnemyShip(ShipInfo rpInfo, int rpLevel, Slot[] rpSlots)
        {
            Info = rpInfo;
            Level = rpLevel;
            Slots = rpSlots;
        }
    }
}
