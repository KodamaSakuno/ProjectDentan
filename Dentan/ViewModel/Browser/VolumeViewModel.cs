using Moen.SystemInterop;

namespace Moen.KanColle.Dentan.ViewModel.Browser
{
    public class VolumeViewModel : ModelBase
    {
        VolumeManager r_Manager;

        int r_Volume;
        public int Volume
        {
            get { return r_Volume; }
            set
            {
                var rVolume = value.Clamp(-1, 101);
                if (r_Volume != rVolume)
                {
                    r_Volume = rVolume;
                    r_Manager.Volume = rVolume;
                    OnPropertyChanged();
                }
            }
        }
        internal int InternalVolume
        {
            set
            {
                r_Volume = value;
                OnPropertyChanged(() => Volume);
            }
        }

        bool r_IsMute;
        public bool IsMute
        {
            get { return r_IsMute; }
            set
            {
                if (r_IsMute != value)
                {
                    r_IsMute = value;
                    r_Manager.IsMute = value;
                    OnPropertyChanged();
                }
            }
        }
        internal bool InternalIsMute
        {
            set
            {
                r_IsMute = value;
                OnPropertyChanged(() => IsMute);
            }
        }

        public string DisplayName
        {
            get { return r_Manager.DisplayName; }
            set { r_Manager.DisplayName = value; }
        }

        public VolumeViewModel(uint rpProcessID)
        {
            r_Manager = new VolumeManager(rpProcessID);

            InternalIsMute = r_Manager.IsMute;
            InternalVolume = r_Manager.Volume;

            r_Manager.VolumeChanged += r =>
            {
                InternalIsMute = r.IsMute;
                InternalVolume = r.Volume;
            };
        }
    }
}
