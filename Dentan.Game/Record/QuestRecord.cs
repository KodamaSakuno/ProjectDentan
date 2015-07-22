using Moen.KanColle.Dentan.Data;
using System.Data.SQLite;

namespace Moen.KanColle.Dentan.Record
{
    public class QuestRecord : RecordBase
    {
        public bool IsInitialized { get; private set; }

        internal QuestRecord(SQLiteConnection rpConnection)
            : base(rpConnection) { }

        internal override void Load()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "CREATE TABLE IF NOT EXISTS quest(" +
                    "id INTEGER PRIMARY KEY ASC, " +
                    "state INTEGER, " +
                    "progress INTEGER DEFAULT 0, " +
                    "update_time INTEGER NOT NULL)";
                rCommand.ExecuteNonQuery();
            }

            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "SELECT * FROM quest";
                using (var rReader = rCommand.ExecuteReader())
                {
                    if (rReader.HasRows)
                        using (var rTransaction = Connection.BeginTransaction())
                        {
                            while (rReader.Read())
                            {
                                var rID = rReader.GetInt32(0);

                                QuestProgressData rData;
                                if (Quest.Progresses.TryGetValue(rID, out rData))
                                {
                                    rData.StateInternal = (QuestState)rReader.GetInt32(1);
                                    rData.CurrentInternal = rReader.GetInt32(2);
                                    rData.UpdateTime = DateTimeUtil.FromUnixTime((ulong)rReader.GetInt64(3));

                                    rData.ResetByDate();
                                }
                            }
                            rTransaction.Commit();
                        }
                }
            }

            IsInitialized = true;
        }

        public void UpdateProgress(QuestProgressData rpProgress)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "INSERT OR IGNORE INTO quest(id) VALUES (@id);" +
                    "UPDATE quest SET progress = @progress WHERE id = @id;" +
                    "UPDATE quest SET update_time = strftime('%s', 'now') WHERE id = @id;";
                rCommand.Parameters.Add(new SQLiteParameter("@id", rpProgress.ID));
                rCommand.Parameters.Add(new SQLiteParameter("@progress", rpProgress.Current));
                rCommand.ExecuteNonQuery();
            }
        }
        public void UpdateStatus(QuestProgressData rpProgress)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "INSERT OR IGNORE INTO quest(id) VALUES (@id);" +
                    "UPDATE quest SET state = @state WHERE id = @id;" +
                    "UPDATE quest SET progress = @startfrom WHERE id = @id AND state = 3;";
                rCommand.Parameters.Add(new SQLiteParameter("@id", rpProgress.ID));
                rCommand.Parameters.Add(new SQLiteParameter("@state", (int)rpProgress.State));
                rCommand.Parameters.Add(new SQLiteParameter("@startfrom", (int)rpProgress.StartFrom));
                rCommand.ExecuteNonQuery();
            }
        }
    }
}
