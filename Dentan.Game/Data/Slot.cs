using System;
namespace Moen.KanColle.Dentan.Data
{
    public class Slot : ModelBase
    {
        Equipment r_Equipment;
        public Equipment Equipment
        {
            get { return r_Equipment; }
            set
            {
                if (r_Equipment != value)
                {
                    r_Equipment = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxPlaneCount { get; set; }

        int r_PlaneCount;
        public int PlaneCount
        {
            get { return r_PlaneCount; }
            set
            {
                if (r_PlaneCount != value)
                {
                    r_PlaneCount = value;
                    HasLostPlane = r_PlaneCount != MaxPlaneCount;
                    OnPropertyChanged();
                }
            }
        }
        bool r_HasLostPlane;
        public bool HasLostPlane
        {
            get { return r_HasLostPlane; }
            set
            {
                if (r_HasLostPlane != value)
                {
                    r_HasLostPlane = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PlaneAA
        {
            get
            {
                if (Equipment == null ||
                    (Equipment.Type != EquipmentType.CarrierBasedFighter &&
                     Equipment.Type != EquipmentType.CarrierBasedDiveBomber &&
                     Equipment.Type != EquipmentType.CarrierBasedTorpedoBomber &&
                     Equipment.Type != EquipmentType.SeaplaneBomber))
                    return 0;
                return (int)(Equipment.Info.AA * Math.Sqrt(PlaneCount));
            }
        }

        public Slot(Equipment rpEquipment, int rpPlaneCount, int rpMaxPlaneCount)
        {
            Equipment = rpEquipment;
            MaxPlaneCount = rpMaxPlaneCount;
            PlaneCount = rpPlaneCount;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}/{2})", Equipment, PlaneCount, MaxPlaneCount);
        }
    }
}
