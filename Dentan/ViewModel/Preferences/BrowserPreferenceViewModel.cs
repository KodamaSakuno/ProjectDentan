using Moen.KanColle.Dentan.Browser;
using Moen.KanColle.Dentan.Model;
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SelectScreenshotFolderCommand { get; private set; }

        public BrowserPreferenceViewModel()
        {
            SelectScreenshotFolderCommand = new DelegatedCommand(() =>
            {
            });
        }
    }
}
