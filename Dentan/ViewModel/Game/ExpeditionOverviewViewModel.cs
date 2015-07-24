using Moen.KanColle.Dentan.Data;
using System.Linq;
using System.Reactive.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class ExpeditionOverviewViewModel : ModelBase
    {
        MapAreaViewModel[] r_Areas;
        public MapAreaViewModel[] Areas
        {
            get { return r_Areas; }
            set
            {
                if (r_Areas != value)
                {
                    r_Areas = value;
                    OnPropertyChanged();
                }
            }
        }

        public ExpeditionOverviewViewModel()
        {
            ExpeditionDataManager.Load();

            KanColleGame.Current.BaseDataLoaded += r => Areas = r.MapAreas.Values.Select(rpMapArea => new MapAreaViewModel(r, rpMapArea)).ToArray();
        }
    }
}
