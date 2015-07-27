namespace Moen.KanColle.Dentan.Cache
{
    public static class ResourceCache
    {
        public static bool IsEnabled { get; set; }
        public static string CacheFolder { get; set; } = "Cache";

        public static ICacheSystem CurrentSystem { get; set; }
        
        static ResourceCache()
        {
            CurrentSystem = new FullyTrustCacheSystem();
        }
    }
}
