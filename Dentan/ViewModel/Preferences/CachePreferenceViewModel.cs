namespace Moen.KanColle.Dentan.ViewModel.Preferences
{
    class CachePreferenceViewModel : InternalPreferenceGroupViewModel
    {
        public override string Name { get { return "缓存"; } }

        public bool IsEnabled
        {
            get { return Model.Cache.Enabled; }
            set
            {
                if (Model.Cache.Enabled != value)
                {
                    Model.Cache.Enabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public string CacheFolder
        {
            get { return Model.Cache.CacheFolder; }
            set
            {
                if (Model.Cache.CacheFolder != value)
                {
                    Model.Cache.CacheFolder = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
