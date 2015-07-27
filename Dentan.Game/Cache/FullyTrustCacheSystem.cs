using Fiddler;
using Moen.KanColle.Dentan.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.Cache
{
    class FullyTrustCacheSystem : ICacheSystem
    {
        public void ProcessRequest(ResourceSession rpSession)
        {
            var rPath = rpSession.Url;
            var rQueryPosition = rPath.LastIndexOf('?');
            if (rQueryPosition != -1)
                rPath = rPath.Remove(rQueryPosition);
            
            var rFilename = Path.Combine(ResourceCache.CacheFolder, rPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar));
            var rFileInfo = new FileInfo(rFilename);
            if (!rFileInfo.Exists)
                return;
            
            var rFiddlerSession = rpSession.FiddlerSession;
            rFiddlerSession.utilCreateResponseAndBypassServer();
            rFiddlerSession.ResponseBody = File.ReadAllBytes(rFilename);
            rFiddlerSession.oResponse.headers.Add("Last-Modified", rFileInfo.LastWriteTime.ToString("s"));

            rpSession.ContentLength = rpSession.LoadedBytes = rFiddlerSession.ResponseBody.Length;
            rpSession.Status = SessionStatus.LoadedFromCache;
        }

        public void ProcessResponse(ResourceSession rpSession)
        {
        }
    }
}
