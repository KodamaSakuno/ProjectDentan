using Moen.KanColle.Dentan.Data;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    class BattlePartViewModel : ViewModel<BattlePart>
    {
        public BattlePartType Type { get { return Model.Type; } }
        public bool IsFirstPart { get { return Model.IsFirstPart; } }
        public bool IsInitializing { get { return Model.IsInitializing; } }
        public BattleStatus[] AlliedStatus { get { return Model.FriendStatus; } }
        public BattleStatus[] EnemyStatus { get { return Model.EnemyStatus; } }

        public BattlePartViewModel(BattlePart rpModel)
            : base(rpModel)
        {
        }
    }
}
