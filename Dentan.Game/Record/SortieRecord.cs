using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Moen.KanColle.Dentan.Record
{
    public class SortieRecord : RecordBase
    {
        internal SortieRecord(SQLiteConnection rpConnection)
            : base(rpConnection) { }

        internal override void Load()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'ship_drop'";
                using (var rReader = rCommand.ExecuteReader())
                    if (rReader.HasRows)
                        using (var rChangeTableNameCommand = Connection.CreateCommand())
                        {
                            rChangeTableNameCommand.CommandText = "ALTER TABLE ship_drop RENAME TO sortie";
                            rChangeTableNameCommand.ExecuteNonQuery();
                        }
            }
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "CREATE TABLE IF NOT EXISTS sortie(" +
                    "time INTEGER PRIMARY KEY, " +
                    "map INTEGER, " +
                    "cell INTEGER, " +
                    "rank INTEGER, " +
                    "is_dropped BOOLEAN, " +
                    "ship INTEGER);";
                rCommand.ExecuteNonQuery();
            }
        }

        internal void Update(AbyssalFleet rpFleet, RawBattleResult rpResult)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                var rCompassData = KanColleGame.Current.CompassData;

                BattleRank rRank;
                Enum.TryParse<BattleRank>(rpResult.Rank, out rRank);

                rCommand.CommandText = "INSERT INTO sortie(time, map, cell, rank, is_dropped, ship) " +
                    "VALUES (@time, @map, @cell, @rank, @is_dropped, @ship)";
                rCommand.Parameters.Add(new SQLiteParameter("@time", DateTimeUtil.ToUnixTime(KanColleGame.Current.Battle.Time)));
                rCommand.Parameters.Add(new SQLiteParameter("@map", rCompassData.MapID));
                rCommand.Parameters.Add(new SQLiteParameter("@cell", rCompassData.Cell));
                rCommand.Parameters.Add(new SQLiteParameter("@rank", (int)rRank));
                rCommand.Parameters.Add(new SQLiteParameter("@is_dropped", KanColleGame.Current.Ships.Count != KanColleGame.Current.Headquarter.Admiral.MaxShipCount ? rpResult.Drop[1] : (bool?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@ship", rpResult.Drop[1] ? rpResult.DropShip.ID : (int?)null));
                rCommand.ExecuteNonQuery();
            }
        }

        public List<Item> GetSortieRecords()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "SELECT * FROM sortie x LEFT JOIN battle.sortie y ON x.time = y.time ORDER BY time DESC";
                using (var rReader = rCommand.ExecuteReader())
                {
                    var rResult = new List<Item>(rReader.VisibleFieldCount);

                    while (rReader.Read())
                    {
                        var rRecord = new Item()
                        {
                            Time = DateTimeUtil.FromUnixTime(Convert.ToUInt64(rReader["time"])).LocalDateTime.ToString(),
                            Map = Convert.ToInt32(rReader["map"]),
                            Cell = Convert.ToInt32(rReader["cell"]),
                            Rank = (BattleRank)Convert.ToInt32(rReader["rank"]),
                            HasDetail = rReader["friend"] != DBNull.Value,
                        };
                        var rIsDropped = rReader["is_dropped"];
                        rRecord.IsDropped = rIsDropped != DBNull.Value ? Convert.ToBoolean(rIsDropped) : (bool?)null;
                            
                        if (rRecord.IsDropped.HasValue && rRecord.IsDropped.Value)
                            rRecord.DroppedShip = KanColleGame.Current.Base.Ships[Convert.ToInt32(rReader["ship"])].Name;

                        rResult.Add(rRecord);
                    }

                    return rResult;
                }
            }
        }

        public class Item
        {
            public string Time { get; set; }
            public int Map { get; set; }
            public int Cell { get; set; }
            public BattleRank Rank { get; set; }
            public bool? IsDropped { get; set; }
            public string DroppedShip { get; set; }
            public bool HasDetail { get; set; }
        }
    }
}
