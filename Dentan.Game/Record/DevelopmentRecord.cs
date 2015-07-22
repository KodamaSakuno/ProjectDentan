using Moen.KanColle.Dentan.Data.Raw;
using System.Data.SQLite;

namespace Moen.KanColle.Dentan.Record
{
    public class DevelopmentRecord : RecordBase
    {
        internal DevelopmentRecord(SQLiteConnection rpConnection)
            : base(rpConnection) { }

        internal override void Load()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "CREATE TABLE IF NOT EXISTS development(" +
                    "time INTEGER PRIMARY KEY, " +
                    "equipment INTEGER, " +
                    "fuel INTEGER, " +
                    "bullet INTEGER, " +
                    "steel INTEGER, " +
                    "bauxite INTEGER, " +
                    "flagship INTEGER, " +
                    "headquarter_level INTEGER)";
                rCommand.ExecuteNonQuery();
            }
        }

        public void Update(RawCreateEquipment rpData, int rpFuel, int rpBullet, int rpSteel, int rpBauxite)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "INSERT INTO development(time, equipment, fuel, bullet, steel, bauxite, flagship, headquarter_level) " +
                    "VALUES (strftime('%s', 'now'), @equipment, @fuel, @bullet, @steel, @bauxite, @flagship, @headquarter_level)";
                rCommand.Parameters.Add(new SQLiteParameter("@equipment", rpData.Success ? rpData.Result.EquipmentID : (int?)null));
                rCommand.Parameters.Add(new SQLiteParameter("@fuel", rpFuel));
                rCommand.Parameters.Add(new SQLiteParameter("@bullet", rpBullet));
                rCommand.Parameters.Add(new SQLiteParameter("@steel", rpSteel));
                rCommand.Parameters.Add(new SQLiteParameter("@bauxite", rpBauxite));
                rCommand.Parameters.Add(new SQLiteParameter("@flagship", KanColleGame.Current.Fleets[1].Ships[0].ShipID));
                rCommand.Parameters.Add(new SQLiteParameter("@headquarter_level", KanColleGame.Current.Headquarter.Admiral.Level));
                rCommand.ExecuteNonQuery();
            }
        }
    }
}
