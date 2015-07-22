using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Moen.KanColle.Dentan.Browser
{
    static class EntryPoint
    {
        static bool r_NormalExit;

        static BrowserWrapper r_BrowserWrapper;

        [STAThread]
        static void Main(string[] rpArguments)
        {
            if (rpArguments.Length != 1)
                return;

            r_NormalExit = false;

            SetRegistry();

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                MessageBox.Show(e.ExceptionObject.ToString());
            };

            r_BrowserWrapper = new BrowserWrapper(rpArguments[0]);

            Task.Run(() =>
            {
                r_BrowserWrapper.BridgeReady.Wait();
                Process.GetProcessById(r_BrowserWrapper.Bridge.Proxy.HostProcessID).WaitForExit();

                if (!r_NormalExit)
                    Environment.Exit(2);
            });

            Dispatcher.Run();

            r_NormalExit = true;
        }

        static int GetIEVersion()
        {
            const string rKey = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Internet Explorer";
            var rVersion = Registry.GetValue(rKey, "svcVersion", null);
            return rVersion == null ? 8000 : Version.Parse((string)rVersion).Major * 1000;
        }
        static void SetRegistry()
        {
            const string rKey = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
            var rProcessName = Process.GetCurrentProcess().ProcessName + ".exe";
            Registry.SetValue(rKey, rProcessName, GetIEVersion(), RegistryValueKind.DWord);
        }
    }
}
