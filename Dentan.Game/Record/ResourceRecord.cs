using Moen.KanColle.Dentan.Data;
using System.Data.SQLite;

namespace Moen.KanColle.Dentan.Record
{
    public class ResourceRecord : RecordBase
    {
        internal ResourceRecord(SQLiteConnection rpConnection)
            : base(rpConnection) { }

        internal override void Load()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "CREATE TABLE IF NOT EXISTS resources("+
                    "time INTEGER PRIMARY KEY, " +
                    "fuel INTEGER, "+
                    "bullet INTEGER, "+
                    "steel INTEGER, "+
                    "bauxite INTEGER, "+
                    "development_material INTEGER, " + 
                    "bucket INTEGER, " +
                    "instant_construction INTEGER, " +
                    "improvement_material INTEGER)";
                rCommand.ExecuteNonQuery();
            }

            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "SELECT * FROM resources ORDER BY time DESC LIMIT 1";
                using (var rReader = rCommand.ExecuteReader())
                    if (rReader.Read())
                    {
                        var rMaterial = KanColleGame.Current.Headquarter.Material;
                        rMaterial.Fuel = rReader.GetInt32(1);
                        rMaterial.Bullet = rReader.GetInt32(2);
                        rMaterial.Steel = rReader.GetInt32(3);
                        rMaterial.Bauxite = rReader.GetInt32(4);
                        rMaterial.DevelopmentMaterial = rReader.GetInt32(5);
                        rMaterial.Bucket = rReader.GetInt32(6);
                        rMaterial.InstantConstruction = rReader.GetInt32(7);
                        rMaterial.ImprovementMaterial = rReader.GetInt32(8);

                        rMaterial.IsDirty = false;
                    }
            }
        }

        public void Update(Material rpData)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "INSERT INTO resources(time, fuel, bullet, steel, bauxite, development_material, bucket, instant_construction, improvement_material) " +
                    "VALUES (strftime('%s', 'now'), @fuel, @bullet, @steel, @bauxite, @development_material, @bucket, @instant_construction, @improvement_material)";
                rCommand.Parameters.Add(new SQLiteParameter("@fuel", rpData.Fuel));
                rCommand.Parameters.Add(new SQLiteParameter("@bullet", rpData.Bullet));
                rCommand.Parameters.Add(new SQLiteParameter("@steel", rpData.Steel));
                rCommand.Parameters.Add(new SQLiteParameter("@bauxite", rpData.Bauxite));
                rCommand.Parameters.Add(new SQLiteParameter("@development_material", rpData.DevelopmentMaterial));
                rCommand.Parameters.Add(new SQLiteParameter("@bucket", rpData.Bucket));
                rCommand.Parameters.Add(new SQLiteParameter("@instant_construction", rpData.InstantConstruction));
                rCommand.Parameters.Add(new SQLiteParameter("@improvement_material", rpData.ImprovementMaterial));
                rCommand.ExecuteNonQuery();
            }
        }
    }
}
