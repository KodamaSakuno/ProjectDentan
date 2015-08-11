namespace Moen.KanColle.Dentan.Data
{    
    public class BattleStatus
    {
        static BattleStatus r_Default = new BattleStatus(10, 5);
        public static BattleStatus Default { get { return r_Default; } }
        
        public IBattleShipInfo ShipInfo { get; internal set; }

        public int MaxHP { get; private set; }
        public int NowHP { get; set; }
        public int BeforeHP { get; private set; }
        public int Damage { get { return BeforeHP - NowHP; } }

        public int GivenDamage { get; internal set; }

        public ShipDamageStatus DamageStatus
        {
            get
            {
                if (IsEscaped)
                    return ShipDamageStatus.Escaped;

                var rRatio = NowHP / (double)MaxHP;

                if (ShipInfo != null && ShipInfo.Info.Speed == ShipSpeed.LandBase)
                    if (rRatio <= 0.0)
                        return ShipDamageStatus.Destroyed;
                    else if (rRatio <= 0.25)
                        return ShipDamageStatus.Broken;
                    else if (rRatio <= 0.5)
                        return ShipDamageStatus.Damaged;
                    else if (rRatio <= 0.75)
                        return ShipDamageStatus.Confused;
                    else
                        return ShipDamageStatus.Healthy;

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
        
        public bool IsEscaped { get; internal set; }

        public BattleStatus(int rpMaxHP, int rpNowHP)
        {
            MaxHP = rpMaxHP;
            NowHP = BeforeHP = rpNowHP;
        }

        public override string ToString()
        {
            return string.Format("{0}->{1}/{2}", BeforeHP, NowHP, MaxHP);
        }
    }
}
