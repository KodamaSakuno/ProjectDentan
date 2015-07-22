
namespace Moen.KanColle.Dentan.Data
{
    public class AerialCombat : ModelBase
    {
        AerialCombatResult? r_Result;
        public AerialCombatResult? Result
        {
            get { return r_Result; }
            set
            {
                if (r_Result != value)
                {
                    r_Result = value;
                    OnPropertyChanged();
                }
            }
        }

        Stage r_Stage1;
        public Stage Stage1
        {
            get { return r_Stage1; }
            set
            {
                if (r_Stage1 != value)
                {
                    r_Stage1 = value;
                    OnPropertyChanged();
                }
            }
        }
        Stage r_Stage2;
        public Stage Stage2
        {
            get { return r_Stage2; }
            set
            {
                if (r_Stage2 != value)
                {
                    r_Stage2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public class Stage
        {
            public int? FriendPlaneCount { get; set; }
            public int? EnemyPlaneCount { get; set; }

            public int? AfterFriendPlaneCount { get; set; }
            public int? AfterEnemyPlaneCount { get; set; }
        }
    }
}
