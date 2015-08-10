using Moen.KanColle.Dentan.Proxy;

namespace Moen.KanColle.Dentan.Cache
{
    public interface ICacheSystem
    {
        void ProcessRequest(ResourceSession rpSession);
        void ProcessResponse(ResourceSession rpSession);
    }
}
