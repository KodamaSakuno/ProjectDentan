using Moen.KanColle.Dentan.Data;
using System.Data.SQLite;
using System.Linq;

namespace Moen.KanColle.Dentan.Record
{
    public class ConstructionRecord : RecordBase
    {
        internal ConstructionRecord(SQLiteConnection rpConnection)
            : base(rpConnection) { }

        internal override void Load()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "CREATE TABLE IF NOT EXISTS construction(" +
                    "time INTEGER PRIMARY KEY, " +
                    "ship INTEGER, " +
                    "fuel INTEGER, " +
                    "bullet INTEGER, " +
                    "steel INTEGER, " +
                    "bauxite INTEGER, " +
                    "development_material INTEGER, " +
                    "flagship INTEGER, " +
                    "headquarter_level INTEGER, " +
                    "is_lsc BOOLEAN," +
                    "empty_dock INTEGER)";
                rCommand.ExecuteNonQuery();
            }
        }

        public void Update(BuildingDock rpData)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "INSERT INTO construction(time, ship, fuel, bullet, steel, bauxite, development_material, flagship, headquarter_level, is_lsc, empty_dock) " +
                    "VALUES (strftime('%s', 'now'), @ship, @fuel, @bullet, @steel, @bauxite, @development_material, @flagship, @headquarter_level, @is_lsc, @empty_dock)";
                rCommand.Parameters.Add(new SQLiteParameter("@ship", rpData.Ship.ID));
                rCommand.Parameters.Add(new SQLiteParameter("@fuel", rpData.Fuel));
                rCommand.Parameters.Add(new SQLiteParameter("@bullet", rpData.Bullet));
                rCommand.Parameters.Add(new SQLiteParameter("@steel", rpData.Steel));
                rCommand.Parameters.Add(new SQLiteParameter("@bauxite", rpData.Bauxite));
                rCommand.Parameters.Add(new SQLiteParameter("@development_material", rpData.DevelopmentMaterial));
                rCommand.Parameters.Add(new SQLiteParameter("@flagship", KanColleGame.Current.Fleets[1].Ships[0].ShipID));
                rCommand.Parameters.Add(new SQLiteParameter("@headquarter_level", KanColleGame.Current.Headquarter.Admiral.Level));
                rCommand.Parameters.Add(new SQLiteParameter("@is_lsc", rpData.IsLargeShipConstruction));

                var rEmptyDocks = !rpData.IsLargeShipConstruction.Value ? (int?)null : KanColleGame.Current.BuildingDocks.Values.Count(r => r.State == BuildingDockState.Idle);
                rCommand.Parameters.Add(new SQLiteParameter("@empty_dock", rEmptyDocks));
                rCommand.ExecuteNonQuery();
            }
        }
    }
}
