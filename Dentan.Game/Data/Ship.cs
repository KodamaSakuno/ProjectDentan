using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;
using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    public enum ShipDamageStatus { Healthy, Minor, Moderate, Heavily, Sink, Confused, Damaged, Broken, Destroyed, Escaped }

    public class Ship : RawDataWrapper<RawShip>, IID, IBattleShipInfo
    {
        ShipInfo r_Info;
        public ShipInfo Info
        {
            get { return r_Info; }
            set
            {
                if (r_Info != value)
                {
                    r_Info = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name { get { return Info.Name; } }

        public int ID { get { return RawData.ID; } }
        public int ShipID { get { return RawData.ShipID; } }
        public int SortNo { get { return RawData.SortNo; } }
        public int Level { get { return RawData.Level; } }

        int r_Experience;
        public int Experience
        {
            get { return r_Experience; }
            set
            {
                if (r_Experience != value)
                {
                    r_Experience = value;
                    OnPropertyChanged();
                }
            }
        }
        public int NextExperience { get { return RawData.Experience[1]; } }

        public int LoS { get { return RawData.LoS[0]; } }

        int r_NowHP;
        public int NowHP
        {
            get { return r_NowHP; }
            set
            {
                if (r_NowHP != value)
                {
                    r_NowHP = value;
                    if (r_OwnerFleet != null)
                        r_OwnerFleet.CheckDemage();

                    OnPropertyChanged();
                }
            }
        }
        int r_MaxHP;
        public int MaxHP
        {
            get { return r_MaxHP; }
            set
            {
                if (r_MaxHP != value)
                {
                    r_MaxHP = value;
                    OnPropertyChanged();
                }
            }
        }

        public ShipDamageStatus DamageStatus
        {
            get
            {
                var rRatio = NowHP / (double)MaxHP;

                if (rRatio <= 0.0)
                    return ShipDamageStatus.Sink;
                else if (rRatio <= 0.25)
                    return ShipDamageStatus.Heavily;
                else if (rRatio <= 0.5)
                    return ShipDamageStatus.Moderate;
                else if (rRatio <= 0.75)
                    return ShipDamageStatus.Minor;
                else
                    return ShipDamageStatus.Healthy;
            }
        }

        int r_Condition;
        public int Condition
        {
            get { return r_Condition; }
            set
            {
                if (r_Condition != value)
                {
                    r_Condition = value;
                    OnPropertyChanged();
                }
            }
        }

        int r_Fuel;
        public int Fuel
        {
            get { return r_Fuel; }
            set
            {
                if (r_Fuel != value)
                {
                    r_Fuel = value;
                    if (r_OwnerFleet != null)
                        r_OwnerFleet.CheckSupply();

                    OnPropertyChanged();
                }
            }
        }
        int r_BeforeFuel;
        public int BeforeFuel
        {
            get { return r_BeforeFuel; }
            set
            {
                if (r_BeforeFuel != value)
                {
                    r_BeforeFuel = value;
                    OnPropertyChanged();
                }
            }
        }

        int r_Bullet;
        public int Bullet
        {
            get { return r_Bullet; }
            set
            {
                if (r_Bullet != value)
                {
                    r_Bullet = value;
                    if (r_OwnerFleet != null)
                        r_OwnerFleet.CheckSupply();

                    OnPropertyChanged();
                }
            }
        }
        int r_BeforeBullet;
        public int BeforeBullet
        {
            get { return r_BeforeBullet; }
            set
            {
                if (r_BeforeBullet != value)
                {
                    r_BeforeBullet = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool NeedSupply { get { return Fuel != Info.MaxFuel || Bullet != Info.MaxBullet; } }

        bool r_IsRepairing;
        public bool IsRepairing
        {
            get { return r_IsRepairing; }
            set
            {
                if (r_IsRepairing != value)
                {
                    r_IsRepairing = value;
                    OnPropertyChanged();
                }
            }
        }

        int[] r_EquipmentIDs;
        Slot[] r_Slots;
        public Slot[] Slots
        {
            get { return r_Slots; }
            set
            {
                if (r_Slots != value)
                {
                    r_Slots = value;
                    OnPropertyChanged();
                }
            }
        }

        Fleet r_OwnerFleet;
        public Fleet OwnerFleet
        {
            get { return r_OwnerFleet; }
            set
            {
                if (r_OwnerFleet != value)
                {
                    r_OwnerFleet = value;
                    OnPropertyChanged();
                }
            }
        }

        int r_FighterPower;
        public int FighterPower
        {
            get { return r_FighterPower; }
            set
            {
                if (r_FighterPower != value)
                {
                    r_FighterPower = value;
                    OnPropertyChanged();
                }
            }
        }

        bool r_Initialized;

        public Ship(RawShip rpRawData)
            : base(rpRawData) { }

        protected override void OnRawDataUpdated()
        {
            if (Info == null || Info.ID != RawData.ShipID)
            {
                ShipInfo rInfo;
                if (!KanColleGame.Current.Base.Ships.TryGetValue(RawData.ShipID, out rInfo))
                    rInfo = ShipInfo.Default;
                Info = rInfo;
            }

            NowHP = RawData.NowHP;
            MaxHP = RawData.MaxHP;
            Condition = RawData.Condition;
            Fuel = BeforeFuel = RawData.Fuel;
            Bullet = BeforeBullet = RawData.Bullet;

            var rRawExperience = RawData.Experience[0];
            if (r_Initialized && rRawExperience > 0 && Experience != rRawExperience)
                RecordManager.Instance.Experience.AddShipExpData(ID, rRawExperience);
            Experience = rRawExperience;

            if (r_EquipmentIDs == null || !r_EquipmentIDs.SequenceEqual(RawData.Equipments))
            {
                r_EquipmentIDs = RawData.Equipments;
                Slots = RawData.Equipments.Take(RawData.EquipmentCount)
                    .Zip(RawData.PlaneCount.Zip(Info.PlaneCount, (rpCount, rpMaxCount) => new { Count = rpCount, MaxCount = rpMaxCount }),
                        (rpID, rpPlane) =>
                        {
                            Equipment rEquipment;
                            if (rpID == -1)
                                rEquipment = Equipment.Default;
                            else if (!KanColleGame.Current.Equipments.TryGetValue(rpID, out rEquipment))
                                KanColleGame.Current.Equipments.Add(rEquipment = new Equipment(new RawEquipment() { ID = rpID, EquipmentID = -1 }));

                            return new Slot(rEquipment, rpPlane.Count, rpPlane.MaxCount);
                        }).ToArray();
            }

            for (var i = 0; i < Slots.Length; i++)
                Slots[i].PlaneCount = RawData.PlaneCount[i];

            FighterPower = Slots.Sum(r => r.PlaneAA);

            if (!r_Initialized)
                r_Initialized = true;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1} ({2})", ID, Name, Level);
        }
    }
}
