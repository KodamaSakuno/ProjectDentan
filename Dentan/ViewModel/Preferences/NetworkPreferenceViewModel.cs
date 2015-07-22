namespace Moen.KanColle.Dentan.ViewModel.Preferences
{
    class NetworkPreferenceViewModel : InternalPreferenceGroupViewModel
    {
        public override string Name { get { return "网络"; } }

        public int Port
        {
            get { return Model.Port; }
            set
            {
                if (Model.Port != value)
                {
                    Model.Port = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool EnabledUpstreamProxy
        {
            get { return Model.UpstreamProxy.Enabled; }
            set
            {
                if (Model.UpstreamProxy.Enabled != value)
                {
                    Model.UpstreamProxy.Enabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public string UpstreamProxyHost
        {
            get { return Model.UpstreamProxy.Host; }
            set
            {
                if (Model.UpstreamProxy.Host != value)
                {
                    Model.UpstreamProxy.Host = value;
                    OnPropertyChanged();
                }
            }
        }
        public int UpstreamProxyPort
        {
            get { return Model.UpstreamProxy.Port; }
            set
            {
                if (Model.UpstreamProxy.Port != value)
                {
                    Model.UpstreamProxy.Port = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UpstreamProxyUseSSL
        {
            get { return Model.UpstreamProxy.UseSSL; }
            set
            {
                if (Model.UpstreamProxy.UseSSL != value)
                {
                    Model.UpstreamProxy.UseSSL = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
