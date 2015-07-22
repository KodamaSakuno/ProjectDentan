using Moen.KanColle.Dentan.Model;

namespace Moen.KanColle.Dentan.ViewModel.Preferences
{
    public abstract class PreferenceGroupViewModel : ModelBase
    {
        public abstract string Name { get; }
    }
    abstract class InternalPreferenceGroupViewModel : PreferenceGroupViewModel
    {
        protected Preference Model { get; private set; }

        protected InternalPreferenceGroupViewModel()
        {
            Model = Preference.Current;
        }
    }
}
