using Moen.KanColle.Dentan.Data;
using System;
using System.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    class ImprovementArsenalAssistantViewModel : ModelBase
    {
        int r_Assistant;
        public ShipInfo Info { get; private set; }
        
        bool r_IsAvailable;
        public bool IsAvailable
        {
            get { return r_IsAvailable; }
            set
            {
                if (r_IsAvailable != value)
                {
                    r_IsAvailable = value;
                    OnPropertyChanged(nameof(IsAvailable));
                }
            }
        }

        public string UpdateTo { get; private set; }

        public ImprovementArsenalAssistantViewModel(int rpAssistant, int? rpUpdateTo)
        {
            r_Assistant = rpAssistant;
            Info = KanColleGame.Current.Base.Ships[Math.Abs(r_Assistant)];

            if (rpUpdateTo.HasValue)
                UpdateTo = KanColleGame.Current.Base.Equipments[rpUpdateTo.Value].Name;
        }

        public void Update()
        {
            if (r_Assistant > 0)
                IsAvailable = KanColleGame.Current.ShipIDs.Intersect(KanColleGame.Current.Base.ShipIDsGroupByModel[r_Assistant]).Any();
        }
    }
}
