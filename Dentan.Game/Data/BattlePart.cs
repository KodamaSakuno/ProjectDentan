using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    public enum BattlePartType { Day, Night, NightSpecial, DaySpecial }

    public class BattlePart : ModelBase
    {
        public BattleData Owner { get; private set; }

        BattlePartType r_Type;
        public BattlePartType Type
        {
            get { return r_Type; }
            internal set
            {
                if (r_Type != value)
                {
                    r_Type = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsFirstPart { get { return Type == BattlePartType.Day || Type == BattlePartType.NightSpecial; } }

        bool r_IsInitializing;
        public bool IsInitializing
        {
            get { return r_IsInitializing; }
            set
            {
                if (r_IsInitializing != value)
                {
                    r_IsInitializing = value;
                    OnPropertyChanged();
                }
            }
        }

        BattleStatus[] r_FriendStatus;
        public BattleStatus[] FriendStatus
        {
            get { return r_FriendStatus; }
            set
            {
                if (r_FriendStatus != value)
                {
                    r_FriendStatus = value;
                    for (var i = 0; i < value.Length; i++)
                    {
                        var rStatus = value[i];
                        if (rStatus == BattleStatus.Default)
                            break;

                        var rSortieFleet = KanColleGame.Current.SortieFleet;
                        if (rSortieFleet != null)
                        {
                            var rInfo = rSortieFleet.Ships[i];
                            rStatus.ShipInfo = rInfo;
                        }
                    }

                    if (value.All(r => r.GivenDamage == 0))
                        value[0].IsMVP = true;
                    else
                    {
                        var rMVPStatus = value.OrderByDescending(r => r.GivenDamage).First();
                        rMVPStatus.IsMVP = true;
                    }

                    OnPropertyChanged();
                }
            }
        }

        BattleStatus[] r_FriendStatusCombined;
        public BattleStatus[] FriendStatusCombined
        {
            get { return r_FriendStatusCombined; }
            set
            {
                if (r_FriendStatusCombined != value)
                {
                    r_FriendStatusCombined = value;
                    for (var i = 0; i < r_FriendStatusCombined.Length; i++)
                    {
                        var rStatus = r_FriendStatusCombined[i];
                        if (rStatus == BattleStatus.Default)
                            break;

                        var rSortieFleet = KanColleGame.Current.Fleets[2];
                        if (rSortieFleet != null)
                        {
                            var rInfo = rSortieFleet.Ships[i];
                            rStatus.ShipInfo = rInfo;
                        }
                    }

                    if (value.All(r => r.GivenDamage == 0))
                        value[0].IsMVP = true;
                    else
                    {
                        var rMVPStatus = value.OrderByDescending(r => r.GivenDamage).First();
                        rMVPStatus.IsMVP = true;
                    }

                    OnPropertyChanged();
                }
            }
        }

        BattleStatus[] r_EnemyStatus;
        public BattleStatus[] EnemyStatus
        {
            get { return r_EnemyStatus; }
            set
            {
                if (r_EnemyStatus != value)
                {
                    r_EnemyStatus = value;
                    for (var i = 0; i < r_EnemyStatus.Length; i++)
                    {
                        var rStatus = r_EnemyStatus[i];
                        if (rStatus == BattleStatus.Default || KanColleGame.Current.CompassData.EnemyFleet == null)
                            break;

                        var rShip = KanColleGame.Current.CompassData.EnemyFleet.Ships[i];

                        rStatus.ShipInfo = rShip;

                        //rStatus.IsLandBase = rShip.Info.Speed == ShipSpeed.LandBase;
                    }

                    OnPropertyChanged();
                }
            }
        }

        public BattlePart(BattleData rpOwner, BattlePartType rpType)
        {
            Owner = rpOwner;
            Type = rpType;

            IsInitializing = true;
        }
    }
}
