using Moen.KanColle.Dentan.Data;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class CompassDataViewModel : ModelBase
    {
        CompassData r_Model;
        public CompassData Model
        {
            get { return r_Model; }
            set
            {
                if (r_Model != value)
                {
                    r_Model = value;
                    OnPropertyChanged();
                }
            }
        }

        public CompassDataViewModel()
        {
            KanColleGame.Current.ObservablePropertyChanged.Where(r => r == "CompassData").Subscribe(_ => Model = KanColleGame.Current.CompassData);
        }
    }
}
