using System.ServiceModel;

namespace Moen.KanColle.Dentan.Browser
{
    [ServiceContract]
    public interface IBrowserWrapper
    {
        int Port { [OperationContract] set; }
        string VolumeMixerDisplayName { [OperationContract]get; }

        double Zoom { [OperationContract] set; }

        [OperationContract]
        void Refresh();
        [OperationContract]
        void Navigate(string rpUrl);
        [OperationContract]
        void ExtractFlash();
        [OperationContract]
        void ClearCache(bool rpClearCookie);
    }
}
