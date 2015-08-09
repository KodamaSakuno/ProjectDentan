using Moen.KanColle.Dentan.Browser;
using Moen.KanColle.Dentan.Model;
using Moen.SystemInterop;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace Moen.KanColle.Dentan.ViewModel.Browser
{
    class BrowserViewModel : ModelBase
    {
        public static BrowserViewModel Current { get; private set; }

        HwndSource r_MainWindowSource;

        MemoryMappedFileCommunicator r_Communicator;
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
                    Navigate(value);
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

        public event Action BrowserReady = () => { };

        public BrowserViewModel()
        {
            Current = this;
        }

        public void Load()
        {
            if (r_Communicator == null)
            {
                r_Communicator = new MemoryMappedFileCommunicator($"Moen/ProjectDentan({HostProcessID})", 4096);
                r_Communicator.ReadPosition = 2048;
                r_Communicator.WritePosition = 0;

                r_Communicator.DataReceived += Communicator_DataReceived;

                r_Communicator.StartReader();

                StartBrowserProcess();
            }
        }

        void StartBrowserProcess()
        {
            var rBrowser = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dentan.Browser.exe");
            if (!File.Exists(rBrowser))
                return;

            r_BrowserProcess = Process.Start(rBrowser, $"{Preference.Current.Browser.CurrentLayoutEngine} {HostProcessID}");
        }

        public void Attach(IntPtr rpHandle)
        {
            BrowserControl = new BrowserHostCore(rpHandle);

            IsReady = true;
            BrowserReady();

            KanColleGame.Current.TokenOutdated += () =>
            {
                App.Root.StatusBar.Message = "Token 过期，需要重新登录";
                Navigate(Preference.Current.Browser.Homepage);
            };
            KanColleGame.Current.GameLaunched += async () =>
            {
                ExtractFlash();

                   await Task.Delay(500);

                HideAddressBar = true;

                VolumeManager = new VolumeViewModel((uint)r_BrowserProcess.Id);
                //VolumeManager.DisplayName = r_Bridge.Proxy.VolumeMixerDisplayName;
            };
        }
        
        public void UpdateUrl(string rpUrl)
        {
            r_Url = rpUrl;
            OnPropertyChanged(nameof(Url));
        }

        public void Navigate(string rpUrl)
        {
            r_Communicator.Write("Naviagte:" + rpUrl);
        }
        public void Refresh()
        {
            r_Communicator.Write("Refresh");
        }

        public void SetVolume()
        {
            if (VolumeManager == null)
                return;

            VolumeManager.IsMute = !VolumeManager.IsMute;
        }

        public void SetZoom(double rpZoom)
        {
            r_Communicator.Write("SetZoom:" + rpZoom);
        }
        public void ExtractFlash()
        {
            r_Communicator.Write("ExtractFlash");
        }
        public void ClearCache(bool rpClearCookie)
        {
            r_Communicator.Write(rpClearCookie ? "ClearCache" : "ClearCacheAndCookie");
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

            string[] rParamaters = null;
            switch (rCommand)
            {
                case "Ready":
                    r_Communicator.Write("Port:" + KanColleGame.Current.Proxy.Port);
                    break;

                case "Attach":
                    DispatcherUtil.UIDispatcher.Invoke(() => Attach(new IntPtr(int.Parse(rParamater))));
                    break;

                case "UpdateUrl":
                    UpdateUrl(rParamater);
                    break;

                case "KeyboardMessage":
                    rParamaters = rParamater.Split(',');
                    var rMsg = new MSG()
                    {
                        hwnd = new IntPtr(int.Parse(rParamaters[0])),
                        message =int.Parse(rParamaters[1]),
                        wParam = new IntPtr(int.Parse(rParamaters[2])),
                        lParam = new IntPtr(int.Parse(rParamaters[3])),
                    };

                    if (r_MainWindowSource == null)
                    {
                        var rHandle = DispatcherUtil.UIDispatcher.Invoke(() => new WindowInteropHelper(App.Current.MainWindow).Handle);
                        r_MainWindowSource = HwndSource.FromHwnd(rHandle);
                    }
                    DispatcherUtil.UIDispatcher.BeginInvoke(new Action(() => ((IKeyboardInputSink)r_MainWindowSource).TranslateAccelerator(ref rMsg, ModifierKeys.None)));
                    break;
            }
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
