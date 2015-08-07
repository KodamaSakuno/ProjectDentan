using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Model.Game;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    class ImprovementArsenalEquipmentViewModel : ModelBase
    {
        ImprovementArsenalModel r_Model;
        EquipmentInfo r_Info;
        
        public string Name { get { return r_Info.Name; } }
        public EquipmentIconType Icon { get { return r_Info.IconType; } }

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

        public ImprovementArsenalAssistantViewModel[] Assistants { get; private set; }

        public ImprovementArsenalEquipmentViewModel(ImprovementArsenalModel rpModel)
        {
            r_Model = rpModel;
            r_Info = KanColleGame.Current.Base.Equipments[r_Model.ID];
            Assistants = (from rDetail in r_Model.Detail
                          where new BitVector32(rDetail.Days)[(int)DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(9.0)).DayOfWeek] && rDetail.Assistants != null
                          from rAssistant in rDetail.Assistants
                          select new ImprovementArsenalAssistantViewModel(rAssistant, rDetail.UpdateTo)).ToArray();
        }

        public void Update()
        {
            if (KanColleGame.Current.Ships.Count != 0)
                foreach (var rAssistant in Assistants)
                    rAssistant.Update();

            IsAvailable = KanColleGame.Current.Equipments.Values.Any(r => r.Info.ID == r_Info.ID) &&
                r_Model.Detail.Any(r => new BitVector32(r.Days)[(int)DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(9.0)).DayOfWeek]) &&
                (Assistants.Length == 0 || Assistants.Any(r => r.IsAvailable));
        }
    }
}
