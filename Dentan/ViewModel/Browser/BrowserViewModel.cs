using Moen.KanColle.Dentan.Browser;
using Moen.SystemInterop;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace Moen.KanColle.Dentan.ViewModel.Browser
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class BrowserViewModel : ModelBase, IBrowserHost
    {
        const string LoginPageUrl = "http://www.dmm.com/netgame/social/application/-/detail/=/app_id=854854/";

        public static BrowserViewModel Current { get; private set; }
        
        string r_Uri;
        Bridge<IBrowserHost, IBrowserWrapper> r_Bridge;
        public bool IsReady { get; private set; }

        public int HostProcessID { get { return Process.GetCurrentProcess().Id; } }

        Process r_BrowserProcess;
        public VolumeViewModel VolumeManager { get; private set; }

        object r_BrowserControl;
        public object BrowserControl
        {
            get { return r_BrowserControl; }
            set
            {
                if (r_BrowserControl != value)
                {
                    r_BrowserControl = value;
                    OnPropertyChanged();
                }
            }
        }
        string r_Url;
        public string Url
        {
            get { return r_Url; }
            set
            {
                if (r_Url != value)
                {
                    r_Url = value;
                    OnPropertyChanged();
                    r_Bridge.Proxy.Navigate(value);
                }
            }
        }

        bool r_HideAddressBar;
        public bool HideAddressBar
        {
            get { return r_HideAddressBar; }
            set
            {
                if (r_HideAddressBar != value)
                {
                    r_HideAddressBar = value;
                    OnPropertyChanged();
                }
            }
        }

        public BrowserViewModel()
        {
            Current = this;

            r_Uri = string.Format("net.pipe://localhost/Moen/ProjectDentan({0})/", HostProcessID);
        }

        public void Load()
        {
            if (r_Bridge == null)
            {
                r_Bridge = new Bridge<IBrowserHost, IBrowserWrapper>(this, r_Uri, "BrowserHost");
                r_Bridge.Connect(r_Uri + "Browser/Browser");

                StartBrowserProcess();
            }
        }

        void StartBrowserProcess()
        {
            var rBrowser = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dentan.Browser.exe");
            if (!File.Exists(rBrowser))
                return;

            r_BrowserProcess = Process.Start(rBrowser, r_Uri);
        }

        public void Attach(IntPtr rpHandle)
        {
            BrowserControl = new BrowserHostCore(rpHandle);

            IsReady = true;

            r_Bridge.Proxy.Port = KanColleGame.Current.Proxy.Port;

            if (File.Exists(@"Data\Last.txt"))
                r_Bridge.Proxy.Navigate(File.ReadAllText(@"Data\Last.txt"));
            else
                r_Bridge.Proxy.Navigate(LoginPageUrl);

            KanColleGame.Current.TokenOutdated += () =>
            {
                App.Root.StatusBar.Message = "Token 过期，需要重新登录";
                r_Bridge.Proxy.Navigate(LoginPageUrl);
            };
            KanColleGame.Current.GameLaunched += async () =>
            {
                r_Bridge.Proxy.ExtractFlash();

                await Task.Delay(500);

                HideAddressBar = true;

                VolumeManager = new VolumeViewModel((uint)r_BrowserProcess.Id);
                VolumeManager.DisplayName = r_Bridge.Proxy.VolumeMixerDisplayName;
            };
        }
        
        public void SetUrl(string rpUrl)
        {
            r_Url = rpUrl;
            OnPropertyChanged(nameof(Url));
        }

        public void Refresh()
        {
            r_Bridge.Proxy.Refresh();
        }

        public void SetVolume()
        {
            if (VolumeManager == null)
                return;

            VolumeManager.IsMute = !VolumeManager.IsMute;
        }

        public void SetZoom(double rpZoom)
        {
            r_Bridge.Proxy.Zoom = rpZoom;
        }
        public void ExtractFlash()
        {
            r_Bridge.Proxy.ExtractFlash();
        }
        public void ClearCache(bool rpClearCookie)
        {
            r_Bridge.Proxy.ClearCache(rpClearCookie);
        }

        class BrowserHostCore : HwndHost
        {
            IntPtr r_Handle;

            public BrowserHostCore(IntPtr rpHandle)
            {
                r_Handle = rpHandle;
            }

            protected override HandleRef BuildWindowCore(HandleRef rpParentHandle)
            {
                NativeMethods.User32.SetParent(r_Handle, rpParentHandle.Handle);

                return new HandleRef(this, r_Handle);
            }

            protected override void DestroyWindowCore(HandleRef rpHandle)
            {
            }
        }
    }
}
