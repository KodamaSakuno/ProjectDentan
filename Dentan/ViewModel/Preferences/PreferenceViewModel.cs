using Moen.KanColle.Dentan.Model;
using System.Collections.Generic;
using System.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Preferences
{
    public class PreferenceViewModel : ModelBase
    {
        public Preference Model { get; private set; }

        internal NetworkPreferenceViewModel Network { get; private set; }
        internal CachePreferenceViewModel Cache { get; private set; }

        IEnumerable<PreferenceGroupViewModel> r_Groups;
        public IEnumerable<PreferenceGroupViewModel> Groups
        {
            get { return r_Groups; }
            set
            {
                if (r_Groups != value)
                {
                    r_Groups = value;
                    OnPropertyChanged();
                }
            }
        }

        PreferenceGroupViewModel r_SelectedGroup;
        public PreferenceGroupViewModel SelectedGroup
        {
            get { return r_SelectedGroup; }
            set
            {
                if (r_SelectedGroup != value)
                {
                    r_SelectedGroup = value;
                    OnPropertyChanged();
                }
            }
        }

        public PreferenceViewModel()
        {
            Model = Preference.Current;

            r_Groups = new PreferenceGroupViewModel[]
            {
                Network = new NetworkPreferenceViewModel(),
                new BrowserPreferenceViewModel(),
                Cache = new CachePreferenceViewModel(),
            };

            r_SelectedGroup = r_Groups.FirstOrDefault();
        }
    }
}
