using Moen.KanColle.Dentan.Data.Raw;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Moen.KanColle.Dentan.Record
{
    public class ExpeditionRecord : RecordBase
    {
        internal ExpeditionRecord(SQLiteConnection rpConnection)
            : base(rpConnection) { }

        internal override void Load()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "CREATE TABLE IF NOT EXISTS expedition_count(" +
                    "ship INTEGER, " +
                    "expedition INTEGER, " +
                    "count INTEGER DEFAULT 0);" +
                    "CREATE UNIQUE INDEX IF NOT EXISTS ship_expedition_index ON expedition_count(ship, expedition);" +

                    "CREATE TABLE IF NOT EXISTS expedition(" +
                    "time INTEGER PRIMARY KEY, " +
                    "result INTEGER, " +
                    "expedition INTEGER, " +
                    "fuel INTEGER, " +
                    "bullet INTEGER, " +
                    "steel INTEGER, " +
                    "bauxite INTEGER, " +
                    "item1 INTEGER, " +
                    "item1_count INTEGER, " +
                    "item2 INTEGER, " +
                    "item2_count INTEGER);";
                rCommand.ExecuteNonQuery();
            }
        }

        internal void UpdateCount(IEnumerable<int> rpShips, int rpExpedition)
        {
            using (var rTransaction = Connection.BeginTransaction())
            {
                foreach (var rShip in rpShips)
                    using (var rCommand = Connection.CreateCommand())
                    {
                        rCommand.CommandText = "INSERT OR IGNORE INTO expedition_count(ship, expedition) VALUES (@ship, @expedition);" +
                            "UPDATE expedition_count SET count = count + 1 WHERE ship = @ship AND expedition = @expedition;";
                        rCommand.Parameters.Add(new SQLiteParameter("@ship", rShip));
                        rCommand.Parameters.Add(new SQLiteParameter("@expedition", rpExpedition));
                        rCommand.ExecuteNonQuery();
                    }

                rTransaction.Commit();
            }
        }

        internal void UpdateExpedition(int rpExpedition, RawExpeditionResult rpData)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                var rMaterial = rpData.Result != 0 ? rpData.Material.ToObject<int[]>() : null;

                rCommand.CommandText = "INSERT INTO expedition(time, result, expedition, fuel, bullet, steel, bauxite, item1, item1_count, item2, item2_count) " +
                    "VALUES (strftime('%s', 'now'), @result, @expedition, @fuel, @bullet, @steel, @bauxite, @item1, @item1_count, @item2, @item2_count)";
                rCommand.Parameters.Add(new SQLiteParameter("@result", (int)rpData.Result));
                rCommand.Parameters.Add(new SQLiteParameter("@expedition", rpExpedition));
                rCommand.Parameters.Add(new SQLiteParameter("@fuel", rMaterial != null ? rMaterial[0] : (int?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@bullet", rMaterial != null ? rMaterial[1] : (int?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@steel", rMaterial != null ? rMaterial[2] : (int?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@bauxite", rMaterial != null ? rMaterial[3] : (int?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@item1", rpData.Item1 != null ? rpData.Item1.ID : (int?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@item1_count", rpData.Item1 != null ? rpData.Item1.Count : (int?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@item2", rpData.Item2 != null ? rpData.Item2.ID : (int?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@item2_count", rpData.Item2 != null ? rpData.Item2.Count : (int?)null));
                rCommand.ExecuteNonQuery();
            }
        }
    }
}
