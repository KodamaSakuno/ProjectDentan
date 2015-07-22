
using System;

namespace Moen.KanColle.Dentan.Browser
{
    public interface IBrowser
    {
        int Port { set; }

        event Action FlashExtracted;

        void Navigate(string rpUrl);
        void Refresh();
        void ExtractFlash();
    }
}
