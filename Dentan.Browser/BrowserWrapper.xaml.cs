using Moen.SystemInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    partial class BrowserWrapper : UserControl, IBrowserWrapper
    {
        public Bridge<IBrowserWrapper, IBrowserHost> Bridge { get; private set; }
        string r_Uri;
        HwndSource r_HwndSource;

        public ManualResetEventSlim BridgeReady { get; private set; }

        List<IBrowserProvider> r_BrowserProviders;
        IBrowserProvider r_BrowserProvider;
        public IBrowser Browser { get; private set; }

        public int Port { set { Browser.Port = value; } }
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

        public BrowserWrapper(string rpUri)
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

            r_Uri = rpUri;
            r_Zoom = 1.0;

            Bridge = new Bridge<IBrowserWrapper, IBrowserHost>(this, r_Uri + "Browser", "Browser");
            Bridge.Connect(r_Uri + "BrowserHost");
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

            Bridge.Proxy.Attach(r_HwndSource.Handle);

            Browser.FlashExtracted += UpdateSize;
            Browser.Navigated += r => Bridge.Proxy.SetUrl(r);
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
    }
}
