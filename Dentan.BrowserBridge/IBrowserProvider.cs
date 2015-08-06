namespace Moen.KanColle.Dentan.Browser
{
    public interface IBrowserProvider
    {
        string BrowserName { get; }

        IBrowser GetBrowser();
        void SetPort(int rpPort);
        void ClearCache(bool rpClearCookie);
    }
}
