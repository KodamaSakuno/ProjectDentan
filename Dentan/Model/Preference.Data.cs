using Moen.KanColle.Dentan.Browser;
using Moen.KanColle.Dentan.Proxy;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Windows;

namespace Moen.KanColle.Dentan.Model
{
    public partial class Preference
    {
        [JsonProperty("version")]
        public string Version { get; } = AppInformation.VersionString;

        [JsonProperty("firstrun")]
        public bool FirstRun { get; set; } = true;

        [JsonProperty("checkupdate")]
        public bool CheckUpdate { get; set; } = true;
        [JsonProperty("channel")]
        public UpdateChannel UpdateChannel { get; set; } = UpdateChannel.Release;

        [JsonProperty("port")]
        public int Port { get; set; } = 15789;
        
        [JsonProperty("upstreamproxy")]
        public UpstreamProxyPreference UpstreamProxy { get; set; } = new UpstreamProxyPreference();
        public class UpstreamProxyPreference
        {
            bool r_Enabled;
            [JsonProperty("enabled")]
            public bool Enabled
            {
                get { return r_Enabled; }
                set
                {
                    r_Enabled = value;
                    KanColleGame.Current.Proxy.EnableUpstreamProxy = value;
                }
            }
            string r_Host;
            [JsonProperty("host")]
            public string Host
            {
                get { return r_Host; }
                set
                {
                    r_Host = value;
                    KanColleGame.Current.Proxy.SetUpstreamProxy(r_Host, r_Port);
                }
            }
            int r_Port;
            [JsonProperty("port")]
            public int Port
            {
                get { return r_Port; }
                set
                {
                    r_Port = value;
                    KanColleGame.Current.Proxy.SetUpstreamProxy(r_Host, r_Port);
                }
            }
            bool r_UseSSL;
            [JsonProperty("usessl")]
            public bool UseSSL
            {
                get { return r_UseSSL; }
                set
                {
                    r_UseSSL = value;
                    KanColleGame.Current.Proxy.UseSSL = value;
                }
            }

            public UpstreamProxyPreference()
            {
                Enabled = false;
                Host = "127.0.0.1";
                UseSSL = false;
            }
        }

        [JsonProperty("cache")]
        public CachePreference Cache { get; set; } = new CachePreference();
        public class CachePreference
        {
            bool r_Enabled;
            [JsonProperty("enabled")]
            public bool Enabled
            {
                get { return r_Enabled; }
                set
                {
                    r_Enabled = value;
                    ResourceCache.IsEnabled = value;
                }
            }
            string r_CacheFolder;
            [JsonProperty("folder")]
            public string CacheFolder
            {
                get { return r_CacheFolder; }
                set
                {
                    r_CacheFolder = value;
                    ResourceCache.CacheFolder = value;
                }
            }
            [JsonProperty("keepoldversion")]
            public bool KeepOldVersionFile { get; set; } = false;

            public CachePreference()
            {
                Enabled = false;
                CacheFolder = "Cache";
            }
        }
        
        [JsonProperty("browser")]
        public BrowserPreference Browser { get; set; } = new BrowserPreference();

        [JsonProperty("window")]
        public WindowPreference Window { get; set; } = new WindowPreference();
        public class WindowPreference
        {
            [JsonProperty("state")]
            public WindowState State { get; set; } = WindowState.Normal;

            [JsonProperty("left")]
            public double Left { get; set; } = 0;
            [JsonProperty("top")]
            public double Top { get; set; } = 0;

            [JsonProperty("width")]
            public double Width { get; set; } = 1280;
            [JsonProperty("height")]
            public double Height { get; set; } = 720;
        }

        public void Save()
        {
            const string rFolder = "Preference";

            if (!Directory.Exists(rFolder))
                Directory.CreateDirectory(rFolder);

            using (var rWriter = new StreamWriter(r_FilePath, false, new UTF8Encoding(true)))
            using (var rJsonWriter = new JsonTextWriter(rWriter))
                r_Serializer.Serialize(rJsonWriter, Current);
        }
    }
}
