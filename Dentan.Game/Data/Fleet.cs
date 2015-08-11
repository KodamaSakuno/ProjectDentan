using Moen.KanColle.Dentan.Data.Raw;
using System.Collections.Generic;
using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    public enum FleetState { Idle, Sortie, Expedition }

    public class Fleet : RawDataWrapper<RawFleet>, IID
    {
        public int ID { get { return RawData.ID; } }
        public string Name { get { return RawData.Name; } }

        internal List<Ship> ShipList {get;set;}
        Ship[] r_Ships;
        public Ship[] Ships
        {
            get { return r_Ships; }
            internal set
            {
                if (r_Ships != value)
                {
                    if (r_Ships != null)
                        foreach (var rShip in r_Ships)
                            rShip.OwnerFleet = null;

                    r_Ships = value;

                    foreach (var rShip in r_Ships)
                        rShip.OwnerFleet = this;

                    CheckSupply();
                    LoS = Calc.CalcLos(this);

                    OnPropertyChanged();
                }
            }
        }

        public Expedition Expedition { get; private set; }

        FleetState r_State;
        public FleetState State
        {
            get { return r_State; }
            set
            {
                if (r_State != value)
                {
                    r_State = value;
                    OnPropertyChanged();
                }
            }
        }

        int r_TotalLevel;
        public int TotalLevel
        {
            get { return r_TotalLevel; }
            set
            {
                if (r_TotalLevel != value)
                {
                    r_TotalLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        double r_LoS;
        public double LoS
        {
            get { return r_LoS; }
            set
            {
                if (r_LoS != value)
                {
                    r_LoS = value;
                    OnPropertyChanged();
                }
            }
        }
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

        bool r_NeedSupply;
        public bool NeedSupply
        {
            get { return r_NeedSupply; }
            set
            {
                if (r_NeedSupply != value)
                {
                    r_NeedSupply = value;
                    OnPropertyChanged();
                }
            }
        }

        int r_UnsuppliedFuel;
        public int UnsuppliedFuel
        {
            get { return r_UnsuppliedFuel; }
            set
            {
                if (r_UnsuppliedFuel != value)
                {
                    r_UnsuppliedFuel = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_UnsuppliedBullet;
        public int UnsuppliedBullet
        {
            get { return r_UnsuppliedBullet; }
            set
            {
                if (r_UnsuppliedBullet != value)
                {
                    r_UnsuppliedBullet = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_UnsuppliedPlane;
        public int UnsuppliedPlane
        {
            get { return r_UnsuppliedPlane; }
            set
            {
                if (r_UnsuppliedPlane != value)
                {
                    r_UnsuppliedPlane = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_UnsuppliedBauxite;
        public int UnsuppliedBauxite
        {
            get { return r_UnsuppliedBauxite; }
            set
            {
                if (r_UnsuppliedBauxite != value)
                {
                    r_UnsuppliedBauxite = value;
                    OnPropertyChanged();
                }
            }
        }

        bool r_HasHeavilyDamage;
        public bool HasHeavilyDamage
        {
            get { return r_HasHeavilyDamage; }
            set
            {
                if (r_HasHeavilyDamage != value)
                {
                    r_HasHeavilyDamage = value;
                    OnPropertyChanged();
                }
            }
        }

        public Fleet(RawFleet rpRawData)
            : base(rpRawData) { }

        protected override void OnRawDataUpdated()
        {
            ShipList = RawData.Ship.TakeWhile(r => r != -1).Select(r => KanColleGame.Current.Ships[r]).ToList();
            UpdateShips();

            AA = Ships.Sum(r => r.FighterPower);

            if (Expedition == null)
                Expedition = new Expedition(this); 
            Expedition.Update((int)RawData.Expedition[0], (int)RawData.Expedition[1], RawData.Expedition[2]);

            if (KanColleGame.Current.SortieFleet == this)
                State = FleetState.Sortie;
            else if (KanColleGame.Current.CombinedFleet != CombinedFleetFlag.None && KanColleGame.Current.SortieFleet?.ID == 1 && ID == 2)
                State = FleetState.Sortie;
            else if (Expedition.ExpeditionID != -1)
                State = FleetState.Expedition;
            else
                State = FleetState.Idle;
        }

        internal void UpdateShips()
        {
            Ships = ShipList.ToArray();
            TotalLevel = Ships.Sum(r => r.Level);
            CheckDemage();
            if (ID != 1)
                foreach (var rExpedition in KanColleGame.Current.Base.Expeditions.Values)
                    rExpedition.Update(this);
        }

        public void CheckSupply()
        {
            NeedSupply = r_Ships.Any(r => r.NeedSupply);

            if (NeedSupply)
            {
                UnsuppliedFuel = r_Ships.Sum(r => r.Info.MaxFuel - r.Fuel);
                UnsuppliedBullet = r_Ships.Sum(r => r.Info.MaxBullet - r.Bullet);
                UnsuppliedPlane = r_Ships.Sum(r => r.Info.PlaneCount.Zip(r.Slots, (rpMaxPlaneCount, rpSlot) => rpMaxPlaneCount - rpSlot.PlaneCount).Sum());
                UnsuppliedBauxite = UnsuppliedPlane * 5;
            }

            LoS = Calc.CalcLos(this);
            AA = Ships.Sum(r => r.FighterPower);
        }
        public void CheckDemage()
        {
            HasHeavilyDamage = r_Ships.Any(r => r.DamageStatus == ShipDamageStatus.Heavily);
        }

        public override string ToString()
        {
            return string.Format("ID={0}, Name={1}", ID, Name);
        }
    }
}
