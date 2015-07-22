using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    public class EnemyFleet : ModelBase, IID
    {
        public int ID { get; set; }

        public bool IsPracticeFleet { get; internal set; }

        string r_Name;
        public string Name
        {
            get { return r_Name; }
            set
            {
                if (r_Name != value)
                {
                    r_Name = value;
                    OnPropertyChanged();
                }
            }
        }

        Formation r_Formation;
        public Formation Formation
        {
            get { return r_Formation; }
            set
            {
                if (r_Formation != value)
                {
                    r_Formation = value;
                    OnPropertyChanged();
                }
            }
        }

        public int[] ShipIDs { get; set; }
        EnemyShip[] r_Ships;
        public EnemyShip[] Ships
        {
            get
            {
                if (r_Ships == null && ShipIDs != null)
                    UpdateShips();
                return r_Ships;
            }
            set
            {
                r_Ships = value;
                OnPropertyChanged();
            }
        }

        public bool HasEquipments { get; set; }

        int r_AA;
        public int AA
        {
            get { return r_AA; }
            set
            {
                if (r_AA != value)
                {
                    r_AA = value;
                    OnPropertyChanged();
                }
            }
        }

        int? r_PracticeExperience;
        public int? PracticeExperience
        {
            get { return r_PracticeExperience; }
            set
            {
                if (r_PracticeExperience != value)
                {
                    r_PracticeExperience = value;
                    OnPropertyChanged();
                }
            }
        }

        public AbyssalFleet AbyssalFleet { get; set; }

        public void UpdateShips()
        {
            Ships = ShipIDs.Select(r =>
            {
                var rInfo = KanColleGame.Current.Base.Ships[r];

                Slot[] rSlots;
                if (IsPracticeFleet)
                    rSlots = rInfo.PlaneCount.Take(rInfo.EquipmentCount).Select(rpCount => new Slot(null, rpCount, rpCount)).ToArray();
                else
                {
                    var rData = AbyssalShip.FromID(r);
                    if (rData == null || rData.Equipments == null)
                        rSlots = rInfo.PlaneCount.Take(rInfo.EquipmentCount).Select(rpCount => new Slot(null, rpCount, rpCount)).ToArray();
                    else
                        rSlots = rData.Equipments.Zip(rInfo.PlaneCount, (rpEquipment, rpCount) => new Slot(rpEquipment, rpCount, rpCount)).ToArray();
                }

                string rCustomName = null;
                if (rInfo.AbyssalShipClass.HasValue && rInfo.AbyssalShipClass.Value == AbyssalShipClass.LateModel)
                    rCustomName = rInfo.Name.Replace("後期型", string.Empty);

                return new EnemyShip(rInfo, 1, rSlots) { CustomName = rCustomName };
            }).ToArray();
            UpdateAA();

            if (!IsPracticeFleet)
                HasEquipments = Ships.All(r => r.Slots.All(rpSlot => rpSlot.Equipment != null));
        }
        public void UpdateAA()
        {
            AA = Ships.Sum(r => r.Slots.Sum(rpSlot => rpSlot.PlaneAA));
        }
    }
}
