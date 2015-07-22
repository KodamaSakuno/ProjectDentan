using Moen.KanColle.Dentan.Record;
using System;
using System.Collections.Specialized;

namespace Moen.KanColle.Dentan.Data.QuestData
{
    [Quest(214, QuestType.Weekly, 1)]
    class CodeAProgress : QuestProgressData
    {
        BitVector32 r_Bits;
        static BitVector32.Section[] r_Sections;

        public override int Current
        {
            get { return r_Bits.Data; }
            internal set
            {
                if (!RecordManager.Instance.Quest.IsInitialized)
                {
                    r_Bits = new BitVector32(value);
                    Update();
                }
            }
        }
        internal override int CurrentInternal
        {
            set 
            {
                r_Bits = new BitVector32(value);
                Update();
            }
        }

        public int SortieTotal { get { return 36; } }
        public int Sortie
        {
            get { return Math.Min(SortieTotal, r_Bits[r_Sections[0]]); }
            internal set
            {
                var rValue = Math.Min(SortieTotal, value);
                if (r_Bits[r_Sections[0]] != rValue)
                {
                    r_Bits[r_Sections[0]] = rValue;
                    OnPropertyChanged();
                }
            }
        }

        public int SWinTotal { get { return 6; } }
        public int SWin
        {
            get { return Math.Min(SWinTotal, r_Bits[r_Sections[1]]); }
            internal set
            {
                var rValue = Math.Min(SWinTotal, value);
                if (r_Bits[r_Sections[1]] != rValue)
                {
                    r_Bits[r_Sections[1]] = rValue;
                    OnPropertyChanged();
                }
            }
        }

        public int BossBattleTotal { get { return 24; } }
        public int BossBattle
        {
            get { return Math.Min(BossBattleTotal, r_Bits[r_Sections[2]]); }
            internal set
            {
                var rValue = Math.Min(BossBattleTotal, value);
                if (r_Bits[r_Sections[2]] != rValue)
                {
                    r_Bits[r_Sections[2]] = rValue;
                    OnPropertyChanged();
                }
            }
        }

        public int BossBattleWinTotal { get { return 12; } }
        public int BossBattleWin
        {
            get { return Math.Min(BossBattleWinTotal, r_Bits[r_Sections[3]]); }
            internal set
            {
                var rValue = Math.Min(BossBattleWinTotal, value);
                if (r_Bits[r_Sections[3]] != rValue)
                {
                    r_Bits[r_Sections[3]] = rValue;
                    OnPropertyChanged();
                }
            }
        }

        static CodeAProgress()
        {
            r_Sections = new BitVector32.Section[4];
            r_Sections[0] = BitVector32.CreateSection(36);
            r_Sections[1] = BitVector32.CreateSection(6, r_Sections[0]);
            r_Sections[2] = BitVector32.CreateSection(24, r_Sections[1]);
            r_Sections[3] = BitVector32.CreateSection(12, r_Sections[2]);
        }
        public CodeAProgress()
        {
            r_Bits = new BitVector32(0);
        }

        internal void IncrementSortie()
        {
            if (State != QuestState.Progress) return;

            Sortie++;

            Update();

            RecordManager.Instance.Quest.UpdateProgress(this);
        }
        internal void Process(BattleData rpBattle)
        {
            if (State != QuestState.Progress) return;

            if (!rpBattle.QuestCounter.Contains(ID) && rpBattle.IsBossBattle)
            {
                BossBattle++;
                rpBattle.QuestCounter.Add(ID);
            }

            if (!rpBattle.QuestCounter.Contains(-ID) && rpBattle.Rank >= BattleRank.S)
            {
                SWin++;
                rpBattle.QuestCounter.Add(-ID);
            }
            if (!rpBattle.QuestCounter.Contains(-ID - 1) && rpBattle.IsBossBattle && rpBattle.Rank >= BattleRank.B)
            {
                BossBattleWin++;
                rpBattle.QuestCounter.Add(-ID - 1);
            }

            Update();

            RecordManager.Instance.Quest.UpdateProgress(this);
        }

        internal override void Update()
        {
            var rPercent = 0.0;
            rPercent += Math.Min(Sortie / (double)SortieTotal, 1.0) * 25.0;
            rPercent += Math.Min(SWin / (double)SWinTotal, 1.0) * 25.0;
            rPercent += Math.Min(BossBattle / (double)BossBattleTotal, 1.0) * 25.0;
            rPercent += Math.Min(BossBattleWin / (double)BossBattleWinTotal, 1.0) * 25.0;

            Percent = rPercent;
            UpdateTime = DateTimeOffset.Now;
        }
    }
}
