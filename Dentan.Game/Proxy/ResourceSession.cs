using System;

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

        public ResourceSession(string rpUrl)
            : base(rpUrl) { }
        public ResourceSession(string rpFullUrl, string rpUrl) : base(rpFullUrl, rpUrl) { }
    }
}
