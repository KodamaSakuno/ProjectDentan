using Moen.SystemInterop;
using mshtml;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
                    var rElement = rDocument.getElementsByTagName("EMBED").OfType<IHTMLElement>().SingleOrDefault(r => ((string)r.getAttribute("src")).Contains("kcs/mainD2.swf?"));
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

        public ScreenshotData TakeScreenshot()
        {
            return Dispatcher.Invoke<ScreenshotData>(() =>
            {
                var rDocument = r_Browser.Document as HTMLDocument;
                if (rDocument == null)
                    return null;

                if (rDocument.url.Contains("kcs/mainD2.swf?"))
                {
                    var rViewObject = rDocument.getElementsByTagName("EMBED").item(index: 0) as NativeInterfaces.IViewObject;
                    if (rViewObject == null)
                        return null;

                    return TakeScreenshotCore(rViewObject);
                }
                else
                {
                    if (rDocument.getElementById("game_frame") == null)
                        return null;

                    var rFrames = rDocument.frames;
                    for (var i = 0; i < rFrames.length; i++)
                    {
                        var rServiceProvider = rFrames.item(i) as NativeInterfaces.IServiceProvider;
                        if (rServiceProvider == null)
                            return null;

                        var rGuidWebBrowserApp = typeof(SHDocVw.IWebBrowserApp).GUID;
                        var rGuidWebBrowser2 = typeof(SHDocVw.IWebBrowser2).GUID;
                        object rObject;
                        rServiceProvider.QueryService(ref rGuidWebBrowserApp, ref rGuidWebBrowser2, out rObject);
                        var rWebBrowser = rObject as SHDocVw.IWebBrowser2;
                        if (rWebBrowser == null || rWebBrowser.Document == null)
                            return null;

                        var rViewObject = rWebBrowser.Document.getElementById("externalswf") as NativeInterfaces.IViewObject;
                        if (rViewObject == null)
                            continue;

                        return TakeScreenshotCore(rViewObject);
                    }
                }

                return null;
            });
        }

        private ScreenshotData TakeScreenshotCore(NativeInterfaces.IViewObject rpViewObject)
        {
            var rEmbedElement = rpViewObject as HTMLEmbed;
            var rWidth = rEmbedElement.clientWidth;
            var rHeight = rEmbedElement.clientHeight;

            var rResult = new ScreenshotData()
            {
                Width = rWidth,
                Height = rHeight,
            };

            var rTargetDevice = new NativeStructs.DVTARGETDEVICE() { tdSize = 0 };
            var rRect = new NativeStructs.RECT(0, 0, rWidth, rHeight);

            var rScreenDC = NativeMethods.User32.GetDC(IntPtr.Zero);
            var rHDC = NativeMethods.Gdi32.CreateCompatibleDC(rScreenDC);
            
            var rInfo = new NativeStructs.BITMAPINFO();
            rInfo.bmiHeader.biSize = Marshal.SizeOf(typeof(NativeStructs.BITMAPINFOHEADER));
            rInfo.bmiHeader.biWidth = rWidth;
            rInfo.bmiHeader.biHeight = rHeight;
            rInfo.bmiHeader.biBitCount = 24;
            rInfo.bmiHeader.biPlanes = 1;

            IntPtr rBits;
            var rHBitmap = NativeMethods.Gdi32.CreateDIBSection(rHDC, ref rInfo, 0, out rBits, IntPtr.Zero, 0);
            var rOldObject = NativeMethods.Gdi32.SelectObject(rHDC, rHBitmap);

            var rEmptyRect = default(NativeStructs.RECT);
            rpViewObject.Draw(1, 0, IntPtr.Zero, ref rTargetDevice, IntPtr.Zero, rHDC, ref rRect, ref rEmptyRect, IntPtr.Zero, IntPtr.Zero);
            
            var rPixels = new byte[rWidth * rHeight * 3];
            Marshal.Copy(rBits, rPixels, 0, rPixels.Length);
            rResult.ImagePixels = rPixels;

            NativeMethods.Gdi32.SelectObject(rHDC, rOldObject);
            NativeMethods.Gdi32.DeleteObject(rHBitmap);
            NativeMethods.Gdi32.DeleteDC(rHDC);
            NativeMethods.User32.ReleaseDC(IntPtr.Zero, rScreenDC);

            return rResult;
        }
    }
}
