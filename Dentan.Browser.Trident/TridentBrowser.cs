using mshtml;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.Browser.Trident
{
    class TridentBrowser : Border, IBrowser
    {
        WebBrowser r_Browser;
        
        public event Action FlashExtracted = () => { };
        public event Action<string> Navigated = delegate { };
        bool r_IsExtracted;

        public TridentBrowser()
        {
            Child = r_Browser = new WebBrowser();
            r_Browser.Navigated += (s, e) =>
            {
                SuppressScriptError();
                Navigated(e.Uri.OriginalString);
            };

            r_Browser.LoadCompleted += (s, e) => ExtractFlash();
        }

        public void Navigate(string rpUrl)
        {
            r_IsExtracted = false;
            r_Browser.Navigate(rpUrl);
        }

        public void Refresh()
        {
            r_IsExtracted = false;
            r_Browser.Refresh();
        }

        public void ExtractFlash()
        {
            if (r_IsExtracted) return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var rDocument = r_Browser.Document as HTMLDocument;
                var rUri = r_Browser.Source;
                if (rUri.AbsoluteUri.Contains("gadgets/=/app_id=854854"))
                {
                    var rGameFrame = rDocument.getElementById("game_frame");
                    ExtractFlashCore(rGameFrame);
                }
                else
                {
                    var rElement = rDocument.getElementsByTagName("EMBED").OfType<IHTMLElement>().SingleOrDefault(r => ((string)r.getAttribute("src")).Contains("kcs/mainD2.swf"));
                    if (rElement != null)
                        ExtractFlashCore(rElement);
                }
            }));
        }
        void ExtractFlashCore(IHTMLElement rpElement)
        {
            rpElement.setAttribute("wmode", "direct");
            rpElement.document.createStyleSheet().cssText = @"body {
            margin:0;
            overflow:hidden
        }
        
        #game_frame {
            position:fixed;
            left:50%;
            top:-16px;
            margin-left:-450px;
            z-index:1
        }";

            r_IsExtracted = true;
            FlashExtracted();
        }

        void SuppressScriptError()
        {
            var rField = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (rField == null)
                return;

            var rObject = rField.GetValue(r_Browser);
            if (rObject == null)
                return;

            rObject.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, rObject, new object[] { true });
        }
    }
}
