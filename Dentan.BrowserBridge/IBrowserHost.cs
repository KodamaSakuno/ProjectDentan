using System;
using System.ServiceModel;

namespace Moen.KanColle.Dentan.Browser
{
    [ServiceContract]
    public interface IBrowserHost
    {
        int HostProcessID { [OperationContract] get; }

        [OperationContract]
        void Attach(IntPtr rpHandle);

        [OperationContract]
        void SetUrl(string rpUrl);
    }
}
