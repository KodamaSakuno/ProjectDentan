using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.Data
{
    public static class AbyssalDataManager
    {
        const string Data = @"Data\AbyssalData.json";
        static Encoding Encoding = new UTF8Encoding(true);

        public static Table<AbyssalFleet> Fleets { get; private set; }
        public static Table<AbyssalShip> Ships { get; private set; }
        public static Dictionary<int, int[]> ShipsPlaneCount { get; private set; }

        static AbyssalDataManager()
        {
            if (!File.Exists(Data))
            {
                Fleets = new Table<AbyssalFleet>();
                Ships = new Table<AbyssalShip>();
            }
            else
            {
                using (var rReader = File.OpenText(Data))
                {
                    var rData = JObject.Load(new JsonTextReader(rReader));
                    Fleets = new Table<AbyssalFleet>(rData["fleets"].ToObject<AbyssalFleet[]>());
                    //Ships = new Table<AbyssalShip>(rData["ships"].ToObject<AbyssalShip[]>());
                    Ships = new Table<AbyssalShip>();
                    ShipsPlaneCount = rData["ships"]?.Where(r => r["plane_count"] != null).ToDictionary(r => (int)r["id"], r => r["plane_count"].ToObject<int[]>());
                }
            }

        }
        internal static void Update()
        {
            var rDirtyFleets = Fleets.Values.Where(r => r.IsDirty).ToArray();
            if (rDirtyFleets.Length == 0)
                return;

            foreach (var rFleet in rDirtyFleets)
                rFleet.IsDirty = false;

            using (var rWriter = new StreamWriter(Data, false, Encoding))
            using (var rJsonWriter = new JsonTextWriter(rWriter))
            {
                var rJsonSerializer = new JsonSerializer();
                rJsonSerializer.Serialize(rJsonWriter, new
                {
                    fleets = Fleets.Values.OrderBy(r => r.ID),
                    ships = ShipsPlaneCount.OrderBy(r => r.Key).Select(r => new { id = r.Key, plane_count = r.Value }),
                });
            }

            Task.Run(() =>
            {
                var rData = new { fleets = rDirtyFleets };
                var rRequest = WebRequest.CreateHttp("http://api.sakuno.moe/pd/abyssal_data");
                rRequest.Method = "POST";
                rRequest.ContentType = "application/json";
                rRequest.UserAgent = "Project Dentan 0.0.1.9";
                
                var rBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rData));
                rRequest.ContentLength = rBytes.Length;
                var rRequestStream = rRequest.GetRequestStream();
                rRequestStream.Write(rBytes, 0, rBytes.Length);
                rRequestStream.Close();
                
                try
                {
                    using (var rResponse = rRequest.GetResponse())
                    {

                    }
                }
                catch (WebException e)
                {
                    var rResponse = e.Response as HttpWebResponse;
                    using (var rReader = new StreamReader(rResponse.GetResponseStream()))
                    {
                        var rContent = rReader.ReadToEnd();
                    }
                }
            });
        }
    }
}
