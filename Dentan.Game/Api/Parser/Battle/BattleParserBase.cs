using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.QuestData;
using Moen.KanColle.Dentan.Data.Raw;
using Newtonsoft.Json;
using System.Linq;

namespace Moen.KanColle.Dentan.Api.Parser.Battle
{
    abstract class BattleParserBase<T> : ApiParser<T>
        where T : RawBattleBase
    {
        protected BattleData Battle { get; set; }

        protected BattleStatus[] AllStatus { get; set; }

        public override void Process(T rpData)
        {
            Battle = Game.Battle ?? new BattleData();

            var rJson = ResponseJson.ToString(Formatting.None);
            if (this is DayBattleParserBase)
                Battle.FirstBattleJson = rJson;
            else
                Battle.SecondBattleJson = rJson;

            ParseEnemyInfo(rpData);
            ParseStatus(rpData);
            ProcessCore(rpData);
            PostProcess(rpData);

            Game.Battle = Battle;

            ProcessQuest(Battle);

            Battle = null;
        }

        protected abstract void ParseEnemyInfo(T rpData);
        protected abstract void ParseStatus(T rpData);
        protected abstract void ProcessCore(T rpData);
        protected abstract void PostProcess(T rpData);
        
        protected void ParseEnemyInfoCore(T rpData)
        {
            var rEnemyIDs = rpData.EnemyIDs.TakeWhile(r => r != -1).ToArray();
            //for (var i = 0; i < rEnemyIDs.Length; i++)
            //{
            //    var rEnemyID = rEnemyIDs[i];
            //    var rShip = AbyssalShip.FromID(rEnemyID);
            //    if (rShip == null)
            //        rShip = new AbyssalShip() { ID = rEnemyID, IsDirty = true };

            //    var rEquipmentIDs = rpData.EnemyEquipments[i].TakeWhile(r => r != -1).ToArray();
            //    if (rShip.EquipmentIDs == null || !rShip.EquipmentIDs.SequenceEqual(rEquipmentIDs))
            //    {
            //        rShip.EquipmentIDs = rEquipmentIDs;
            //        rShip.IsDirty = true;
            //    }
            //}

            var rEnemyFleet = Game.CompassData.EnemyFleet ?? (Game.CompassData.EnemyFleet = new EnemyFleet());
            if (rEnemyFleet != null)
            {
                if (rEnemyFleet.ShipIDs == null || !rEnemyFleet.ShipIDs.SequenceEqual(rEnemyIDs))
                {
                    rEnemyFleet.ShipIDs = rEnemyIDs;
                    rEnemyFleet.Formation = rpData.Formation.Enemy;
                    rEnemyFleet.UpdateShips();
                }

                if (!rEnemyFleet.HasEquipments)
                {
                    var rShips = rEnemyFleet.Ships;
                    for (var i = 0; i < rShips.Length; i++)
                    {
                        var rSlots = rShips[i].Slots;
                        for (var j = 0; j < rSlots.Length; j++)
                        {
                            var rID = rpData.EnemyEquipments[i][j];
                            rSlots[j].Equipment = rID != -1 ? Equipment.GetDummyFromID(rID) : Equipment.Default;
                        }
                    }

                    rEnemyFleet.UpdateAA();
                    rEnemyFleet.HasEquipments = true;
                }
            }
        }

        protected virtual void ProcessQuest(BattleData rpData)
        {
            foreach (var rProgress in Quest.Progresses.Values.OfType<DestroyTargetProgress>())
                rProgress.ProcessDay(rpData);
            foreach (var rProgress in Quest.Progresses.Values.OfType<BossTargetProgress>())
                rProgress.Process(rpData);
            foreach (var rProgress in Quest.Progresses.Values.OfType<DailyBattleProgress>())
                rProgress.Current++;
            ((CodeAProgress)Quest.Progresses[214]).Process(rpData);
        }

        protected void ProcessShelling(RawShelling rpData)
        {
            if (rpData == null) return;

            var rFrom = rpData.ActionOrder;
            var rTargetList = rpData.Defense;
            var rDamageList = rpData.Damage;
            for (var i = 0; i < rFrom.Length; i++)
            {
                var rTarget = rTargetList[i];
                for (var j = 0; j < rTarget.Length; j++)
                {
                    if (rTarget[j] == -1)
                        continue;

                    var rDamage = rDamageList[i][j];
                    if (rFrom[i] <= 6)
                        AllStatus[rFrom[i] - 1].GivenDamage += rDamage;
                    AllStatus[rTarget[j] - 1].NowHP -= rDamage;
                }
            }
        }
    }
}
