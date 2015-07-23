
using System;

namespace Moen.KanColle.Dentan.Browser
{
    public interface IBrowser
    {
        int Port { set; }

        event Action FlashExtracted;
        event Action<string> Navigated;

        void Navigate(string rpUrl);
        void Refresh();
        void ExtractFlash();
    }
}
