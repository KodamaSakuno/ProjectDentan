using Moen.KanColle.Dentan.Utils;
using System;
using System.Windows;
using System.Windows.Input;

namespace Moen.KanColle.Dentan.ViewModel.Tool
{
    class SpecialScreenshotViewModel : ModelBase
    {
        static readonly Int32Rect MaterialRect = new Int32Rect(651, 6, 149, 64);

        public FleetDetailSpecialScreenshotViewModel FleetDetail { get; private set; }
        
        bool r_OutputToClipboard;
        public bool OutputToClipboard
        {
            get { return r_OutputToClipboard; }
            set
            {
                if (r_OutputToClipboard != value)
                {
                    r_OutputToClipboard = value;
                    OnPropertyChanged(nameof(OutputToClipboard));
                }
            }
        }

        public ICommand TakeScreenshotOfMaterialCommand { get; private set; }

        public SpecialScreenshotViewModel()
        {
            r_OutputToClipboard = true;

            TakeScreenshotOfMaterialCommand = new DelegatedCommand(TakeScreenshotOfMaterial);

            FleetDetail = new FleetDetailSpecialScreenshotViewModel(this);
        }

        void TakeScreenshotOfMaterial() => ScreenCapturer.Instance.TakePartialScreenshotAndOutput(MaterialRect, r_OutputToClipboard);
    }
}
