using System.Collections.Generic;

namespace Moen.KanColle.Dentan.Data.QuestData
{
    abstract class BossTargetProgress : QuestProgressData
    {
        public HashSet<int> MapIDs { get; private set; }

        public BattleRank LowestRank { get; private set; }

        protected BossTargetProgress(int[] rpMapIDs, BattleRank rpLowestRank)
        {
            MapIDs = rpMapIDs == null ? null : new HashSet<int>(rpMapIDs);
            LowestRank = rpLowestRank;
        }
        
        internal virtual void Process(BattleData rpBattle)
        {
            if (!rpBattle.IsBossBattle) return;

            if (MapIDs.Contains(rpBattle.MapID) && rpBattle.Rank >= LowestRank && !rpBattle.QuestCounter.Contains(ID))
            {
                Current++;
                rpBattle.QuestCounter.Add(ID);
            }
        }
    }
    abstract class BossTargetWithRequirement : BossTargetProgress
    {
        protected BossTargetWithRequirement(int[] rpMapIDs, BattleRank rpLowestRank)
            : base(rpMapIDs, rpLowestRank) { }

        internal override void Process(BattleData rpBattle)
        {
            if (ShipRequirement.Check(ID))
                base.Process(rpBattle);
        }
    }

    [Quest(226, QuestType.Daily, 5)]
    class SoundWesternBoss : BossTargetProgress
    {
        public SoundWesternBoss()
            : base(new[] { 21, 22, 23, 24, 25 }, BattleRank.B) { }
    }
    [Quest(229, QuestType.Weekly, 12)]
    class EasternBoss : BossTargetProgress
    {
        public EasternBoss()
            : base(new[] { 41, 42, 43, 44, 45 }, BattleRank.B) { }
    }
    [Quest(242, QuestType.Weekly, 1)]
    class Eastern2Boss : BossTargetProgress
    {
        public Eastern2Boss()
            : base(new[] { 44 }, BattleRank.B) { }
    }
    [Quest(243, QuestType.Weekly, 2)]
    class SouthernBoss : BossTargetProgress
    {
        public SouthernBoss()
            : base(new[] { 52 }, BattleRank.S) { }
    }
    [Quest(241, QuestType.Weekly, 5)]
    class NothernBoss : BossTargetProgress
    {
        public NothernBoss()
            : base(new[] { 33, 34, 35 }, BattleRank.B) { }
    }
    [Quest(256, QuestType.Monthly, 3)]
    class SW61Boss : BossTargetProgress
    {
        public SW61Boss()
            : base(new[] { 61 }, BattleRank.S) { }
    }

    [Quest(261, QuestType.Weekly, 3)]
    class Weekly15Boss : BossTargetProgress
    {
        public Weekly15Boss()
            : base(new[] { 15 }, BattleRank.A) { }
    }
    [Quest(265, QuestType.Monthly, 10)]
    class Monthly15Boss : BossTargetProgress
    {
        public Monthly15Boss()
            : base(new[] { 15 }, BattleRank.A) { }
    }
}
