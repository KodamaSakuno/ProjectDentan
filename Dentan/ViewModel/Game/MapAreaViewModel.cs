using Moen.KanColle.Dentan.Data;
using System.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class MapAreaViewModel : ViewModel<MapAreaInfo>
    {
        ExpeditionInfoViewModel[] r_Expeditions;
        public ExpeditionInfoViewModel[] Expeditions
        {
            get { return r_Expeditions; }
            set
            {
                if (r_Expeditions != value)
                {
                    r_Expeditions = value;
                    OnPropertyChanged();
                }
            }
        }

        public MapAreaViewModel(BaseInfo rpBaseInfo, MapAreaInfo rpModel)
            : base(rpModel)
        {
            Expeditions = rpBaseInfo.Expeditions.Values.Where(r => r.MapArea == rpModel.ID).Select(r => new ExpeditionInfoViewModel(r)).ToArray();
        }
    }
}
