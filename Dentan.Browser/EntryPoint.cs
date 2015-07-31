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
    }
}
