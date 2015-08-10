namespace Moen.KanColle.Dentan.Browser
{
    public interface IBrowserProvider
    {
        string BrowserName { get; }
        string WorkingDirectory { get; set; }

        IBrowser GetBrowser();
        void SetPort(int rpPort);
        void ClearCache(bool rpClearCookie);
    }
}
