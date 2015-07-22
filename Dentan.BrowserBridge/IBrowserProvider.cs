namespace Moen.KanColle.Dentan.Browser
{
    public interface IBrowserProvider
    {
        string BrowserName { get; }

        IBrowser GetBrowser();
        void ClearCache(bool rpClearCookie);
    }
}
