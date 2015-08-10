using Moen.KanColle.Dentan.Utils;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Moen.KanColle.Dentan.ViewModel.Tool
{
    class FleetDetailSpecialScreenshotViewModel : ModelBase
    {
        static readonly Int32Rect DetailRect = new Int32Rect(319, 102, 468, 366);

        SpecialScreenshotViewModel r_Owner;

        ShipDetail[] r_Details;
        public ShipDetail[] Details
        {
            get { return r_Details; }
            set
            {
                if (r_Details != value)
                {
                    r_Details = value;
                    OnPropertyChanged(nameof(Details));
                }
            }
        }
        int r_ShipCount;
        public int ShipCount
        {
            get { return r_ShipCount; }
            set
            {
                if (r_ShipCount != value)
                {
                    r_ShipCount = value;
                    OnPropertyChanged(nameof(ShipCount));

                    foreach (var rDetail in r_Details)
                        rDetail.IsVisible = rDetail.ID <= value;
                }
            }
        }

        int r_ColumnCount;
        public int ColumnCount
        {
            get { return r_ColumnCount; }
            set
            {
                if (r_ColumnCount != value)
                {
                    r_ColumnCount = value;
                    OnPropertyChanged(nameof(ColumnCount));
                }
            }
        }

        public ICommand OutputCommand { get; private set; }

        public FleetDetailSpecialScreenshotViewModel(SpecialScreenshotViewModel rpOwner)
        {
            r_Owner = rpOwner;

            r_Details = new ShipDetail[6];
            for (var i = 0; i < 6; i++)
                r_Details[i] = new ShipDetail(i + 1);

            r_ShipCount = 6;
            r_ColumnCount = 3;

            OutputCommand = new DelegatedCommand(Output);
        }

        void Output()
        {
            var rDrawingVisual = new DrawingVisual();
            using (var rDrawingContext = rDrawingVisual.RenderOpen())
            {
                for (var i = 0; i < r_ShipCount; i++)
                {
                    var rDetail = r_Details[i];

                    var rLeft = i % r_ColumnCount * DetailRect.Width;
                    var rTop = i / r_ColumnCount * DetailRect.Height;
                    var rRect = new Rect(rLeft, rTop, DetailRect.Width, DetailRect.Height);

                    if (rDetail.Screenshot != null)
                        rDrawingContext.DrawImage(rDetail.Screenshot, rRect);
                    else
                        rDrawingContext.DrawRectangle(Brushes.Black, null, rRect);
                }
            }

            var rWidth = r_ColumnCount * DetailRect.Width;
            var rHeight = (int)Math.Ceiling(r_ShipCount / (double)r_ColumnCount) * DetailRect.Height;
            var rBitmap = new RenderTargetBitmap(rWidth, rHeight, 96, 96, PixelFormats.Default);
            rBitmap.Render(rDrawingVisual);

            if (r_Owner.OutputToClipboard)
                ScreenCapturer.Instance.OutputToClipboard(rBitmap);
            else
                ScreenCapturer.Instance.OutputAsFile(rBitmap);
        }

        internal class ShipDetail : ModelBase
        {
            public int ID { get; private set; }

            bool r_IsVisible;
            public bool IsVisible
            {
                get { return r_IsVisible; }
                set
                {
                    if (r_IsVisible != value)
                    {
                        r_IsVisible = value;
                        OnPropertyChanged(nameof(IsVisible));
                    }
                }
            }

            BitmapSource r_Screenshot;
            public BitmapSource Screenshot
            {
                get { return r_Screenshot; }
                set
                {
                    if (r_Screenshot != value)
                    {
                        r_Screenshot = value;
                        OnPropertyChanged(nameof(Screenshot));
                    }
                }
            }

            public ICommand TakeScreenshotCommand { get; private set; }

            public ShipDetail(int rpID)
            {
                ID = rpID;
                r_IsVisible = true;

                TakeScreenshotCommand = new DelegatedCommand(async () => Screenshot = await ScreenCapturer.Instance.TakePartialScreenshot(DetailRect));
            }
        }
    }
}
