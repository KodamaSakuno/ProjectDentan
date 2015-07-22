using System.Data.SQLite;

namespace Moen.KanColle.Dentan.Record
{
    public class ExperienceRecord : RecordBase
    {
        internal ExperienceRecord(SQLiteConnection rpConnection)
            : base(rpConnection) { }

        internal override void Load()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "CREATE TABLE IF NOT EXISTS ship_experience(" +
                    "ship INTEGER, " +
                    "experience INTEGER, " +
                    "time INTEGER DEFAULT (strftime('%s', 'now')));" +

                    "CREATE TABLE IF NOT EXISTS admiral_experience (" +
                    "time INTEGER PRIMARY KEY, " +
                    "experience INTEGER);";
                rCommand.ExecuteNonQuery();
            }
        }

        public void AddShipExpData(int rpID, int rpExperience)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "INSERT INTO ship_experience(ship, experience) " +
                    "VALUES (@ship, @experience)";
                rCommand.Parameters.Add(new SQLiteParameter("@ship", rpID));
                rCommand.Parameters.Add(new SQLiteParameter("@experience", rpExperience));
                rCommand.ExecuteNonQuery();
            }
        }
        public void AddAdmiralExpData(int rpExperience)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "INSERT INTO admiral_experience(time, experience) " +
                    "VALUES (strftime('%s', 'now'), @experience)";
                rCommand.Parameters.Add(new SQLiteParameter("@experience", rpExperience));
                rCommand.ExecuteNonQuery();
            }
        }
    }
}
