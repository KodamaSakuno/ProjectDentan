using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System;
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
                    "VALUES (strftime('%s', 'now'), @map, @cell, @rank, @is_dropped, @ship)";
                rCommand.Parameters.Add(new SQLiteParameter("@map", rCompassData.MapID));
                rCommand.Parameters.Add(new SQLiteParameter("@cell", rCompassData.Cell));
                rCommand.Parameters.Add(new SQLiteParameter("@rank", (int)rRank));
                rCommand.Parameters.Add(new SQLiteParameter("@is_dropped", KanColleGame.Current.Ships.Count != KanColleGame.Current.Headquarter.Admiral.MaxShipCount ? rpResult.Drop[1] : (bool?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@ship", rpResult.Drop[1] ? rpResult.DropShip.ID : (int?)null));
                rCommand.ExecuteNonQuery();
            }
        }
    }
}
