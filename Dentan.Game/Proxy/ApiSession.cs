using Newtonsoft.Json.Linq;
using System;

namespace Moen.KanColle.Dentan.Proxy
{
    public class ApiSession : Session
    {
        public string RequestBody { get; set; }

        public string ResponseString { get; set; }

        WeakReference<JObject> r_ResponseJson = new WeakReference<JObject>(null);
        public JObject ResponseJson
        {
            get
            {
                try
                {
                    JObject rResult;
                    if (!r_ResponseJson.TryGetTarget(out rResult))
                        r_ResponseJson.SetTarget(JObject.Parse(ResponseString));
                    return rResult;
                }
                catch { return null; }
            }
        }

        public ApiSession(string rpUrl)
            : base(rpUrl) { }
        public ApiSession(string rpFullUrl, string rpUrl) : base(rpFullUrl, rpUrl) { }
    }
}
