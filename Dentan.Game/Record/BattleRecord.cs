using Moen.KanColle.Dentan.Data;
using System;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;

namespace Moen.KanColle.Dentan.Record
{
    public class BattleRecord : RecordBase
    {
        internal BattleRecord(SQLiteConnection rpConnection)
            : base(rpConnection) { }

        internal override void Load()
        {
        }

        internal void Update(CompassData rpCompassData, BattleData rpBattle, string rpRank)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                BattleRank rRank;
                Enum.TryParse<BattleRank>(rpRank, out rRank);

                rCommand.CommandText = "INSERT INTO sortie(time, map, cell, rank, first, second) " +
                    "VALUES (@time, @map, @cell, @rank, @first, @second)";
                rCommand.Parameters.AddWithValue("@time", DateTimeUtil.ToUnixTime(rpBattle.Time));
                rCommand.Parameters.AddWithValue("@map", rpCompassData.MapID);
                rCommand.Parameters.AddWithValue("@cell", rpCompassData.Cell);
                rCommand.Parameters.AddWithValue("@rank", (int)rRank);
                using (var rMemoryStream = new MemoryStream())
                {
                    using (var rCompressStream = new GZipStream(rMemoryStream, CompressionMode.Compress))
                    using (var rWriter = new StreamWriter(rCompressStream))
                        rWriter.Write(rpBattle.FirstBattleJson);
                    rCommand.Parameters.AddWithValue("@first", rMemoryStream.ToArray());
                }

                byte[] rSecond = null;
                if (!rpBattle.SecondBattleJson.IsNullOrEmpty())
                    using (var rMemoryStream = new MemoryStream())
                    {
                        using (var rCompressStream = new GZipStream(rMemoryStream, CompressionMode.Compress))
                        using (var rWriter = new StreamWriter(rCompressStream))
                            rWriter.Write(rpBattle.SecondBattleJson);

                        rSecond = rMemoryStream.ToArray();
                    }
                rCommand.Parameters.AddWithValue("@second", rSecond);

                rCommand.ExecuteNonQuery();
            }
        }
    }
}
