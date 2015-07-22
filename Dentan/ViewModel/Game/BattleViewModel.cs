using Moen.KanColle.Dentan.Data;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    class BattleViewModel : ViewModel<BattleData>
    {
        public BattlePartViewModel DayBattle { get; private set; }

        public BattlePartViewModel NightBattle { get; private set; }


        public BattleViewModel(BattleData rpModel)
            : base(rpModel)
        {
            DayBattle = new BattlePartViewModel(rpModel.DayBattle);

            rpModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "NightBattle")
                    {
                        NightBattle = new BattlePartViewModel(rpModel.NightBattle);
                        OnPropertyChanged("NightBattle");
                    }
                };
        }
    }
}
