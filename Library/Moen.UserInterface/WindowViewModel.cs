
namespace Moen.UserInterface
{
    public class WindowViewModel : ModelBase
    {
        string r_Title = "Window";
        public string Title
        {
            get { return r_Title; }
            set
            {
                if (r_Title != value)
                {
                    r_Title = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
