using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Browser
{
    public class BrowserPreference
    {
        [JsonProperty("zoom")]
        public double Zoom { get; set; } = 1.0;
        
        [JsonProperty("flash")]
        public FlashPreference Flash { get; set; } = new FlashPreference();
        public class FlashPreference
        {
            [JsonProperty("quality")]
            public FlashQuality Quality { get; set; } = FlashQuality.Default;
            [JsonProperty("rendermode")]
            public FlashRenderMode RenderMode { get; set; } = FlashRenderMode.Default;
        }
        
        [JsonProperty("screenshot")]
        public ScreenshotPreference Screenshot { get; set; } = new ScreenshotPreference();
        public class ScreenshotPreference
        {
            [JsonProperty("folder")]
            public string Folder { get; set; } = "Screenshot";
            //[JsonProperty("filenameformat")]
            [JsonIgnore]
            public string FilenameFormat { get; set; } = "";
            [JsonProperty("imageformat")]
            public ScreenshotImageFormat ImageFormat { get; set; } = ScreenshotImageFormat.Png;
        }
    }
}
