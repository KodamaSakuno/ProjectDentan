namespace Moen.KanColle.Dentan.Data
{
    public class ExpeditionFleetInfo : ModelBase
    {
        public int ID { get; private set; }

        bool? r_CanSuccess;
        public bool? CanSuccess
        {
            get { return r_CanSuccess; }
            set
            {
                if (r_CanSuccess != value)
                {
                    r_CanSuccess = value;
                    OnPropertyChanged();
                }
            }
        }

        public ExpeditionFleetInfo(int rpID)
        {
            ID = rpID;
        }
    }
}
