using Moen.KanColle.Dentan.Browser;
using Moen.KanColle.Dentan.Model;
using Moen.SystemInterop;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Moen.KanColle.Dentan.Utils
{
    public class ScreenCapturer
    {
        public static ScreenCapturer Instance { get; private set; }

        MemoryMappedFileCommunicator r_Communicator;

        TaskCompletionSource<BitmapSource> r_TaskScreenshotTask;

        public ScreenCapturer(MemoryMappedFileCommunicator rpCommunicator)
        {
            r_Communicator = rpCommunicator;

            Instance = this;
        }

        public void ScreenshotFail()
        {
            r_TaskScreenshotTask.SetResult(null);
            App.Root.StatusBar.Message = "截图失败";
        }

        public async Task<BitmapSource> TakeScreenshot(Func<BitmapSource, BitmapSource> rpProcessAction)
        {
            r_TaskScreenshotTask = new TaskCompletionSource<BitmapSource>();
            r_Communicator.Write("TakeScreenshot");

            var rImage = await r_TaskScreenshotTask.Task;

            if (rpProcessAction != null)
                rImage = rpProcessAction(rImage);

            r_TaskScreenshotTask = null;

            return rImage;
        }
        public Task<BitmapSource> TakePartialScreenshot(Int32Rect rpRect)
        {
            return TakeScreenshot(r =>
            {
                var rResult = new CroppedBitmap(r, rpRect);
                rResult.Freeze();

                return rResult;
            });
        }
        public async void TakeScreenshotAndOutput(Func<BitmapSource, BitmapSource> rpProcessAction = null, bool rpOutputToClipboard = true)
        {
            if (r_TaskScreenshotTask != null)
                return;

            var rImage = await TakeScreenshot(rpProcessAction);
            if (rImage == null)
                return;

            if (rpOutputToClipboard)
                OutputToClipboard(rImage);
            else
                OutputAsFile(rImage);
        }
        public async void TakePartialScreenshotAndOutput(Int32Rect rpRect, bool rpOutputToClipboard)
        {
            if (r_TaskScreenshotTask != null)
                return;

            var rImage = await TakePartialScreenshot(rpRect);
            if (rImage == null)
                return;

            if (rpOutputToClipboard)
                OutputToClipboard(rImage);
            else
                OutputAsFile(rImage);
        }

        public void OutputToClipboard(BitmapSource rpImage)
        {
            Clipboard.SetImage(rpImage);
            App.Root.StatusBar.Message = "截图已保存到剪贴板上";
        }
        public void OutputAsFile(BitmapSource rpImage)
        {
            var rPreference = Preference.Current.Browser.Screenshot;

            string rExtension;
            BitmapEncoder rEncoder;
            switch (rPreference.ImageFormat)
            {
                case ScreenshotImageFormat.Png:
                    rExtension = "png";
                    rEncoder = new PngBitmapEncoder();
                    break;

                case ScreenshotImageFormat.Jpeg:
                    rExtension = "jpg";
                    rEncoder = new JpegBitmapEncoder();
                    break;

                case ScreenshotImageFormat.Bmp:
                    rExtension = "bmp";
                    rEncoder = new BmpBitmapEncoder();
                    break;

                default: throw new InvalidOperationException("Unknown image format.");
            }

            var rPath = Path.Combine(rPreference.Folder, string.Format(rPreference.FilenameFormat, DateTime.Now, rExtension));
            var rDirectory = Path.GetDirectoryName(rPath);
            if (!Directory.Exists(rDirectory))
                Directory.CreateDirectory(rDirectory);

            using (var rFile = File.Open(rPath, FileMode.Create))
            {
                rEncoder.Frames.Add(BitmapFrame.Create(rpImage));
                rEncoder.Save(rFile);
            }

            App.Root.StatusBar.Message = "截图已保存到 " + rPath;
        }

        internal void GetScreenshot(string rpMapName, int rpWidth, int rpHeight)
        {
            using (var rMap = MemoryMappedFile.CreateOrOpen(rpMapName, rpWidth * rpHeight * 3, MemoryMappedFileAccess.ReadWrite))
            {
                var rInfo = new NativeStructs.BITMAPINFO();
                rInfo.bmiHeader.biSize = Marshal.SizeOf(typeof(NativeStructs.BITMAPINFOHEADER));
                rInfo.bmiHeader.biWidth = rpWidth;
                rInfo.bmiHeader.biHeight = rpHeight;
                rInfo.bmiHeader.biBitCount = 24;
                rInfo.bmiHeader.biPlanes = 1;

                IntPtr rBits;
                var rHBitmap = NativeMethods.Gdi32.CreateDIBSection(IntPtr.Zero, ref rInfo, 0, out rBits, rMap.SafeMemoryMappedFileHandle.DangerousGetHandle(), 0);
                if (rHBitmap == IntPtr.Zero)
                    throw new InvalidOperationException();

                DispatcherUtil.UIDispatcher.Invoke(() =>
                {
                    var rImage = Imaging.CreateBitmapSourceFromHBitmap(rHBitmap, IntPtr.Zero, new Int32Rect(0, 0, rpWidth, rpHeight), BitmapSizeOptions.FromEmptyOptions());
                    rImage.Freeze();
                    r_TaskScreenshotTask.SetResult(rImage);
                });

                NativeMethods.Gdi32.DeleteObject(rHBitmap);
            }

            r_Communicator.Write("FinishScreenshotTransmission");
        }
    }
}
