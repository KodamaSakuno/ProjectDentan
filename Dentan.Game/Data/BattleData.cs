using System;
using System.Collections.Generic;
using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    public class BattleData : ModelBase
    {
        public int MapID { get; internal set; }
        public bool IsBossBattle { get; internal set; }
        public HashSet<int> QuestCounter { get; private set; }

        public BattlePart DayBattle { get; private set; }
        BattlePart r_NightBattle;
        public BattlePart NightBattle
        {
            get { return r_NightBattle; }
            internal set
            {
                if (r_NightBattle != value)
                {
                    r_NightBattle = value;
                    OnPropertyChanged();
                }
            }
        }

        bool? r_CanNightBattle;
        public bool? CanNightBattle
        {
            get { return r_CanNightBattle; }
            set
            {
                if (r_CanNightBattle != value)
                {
                    r_CanNightBattle = value;
                    OnPropertyChanged();
                }
            }
        }

        Formation r_FriendFormation;
        public Formation FriendFormation
        {
            get { return r_FriendFormation; }
            set
            {
                if (r_FriendFormation != value)
                {
                    r_FriendFormation = value;
                    OnPropertyChanged();
                }
            }
        }
        Formation r_EnemyFormation;
        public Formation EnemyFormation
        {
            get { return r_EnemyFormation; }
            set
            {
                if (r_EnemyFormation != value)
                {
                    r_EnemyFormation = value;
                    OnPropertyChanged();
                }
            }
        }

        EngagementForm r_EngagementForm;
        public EngagementForm EngagementForm
        {
            get { return r_EngagementForm; }
            set
            {
                if (r_EngagementForm != value)
                {
                    r_EngagementForm = value;
                    OnPropertyChanged();
                }
            }
        }

        AerialCombat r_AerialCombat;
        public AerialCombat AerialCombat
        {
            get { return r_AerialCombat; }
            set
            {
                if (r_AerialCombat != value)
                {
                    r_AerialCombat = value;
                    OnPropertyChanged();
                }
            }
        }

        double? r_FriendDamageRate;
        public double? FriendDamageRate
        {
            get { return r_FriendDamageRate; }
            set
            {
                if (r_FriendDamageRate != value)
                {
                    r_FriendDamageRate = value;
                    OnPropertyChanged();
                }
            }
        }
        double? r_EnemyDamageRate;
        public double? EnemyDamageRate
        {
            get { return r_EnemyDamageRate; }
            set
            {
                if (r_EnemyDamageRate != value)
                {
                    r_EnemyDamageRate = value;
                    OnPropertyChanged();
                }
            }
        }

        BattleRank? r_Rank;
        public BattleRank? Rank
        {
            get { return r_Rank; }
            set
            {
                if (r_Rank != value)
                {
                    r_Rank = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEnemyFlagshipSunk
        {
            get
            {
                return (NightBattle == null ? DayBattle.EnemyStatus : NightBattle.EnemyStatus ?? DayBattle.EnemyStatus)[0].DamageStatus == ShipDamageStatus.Sink;
            }
        }

        internal BattleData()
        {
            DayBattle = new BattlePart(this, BattlePartType.Day);
            NightBattle = new BattlePart(this, BattlePartType.Night);
            QuestCounter = new HashSet<int>();

            AerialCombat = new AerialCombat();
        }

        internal void UpdateDamageRate()
        {
            var rEnemyBeforeHPTotal = (double)DayBattle.EnemyStatus.Sum(r => r.BeforeHP);
            var rFriendBeforeHPTotal = (double)DayBattle.FriendStatus.Sum(r => r.BeforeHP);
            if (DayBattle.FriendStatusCombined != null)
                rFriendBeforeHPTotal += DayBattle.FriendStatusCombined.Sum(r => r.BeforeHP);

            var rEnemyStatus = NightBattle == null ? DayBattle.EnemyStatus : NightBattle.EnemyStatus ?? DayBattle.EnemyStatus;
            var rEnemyAfterHPTotal = (double)rEnemyStatus.Sum(r => Math.Max(r.NowHP, 0));
            var rFriendAfterHPTotal = (double)(NightBattle == null ? DayBattle.FriendStatus : NightBattle.FriendStatus ?? DayBattle.FriendStatus).Sum(r => Math.Max(r.NowHP, 0));
            var rFriendCombinedStatus = NightBattle == null ? DayBattle.FriendStatusCombined : NightBattle.FriendStatusCombined ?? DayBattle.FriendStatusCombined;
            if (rFriendCombinedStatus != null)
                rFriendAfterHPTotal += rFriendCombinedStatus.Sum(r => Math.Max(r.NowHP, 0));

            FriendDamageRate = (rFriendBeforeHPTotal - rFriendAfterHPTotal) / rFriendBeforeHPTotal * 100;
            EnemyDamageRate = (rEnemyBeforeHPTotal - rEnemyAfterHPTotal) / rEnemyBeforeHPTotal * 100;

            var rEnemySunkCount = rEnemyStatus.Count(r => r.DamageStatus == ShipDamageStatus.Sink);

            if (EnemyDamageRate >= 100)
                Rank = FriendDamageRate <= 0 ? BattleRank.SS : BattleRank.S;
            else if (rEnemySunkCount >= Math.Round(rEnemyStatus.Length * 0.6))
                Rank = BattleRank.A;
            else if (IsEnemyFlagshipSunk || EnemyDamageRate > FriendDamageRate * 2.5)
                Rank = BattleRank.B;
            else if (EnemyDamageRate > FriendDamageRate || EnemyDamageRate >= 50.0)
                Rank = BattleRank.C;
            else
                Rank = BattleRank.D;
        }
    }
}
