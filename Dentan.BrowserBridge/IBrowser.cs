using System;

namespace Moen.KanColle.Dentan.Browser
{
    public interface IBrowser
    {
        event Action FlashExtracted;
        event Action<string> Navigated;

        void Navigate(string rpUrl);
        void Refresh();
        void ExtractFlash();
        ScreenshotData TakeScreenshot();
    }
}
