using Moen.KanColle.Dentan.Model;
using Moen.KanColle.Dentan.View;
using Moen.KanColle.Dentan.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Moen.KanColle.Dentan
{
    public partial class App : Application
    {
        public static MainWindowViewModel Root { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!Debugger.IsAttached)
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                DispatcherUnhandledException += App_DispatcherUnhandledException;
            }

            DispatcherUtil.UIDispatcher = Dispatcher;

            Preference.Load();

            Root = new MainWindowViewModel();
            MainWindow = new MainWindow() { DataContext = Root };

            ShowFirstRunWindowIfRequired();

            if (Preference.Current.UpstreamProxy.Enabled)
            {
                KanColleGame.Current.Proxy.EnableUpstreamProxy = true;
                KanColleGame.Current.Proxy.SetUpstreamProxy(Preference.Current.UpstreamProxy.Host, Preference.Current.UpstreamProxy.Port);
            }
            KanColleGame.Current.Proxy.Start(Preference.Current.Port);
            KanColleGame.Current.Proxy.GameToken += r => Preference.Current.Browser.GameToken = r;

            Task.Run(() =>
            {
                DownloadFile("http://api.sakuno.moe/pd/abyssal_data", @"Data\AbyssalData.json");
            });
            Task.Run(() =>
            {
                var rRequest = WebRequest.CreateHttp("http://api.sakuno.moe/pd/lastest_version");
                rRequest.UserAgent = "Project Dentan " + AppInformation.VersionString;
                using (var rResponse = (HttpWebResponse)rRequest.GetResponse())
                using (var rStream = rResponse.GetResponseStream())
                using (var rJsonReader = new JsonTextReader(new StreamReader(rStream)))
                {
                    var rJson = JObject.Load(rJsonReader);
                    var rVersion = Version.Parse((string)rJson["version"]);
                    if (rVersion > AppInformation.Version)
                        Dispatcher.BeginInvoke(new Action(() => MessageBox.Show(MainWindow, $"发现新版（{rVersion}）", "Project Dentan", MessageBoxButton.OK, MessageBoxImage.Information)));
                }
            });

            MainWindow.Show();
        }

        static void DownloadFile(string rpUrl, string rpFilename)
        {
            var rRequest = WebRequest.CreateHttp(rpUrl);
            rRequest.UserAgent = "Project Dentan " + AppInformation.VersionString;
            if (File.Exists(rpFilename))
                rRequest.IfModifiedSince = File.GetLastWriteTime(rpFilename);

            try
            {
                using (var rResponse = (HttpWebResponse)rRequest.GetResponse())
                using (var rResponseStream = rResponse.GetResponseStream())
                {
                    if (!Directory.Exists("Data"))
                        Directory.CreateDirectory("Data");

                    using (var rFileStream = File.Open(rpFilename, FileMode.Create, FileAccess.Write))
                        rResponseStream.CopyTo(rFileStream);

                    File.SetLastWriteTime(rpFilename, Convert.ToDateTime(rResponse.Headers["Last-Modified"]));
                }
            }
            catch (WebException rExcpetion)
            {
                if (((HttpWebResponse)rExcpetion.Response).StatusCode != HttpStatusCode.NotModified)
                    throw;
            }
        }

        public static void ShowFirstRunWindowIfRequired()
        {
            if (Preference.Current.FirstRun && FirstRunWindow.IsRequired())
            {
                new FirstRunWindow().ShowDialog();
                Preference.Current.FirstRun = false;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Preference.Current.Save();
            KanColleGame.Current.Proxy.Shutdown();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
        }
    }
}
