using Moen.KanColle.Dentan.Data;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class EquipmentTypeViewModel : ModelBase
    {
        public EquipmentIconType Type { get; private set; }

        bool r_IsSelected;
        public bool IsSelected
        {
            get { return r_IsSelected; }
            set
            {
                if (r_IsSelected != value)
                {
                    r_IsSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public EquipmentTypeViewModel(EquipmentIconType rpType)
        {
            Type = rpType;
        }
    }
}
