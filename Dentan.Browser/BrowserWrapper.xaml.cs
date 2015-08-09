using Moen.SystemInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Moen.KanColle.Dentan.Browser
{
    /// <summary>
    /// Browser.xaml の相互作用ロジック
    /// </summary>
    partial class BrowserWrapper : UserControl
    {
        MemoryMappedFileCommunicator r_Communicator;
        HwndSource r_HwndSource;

        public ManualResetEventSlim BridgeReady { get; private set; }
        
        List<IBrowserProvider> r_BrowserProviders;
        IBrowserProvider r_BrowserProvider;
        public IBrowser Browser { get; private set; }
        
        public string VolumeMixerDisplayName { get { return string.Format("PD 内置浏览器 ({0})", r_BrowserProvider.BrowserName); } }

        double r_Zoom;
        public double Zoom 
        {
            get { return r_Zoom; }
            set 
            {
                r_Zoom = value;
                UpdateSize();
            } 
        }

        MemoryMappedFile r_ActiveScreenshotMemoryMappedFile;

        public BrowserWrapper(int rpHostProcessID)
        {
            r_BrowserProviders = new List<IBrowserProvider>();
            var rBrowserPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Browsers");
            foreach (var rBrowser in Directory.EnumerateFiles(rBrowserPath, "*.dll"))
            {
                FileSystem.Unblock(rBrowser);

                var rAssembly = Assembly.LoadFile(rBrowser);
                var rTypes = rAssembly.GetTypes().Where(r => r.GetInterface(typeof(IBrowserProvider).FullName) != null);
                foreach (var rType in rTypes)
                {
                    r_BrowserProviders.Add((IBrowserProvider)rAssembly.CreateInstance(rType.FullName));
                }
            }

            BridgeReady = new ManualResetEventSlim(false);

            InitializeComponent();
            
            r_Zoom = 1.0;

            r_Communicator = new MemoryMappedFileCommunicator($"Moen/ProjectDentan({rpHostProcessID})", 4096);
            r_Communicator.ReadPosition = 0;
            r_Communicator.WritePosition = 2048;

            r_Communicator.DataReceived += Communicator_DataReceived;
            r_Communicator.StartReader();
            r_Communicator.Write("Ready");

            BridgeReady.Set();

            r_BrowserProvider = r_BrowserProviders.First();

            Container.Content = Browser = r_BrowserProvider.GetBrowser();

            var rParameters = new HwndSourceParameters(string.Format("Dentan.Browser({0})", r_BrowserProvider.BrowserName)) { WindowStyle = 0 };
            r_HwndSource = new HwndSource(rParameters) { SizeToContent = SizeToContent.Manual };
            r_HwndSource.CompositionTarget.BackgroundColor = Color.FromRgb(0x59, 0x59, 0x59);

            NativeMethods.User32.SetWindowLongPtr(r_HwndSource.Handle, NativeConstants.GetWindowLong.GWL_STYLE,
                (IntPtr)(NativeEnums.WindowStyle.WS_CHILD | NativeEnums.WindowStyle.WS_CLIPCHILDREN));
            NativeMethods.User32.SetWindowPos(r_HwndSource.Handle, IntPtr.Zero, 0, 0, 0, 0,
                NativeEnums.SetWindowPosition.SWP_FRAMECHANGED | NativeEnums.SetWindowPosition.SWP_NOMOVE | NativeEnums.SetWindowPosition.SWP_NOSIZE | NativeEnums.SetWindowPosition.SWP_NOZORDER);

            r_HwndSource.RootVisual = this;
            
            r_Communicator.Write("Attach:" + r_HwndSource.Handle.ToInt32());

            Browser.FlashExtracted += UpdateSize;
            Browser.Navigated += r => r_Communicator.Write("UpdateUrl:" + r);
        }
        
        public void Refresh()
        {
            Dispatcher.BeginInvoke(new Action(Browser.Refresh));
        }
        public void Navigate(string rpUrl)
        {
            Dispatcher.BeginInvoke(new Action<string>(Browser.Navigate), rpUrl);
        }

        void UpdateSize()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Width = 800 * Zoom;
                Height = 480 * Zoom;
                VerticalAlignment = VerticalAlignment.Top;
            }));
        }

        public void ExtractFlash()
        {
            Browser.ExtractFlash();
        }

        public void ClearCache(bool rpClearCookie)
        {
            Task.Run(() => r_BrowserProvider.ClearCache(rpClearCookie));
        }
        
        void Communicator_DataReceived(byte[] rpBytes)
        {
            var rMessage = Encoding.UTF8.GetString(rpBytes);
            var rCommand = rMessage;
            var rParamater = string.Empty;
            var rPos = rMessage.IndexOf(':');
            if (rPos != -1)
            {
                rCommand = rMessage.Remove(rPos);
                rParamater = rMessage.Substring(rPos + 1);
            }

            switch (rCommand)
            {
                case "Port":
                    r_BrowserProvider.SetPort(int.Parse(rParamater));
                    break;

                case "Naviagte":
                    Navigate(rParamater);
                    break;
                case "Refresh":
                    Refresh();
                    break;

                case "SetZoom":
                    Zoom = double.Parse(rParamater);
                    break;

                case "ExtractFlash":
                    ExtractFlash();
                    break;

                case "TakeScreenshot":
                    BeginScreenshotTransmission(Browser.TakeScreenshot());
                    break;
                case "FinishScreenshotTransmission":
                    r_ActiveScreenshotMemoryMappedFile.Dispose();
                    r_ActiveScreenshotMemoryMappedFile = null;
                    break;

                case "ClearCache":
                    ClearCache(false);
                    break;
                case "ClearCacheAndCookie":
                    ClearCache(true);
                    break;
            }
        }

        void BeginScreenshotTransmission(ScreenshotData rpData)
        {
            if (rpData == null || rpData.ImagePixels == null)
            {
                r_Communicator.Write("ScreenshotFail");
                return;
            }
            
            const string MapName = "ProjectDentan/Screenshot";

            r_ActiveScreenshotMemoryMappedFile = MemoryMappedFile.CreateNew(MapName, rpData.ImagePixels.Length, MemoryMappedFileAccess.ReadWrite);

            using (var rStream = r_ActiveScreenshotMemoryMappedFile.CreateViewStream())
                rStream.Write(rpData.ImagePixels, 0, rpData.ImagePixels.Length);
            
            r_Communicator.Write($"ScreenshotTransmission:{MapName},{rpData.Width},{rpData.Height}");
        }
    }
}
