using Moen.SystemInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        
        static Dictionary<string, string> r_Map;
        static BrowserWrapper()
        {
            r_Map = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "Browsers"))
                .EnumerateFiles("*.dll", SearchOption.AllDirectories).ToDictionary(r => Path.GetFileNameWithoutExtension(r.FullName), r => r.FullName);
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                var rName = e.Name;
                var rPosition = rName.IndexOf(',');
                if (rPosition != -1)
                    rName = rName.Remove(rPosition);
                return Assembly.LoadFile(r_Map[rName]);
            };
        }
        public BrowserWrapper(string rpLayoutEngine, int rpHostProcessID)
        {
            LoadBrowser(rpLayoutEngine);

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

            Container.Content = Browser = r_BrowserProvider.GetBrowser();

            var rParameters = new HwndSourceParameters(string.Format("Dentan.Browser({0})", r_BrowserProvider.BrowserName)) { WindowStyle = 0 };
            r_HwndSource = new HwndSource(rParameters) { SizeToContent = SizeToContent.Manual };
            r_HwndSource.CompositionTarget.BackgroundColor = Color.FromRgb(0x59, 0x59, 0x59);

            NativeMethods.User32.SetWindowLongPtr(r_HwndSource.Handle, NativeConstants.GetWindowLong.GWL_STYLE,
                (IntPtr)(NativeEnums.WindowStyle.WS_CHILD | NativeEnums.WindowStyle.WS_CLIPCHILDREN));
            NativeMethods.User32.SetWindowPos(r_HwndSource.Handle, IntPtr.Zero, 0, 0, 0, 0,
                NativeEnums.SetWindowPosition.SWP_FRAMECHANGED | NativeEnums.SetWindowPosition.SWP_NOMOVE | NativeEnums.SetWindowPosition.SWP_NOSIZE | NativeEnums.SetWindowPosition.SWP_NOZORDER);

            ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessage;

            r_HwndSource.RootVisual = this;

            r_Communicator.Write("Attach:" + r_HwndSource.Handle.ToInt32());

            Browser.FlashExtracted += UpdateSize;
            Browser.Navigated += r => r_Communicator.Write("UpdateUrl:" + r);
        }
        void LoadBrowser(string rpLayoutEngine)
        {
            var rBrowserPath = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "Browsers"));

            foreach (var rFile in rBrowserPath.EnumerateFiles("*.dll", SearchOption.AllDirectories))
                FileSystem.Unblock(rFile.FullName);

            var rEntryFile = rBrowserPath.EnumerateFiles("*.json").Select(r =>
            {
                using (var rReader = File.OpenText(r.FullName))
                    return JObject.Load(new JsonTextReader(rReader)).ToObject<LayoutEngine>();
            }).Single(r => r.Name == rpLayoutEngine).EntryFile;

            var rAssembly = Assembly.LoadFile(Path.Combine(rBrowserPath.FullName, rEntryFile));
            var rType = rAssembly.GetTypes().Where(r => r.GetInterface(typeof(IBrowserProvider).FullName) != null).First();

            r_BrowserProvider = (IBrowserProvider)rAssembly.CreateInstance(rType.FullName);
            r_BrowserProvider.WorkingDirectory = Path.GetDirectoryName(rAssembly.Location);
            Environment.CurrentDirectory = r_BrowserProvider.WorkingDirectory;
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

        void ThreadPreprocessMessage(ref MSG rpMessage, ref bool rrpHandled)
        {
            var rMessage = (NativeConstants.WindowMessage)rpMessage.message;
            if (rrpHandled)
                return;

            switch(rMessage)
            {
                case NativeConstants.WindowMessage.WM_KEYDOWN:
                case NativeConstants.WindowMessage.WM_KEYUP:
                case NativeConstants.WindowMessage.WM_SYSKEYDOWN:
                case NativeConstants.WindowMessage.WM_SYSKEYUP:
                case NativeConstants.WindowMessage.WM_SYSCHAR:
                case NativeConstants.WindowMessage.WM_SYSDEADCHAR:
                    r_Communicator.Write($"KeyboardMessage:{rpMessage.hwnd.ToInt32()},{rpMessage.message},{rpMessage.wParam.ToInt32()},{rpMessage.lParam.ToInt32()}");
                    break;
            }
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
