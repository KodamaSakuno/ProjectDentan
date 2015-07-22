using Moen.KanColle.Dentan.Data;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class AdmiralViewModel : ModelBase
    {
        public Admiral Model { get { return KanColleGame.Current.Headquarter.Admiral; } }

        public AdmiralViewModel()
        {
            KanColleGame.Current.Headquarter.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "Admiral")
                        OnPropertyChanged(() => Model);
                };
        }
    }
}
