using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

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

            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "SELECT MAX(COUNT(item1), COUNT(item2)) FROM expedition WHERE item1 = -1 OR item2 = -1";
                var rCount = (long)rCommand.ExecuteScalar();
                if (rCount > 0L)
                    CorrectItemID();
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

        void CorrectItemID()
        {
            using (var rTranscation = Connection.BeginTransaction())
            {
                using (var rGetExpeditionCommand = Connection.CreateCommand())
                {
                    rGetExpeditionCommand.CommandText = "SELECT DISTINCT expedition FROM expedition WHERE item1 = -1 OR item2 = -1";
                    using (var rReader = rGetExpeditionCommand.ExecuteReader())
                        while (rReader.Read())
                        {
                            var rExpedition = rReader.GetInt32(0);
                            var rExpeditionInfo = KanColleGame.Current.Base.Expeditions[rExpedition];

                            using (var rCorrectCommand = Connection.CreateCommand())
                            {
                                var rSQLBuilder = new StringBuilder(128);

                                if (rExpeditionInfo.GetItem2[0] == 0)
                                    rSQLBuilder.Append("UPDATE expedition SET item1 = @item1 WHERE expedition = @expedition AND item1 = -1");
                                else
                                {
                                    rSQLBuilder.Append("UPDATE expedition SET item1 = @item1, item2 = @item2 WHERE expedition = @expedition AND item2 NOTNULL;");
                                    rSQLBuilder.Append("UPDATE expedition SET item2 = @item2, item2_count = item1_count, item1 = NULL, item1_count = NULL WHERE expedition = @expedition AND item1 = -1 AND item2 IS NULL;");
                                    rCorrectCommand.Parameters.Add(new SQLiteParameter("@item2", rExpeditionInfo.GetItem2[0]));
                                }

                                rCorrectCommand.CommandText = rSQLBuilder.ToString();
                                rCorrectCommand.Parameters.Add(new SQLiteParameter("@item1", rExpeditionInfo.GetItem1[0]));
                                rCorrectCommand.Parameters.Add(new SQLiteParameter("@expedition", rExpedition));
                                rCorrectCommand.ExecuteNonQuery();
                            }
                        }
                }

                rTranscation.Commit();
            }
        }

        public List<Item> GetRecords()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "SELECT * FROM expedition ORDER BY time DESC";
                using (var rReader = rCommand.ExecuteReader())
                {
                    var rResult = new List<Item>(rReader.VisibleFieldCount);

                    while (rReader.Read())
                    {
                        var rRecord = new Item()
                        {
                            Time = DateTimeUtil.FromUnixTime((ulong)rReader.GetInt64(0)).LocalDateTime.ToString(),
                            Result = (ExpeditionResult)rReader.GetInt32(1),
                            ExpeditionName = KanColleGame.Current.Base.Expeditions[rReader.GetInt32(2)].Name,
                        };
                        if (rRecord.Result != ExpeditionResult.Failure)
                        {
                            rRecord.Fuel = rReader.GetInt32(3);
                            rRecord.Bullet = rReader.GetInt32(4);
                            rRecord.Steel = rReader.GetInt32(5);
                            rRecord.Bauxite = rReader.GetInt32(6);
                        }

                        if (!rReader.IsDBNull(7))
                        {
                            rRecord.Item1Name = KanColleGame.Current.Base.UseItems[rReader.GetInt32(7)].Name;
                            rRecord.Item1Count = rReader.GetInt32(8);
                        }
                        if (!rReader.IsDBNull(9))
                        {
                            rRecord.Item2Name = KanColleGame.Current.Base.UseItems[rReader.GetInt32(9)].Name;
                            rRecord.Item2Count = rReader.GetInt32(10);
                        }

                        rResult.Add(rRecord);
                    }

                    return rResult;
                }
            }
        }

        public class Item
        {
            public string Time { get; set; }
            public ExpeditionResult Result { get; set; }
            public string ExpeditionName { get; set; }
            public int? Fuel { get; set; }
            public int? Bullet { get; set; }
            public int? Steel { get; set; }
            public int? Bauxite { get; set; }
            public string Item1Name { get; set; }
            public int? Item1Count { get; set; }
            public string Item2Name { get; set; }
            public int? Item2Count { get; set; }
        }
    }
}
