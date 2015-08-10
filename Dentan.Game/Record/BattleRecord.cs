using Moen.KanColle.Dentan.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Moen.KanColle.Dentan.Record
{
    public class BattleRecord : RecordBase
    {
        static HashSet<int> r_HistoryDisabledMap = new HashSet<int>(new[] { 11, 12, 13, 14, 21, 22, 23, 31, 32, 33, 41, 42 });

        internal BattleRecord(SQLiteConnection rpConnection)
            : base(rpConnection) { }

        internal override void Load()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "CREATE TABLE IF NOT EXISTS battle.sortie(" +
                    "time INTEGER PRIMARY KEY, " +
                    "friend BLOB, " +
                    "first BLOB, " +
                    "second BLOB);" +
                    "CREATE TABLE IF NOT EXISTS battle.practice(" +
                    "time INTEGER PRIMARY KEY, " +
                    "opponent TEXT, " +
                    "opponent_level INTEGER, " +
                    "opponent_rank INTEGER, " +
                    "opponent_comment TEXT, " +
                    "opponent_fleet TEXT, " +
                    "rank INTEGER, " +
                    "friend BLOB, " +
                    "first BLOB, " +
                    "second BLOB);";
                rCommand.ExecuteNonQuery();
            }
        }

        internal void UpdateSortie(CompassData rpCompassData, BattleData rpBattle, string rpRank)
        {
            if (r_HistoryDisabledMap.Contains(rpCompassData.MapID))
                return;

            using (var rCommand = Connection.CreateCommand())
            {
                BattleRank rRank;
                Enum.TryParse<BattleRank>(rpRank, out rRank);

                rCommand.CommandText = "INSERT INTO battle.sortie(time, friend, first, second) " +
                    "VALUES (@time, @friend, @first, @second)";
                rCommand.Parameters.AddWithValue("@time", DateTimeUtil.ToUnixTime(rpBattle.Time));
                rCommand.Parameters.AddWithValue("@friend", GetParticipatedFleets(rpBattle.ParticipatedFleetIDs));
                rCommand.Parameters.AddWithValue("@first", CompressString(rpBattle.FirstBattleJson));
                rCommand.Parameters.AddWithValue("@second", !rpBattle.SecondBattleJson.IsNullOrEmpty() ? CompressString(rpBattle.SecondBattleJson) : null);

                rCommand.ExecuteNonQuery();
            }
        }
        internal void UpdatePractice(CompassData rpCompassData, BattleData rpBattle, string rpRank)
        {
            using (var rCommand = Connection.CreateCommand())
            {
                BattleRank rRank;
                Enum.TryParse<BattleRank>(rpRank, out rRank);

                rCommand.CommandText = "INSERT INTO battle.practice(time, opponent, opponent_level, opponent_rank, opponent_comment, opponent_fleet, rank, friend, first, second) " +
                    "VALUES (@time, @opponent, @opponent_level, @opponent_rank, @opponent_comment, @opponent_fleet, @rank, @friend, @first, @second)";
                rCommand.Parameters.AddWithValue("@time", DateTimeUtil.ToUnixTime(rpBattle.Time));
                rCommand.Parameters.AddWithValue("@opponent", rpCompassData.OpponentInfo.Name);
                rCommand.Parameters.AddWithValue("@opponent_level", rpCompassData.OpponentInfo.Level);
                rCommand.Parameters.AddWithValue("@opponent_rank", rpCompassData.OpponentInfo.Rank);
                rCommand.Parameters.AddWithValue("@opponent_comment", rpCompassData.OpponentInfo.Comment);
                rCommand.Parameters.AddWithValue("@opponent_fleet", rpCompassData.OpponentInfo.FleetName);
                rCommand.Parameters.AddWithValue("@rank", (int)rRank);
                rCommand.Parameters.AddWithValue("@friend", GetParticipatedFleets(rpBattle.ParticipatedFleetIDs));
                rCommand.Parameters.AddWithValue("@first", CompressString(rpBattle.FirstBattleJson));
                rCommand.Parameters.AddWithValue("@second", !rpBattle.SecondBattleJson.IsNullOrEmpty() ? CompressString(rpBattle.SecondBattleJson) : null);

                rCommand.ExecuteNonQuery();
            }
        }

        static byte[] CompressString(string rpString)
        {
            using (var rMemoryStream = new MemoryStream())
            {
                using (var rCompressStream = new GZipStream(rMemoryStream, CompressionMode.Compress))
                using (var rWriter = new StreamWriter(rCompressStream))
                    rWriter.Write(rpString);

                return rMemoryStream.ToArray();
            }
        }
        static byte[] GetParticipatedFleets(IEnumerable<int> rpParticipatedFleetIDs)
        {
            var rData = rpParticipatedFleetIDs.Select(r => KanColleGame.Current.Fleets[r]).Select(r => new
            {
                id = r.ID,
                name = r.Name,
                ships = r.Ships.Select(rpShip => new
                {
                    id = rpShip.ID,
                    ship_id = rpShip.ShipID,
                    level = rpShip.Level,
                    fuel = rpShip.Fuel,
                    bullet = rpShip.Bullet,
                    condition = rpShip.Condition,
                    slots = rpShip.Slots.Select(rpSlot => new
                    {
                        equipment = rpSlot.Equipment.RawData.EquipmentID,
                        level = rpSlot.Equipment.Level,
                        plane_count = rpSlot.PlaneCount,
                    }).ToArray(),
                    paramater = new
                    {
                        firepower = rpShip.RawData.FirePower[0],
                        tropedo = rpShip.RawData.Torpedo[0],
                        aa = rpShip.RawData.AA[0],
                        armor = rpShip.RawData.Armor[0],
                        evasion = rpShip.RawData.Evasion[0],
                        asw = rpShip.RawData.ASW[0],
                        los = rpShip.LoS,
                        luck = rpShip.RawData.Luck[0],
                        range = rpShip.RawData.Range,
                        speed = rpShip.Info.Speed,
                    },
                }).ToArray(),
            }).OrderBy(r => r.id);

            using (var rMemoryStream = new MemoryStream())
            {
                using (var rCompressStream = new GZipStream(rMemoryStream, CompressionMode.Compress))
                using (var rWriter = new StreamWriter(rCompressStream))
                using (var rJsonTextWriter = new JsonTextWriter(rWriter))
                    new JsonSerializer().Serialize(rJsonTextWriter, rData);

                return rMemoryStream.ToArray();
            }
        }

        public List<PracticeItem> GetPracticeRecords()
        {
            using (var rCommand = Connection.CreateCommand())
            {
                rCommand.CommandText = "SELECT * FROM battle.practice ORDER BY time DESC";
                using (var rReader = rCommand.ExecuteReader())
                {
                    var rResult = new List<PracticeItem>(rReader.VisibleFieldCount);

                    while (rReader.Read())
                        rResult.Add(new PracticeItem()
                        {
                            Time = DateTimeUtil.FromUnixTime(Convert.ToUInt64(rReader["time"])).LocalDateTime.ToString(),
                            Opponent = (string)rReader["opponent"],
                            OpponentLevel = Convert.ToInt32(rReader["opponent_level"]),
                            OpponentComment = (string)rReader["opponent_comment"],
                            OpponentFleet = (string)rReader["opponent_fleet"],
                            Rank = (BattleRank)Convert.ToInt32(rReader["rank"]),
                        });

                    return rResult;
                }
            }
        }

        public class PracticeItem
        {
            public string Time { get; set; }
            public string Opponent { get; set; }
            public int OpponentLevel { get; set; }
            public string OpponentComment { get; set; }
            public string OpponentFleet { get; set; }
            public BattleRank Rank { get; set; }
        }
    }
}
