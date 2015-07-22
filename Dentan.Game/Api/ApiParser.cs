using Moen.KanColle.Dentan.Proxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moen.KanColle.Dentan.Api
{
    public abstract class ApiParserBase
    {
        public KanColleGame Game { get { return KanColleGame.Current; } }
        public GameProxy Proxy { get { return Game.Proxy; } }

        public string Path { get; internal set; }
        public IReadOnlyDictionary<string, string> Request { get; internal set; }
        public JObject ResponseJson { get; private set; }

        public int ResultCode { get; private set; }
        public string ResultMessage { get; private set; }

        internal virtual void Process(JObject rpJson)
        {
            ResultCode = (int)rpJson["api_result"];
            ResultMessage = (string)rpJson["api_result_msg"];
            ResponseJson = rpJson;
        }

        protected void SaveResponseBody()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            string rPath;
            if (Path == "api_start2")
                rPath = "start";
            else
                rPath = Path.Substring(Path.LastIndexOf('/') + 1);

            File.WriteAllText(@"Data\" + rPath, ResponseJson.ToString(Formatting.Indented), new UTF8Encoding(true));
        }
    }

    public abstract class ApiParser : ApiParserBase
    {
        internal override sealed void Process(JObject rpJson)
        {
            base.Process(rpJson);
            Process();
        }
        public abstract void Process();
    }
    public abstract class ApiParser<T> : ApiParserBase
    {
        internal override sealed void Process(JObject rpJson)
        {
            base.Process(rpJson);
            try
            {
                T rData = default(T);

                var rApiData = rpJson["api_data"];
                if (rApiData != null)
                    rData = rApiData.ToObject<T>();

                Process(rData);
            }
            catch (JsonReaderException) { }
        }
        public abstract void Process(T rpData);
    }
}
