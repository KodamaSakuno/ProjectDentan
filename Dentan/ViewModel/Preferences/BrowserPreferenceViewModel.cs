using Moen.KanColle.Dentan.Browser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Moen.KanColle.Dentan.ViewModel.Preferences
{
    class BrowserPreferenceViewModel : InternalPreferenceGroupViewModel
    {
        public override string Name { get { return "浏览器"; } }

        public double Zoom
        {
            get { return Model.Browser.Zoom; }
            set
            {
                if (Model.Browser.Zoom != value)
                {
                    Model.Browser.Zoom = value;
                    OnPropertyChanged(nameof(Zoom));
                }
            }
        }

        public string Homepage
        {
            get { return Model.Browser.Homepage; }
            set
            {
                if (Model.Browser.Homepage != value)
                {
                    Model.Browser.Homepage = value;
                    OnPropertyChanged(nameof(Homepage));
                }
            }
        }

        public string CurrentLayoutEngine
        {
            get { return Model.Browser.CurrentLayoutEngine; }
            set
            {
                if (Model.Browser.CurrentLayoutEngine != value)
                {
                    Model.Browser.CurrentLayoutEngine = value;
                    OnPropertyChanged(nameof(CurrentLayoutEngine));
                }
            }
        }

        public FlashQuality FlashQuality
        {
            get { return Model.Browser.Flash.Quality; }
            set
            {
                if (Model.Browser.Flash.Quality != value)
                {
                    Model.Browser.Flash.Quality = value;
                    OnPropertyChanged(nameof(FlashQuality));
                }
            }
        }
        public FlashRenderMode FlashRenderMode
        {
            get { return Model.Browser.Flash.RenderMode; }
            set
            {
                if (Model.Browser.Flash.RenderMode != value)
                {
                    Model.Browser.Flash.RenderMode = value;
                    OnPropertyChanged(nameof(FlashRenderMode));
                }
            }
        }

        public string ScreenshotFolder
        {
            get { return Model.Browser.Screenshot.Folder; }
            set
            {
                if (Model.Browser.Screenshot.Folder != value)
                {
                    Model.Browser.Screenshot.Folder = value;
                    OnPropertyChanged(nameof(ScreenshotFolder));
                }
            }
        }
        public string ScreenshotFilenameFormat
        {
            get { return Model.Browser.Screenshot.FilenameFormat; }
            set
            {
                if (Model.Browser.Screenshot.FilenameFormat != value)
                {
                    Model.Browser.Screenshot.FilenameFormat = value;
                    OnPropertyChanged(nameof(ScreenshotFilenameFormat));
                }
            }
        }
        public ScreenshotImageFormat ScreenshotImageFormat
        {
            get { return Model.Browser.Screenshot.ImageFormat; }
            set
            {
                if (Model.Browser.Screenshot.ImageFormat != value)
                {
                    Model.Browser.Screenshot.ImageFormat = value;
                    OnPropertyChanged(nameof(ScreenshotImageFormat));
                }
            }
        }

        public ICommand SelectScreenshotFolderCommand { get; private set; }

        public LayoutEngine[] LayoutEngines { get; private set; }

        public BrowserPreferenceViewModel()
        {
            LayoutEngines = Directory.EnumerateFiles("Browsers", "*.json").Select(r =>
            {
                using (var rReader = File.OpenText(r))
                    return JObject.Load(new JsonTextReader(rReader)).ToObject<LayoutEngine>();
            }).ToArray();
            CurrentLayoutEngine = Model.Browser.CurrentLayoutEngine;

            SelectScreenshotFolderCommand = new DelegatedCommand(() =>
            {
            });
        }
    }
}
