using System;
using System.IO;

namespace Moen.KanColle.Dentan.Proxy
{
    public class ResourceSession : Session
    {
        WeakReference<byte[]> r_Data = new WeakReference<byte[]>(null);
        public byte[] Data
        {
            get
            {
                byte[] rResult;
                r_Data.TryGetTarget(out rResult);
                return rResult;
            }
            set { r_Data.SetTarget(value); }
        }

        public DateTime LastModifiedTime { get; set; }

        string r_OriginalCachePath;
        public string OriginalCachePath 
        {
            get
            {
                if (r_OriginalCachePath == null)
                {
                    r_OriginalCachePath = Path.Combine(ResourceCache.CacheFolder, Url.Replace('/', '\\'));
                    var rPosition = r_OriginalCachePath.LastIndexOf('?');
                    if (rPosition != -1)
                        r_OriginalCachePath = r_OriginalCachePath.Remove(rPosition);
                }
                return r_OriginalCachePath;
            }
        }

        string r_Path;
        public string CachePath
        {
            get
            {
                if (r_Path == null)
                    r_Path = Path.Combine(ResourceCache.CacheFolder, ResourceCache.GetFilePath(this));
                return r_Path;
            }
        }
        public string FileNameWithoutVersion { get; internal set; }

        string r_Name;
        public string Name
        {
            get { return r_Name; }
            set
            {
                if (r_Name != value)
                {
                    r_Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public ResourceSession(string rpUrl)
            : base(rpUrl) { }
        public ResourceSession(string rpFullUrl, string rpUrl) : base(rpFullUrl, rpUrl) { }
    }
}
