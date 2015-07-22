using System.Windows;

namespace Moen.KanColle.Dentan.ViewModel
{
    public class PaneViewModel : ModelBase
    {
        string r_ContentID;
        public string ContentID
        {
            get { return r_ContentID; }
            set
            {
                if (r_ContentID != value)
                {
                    r_ContentID = value;
                    OnPropertyChanged();
                }
            }
        }

        string r_Title;
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

        object r_Content;
        public object Content
        {
            get { return r_Content; }
            set
            {
                if (r_Content != value)
                {
                    r_Content = value;
                    OnPropertyChanged();
                }
            }
        }

        Visibility r_Visibility;
        public Visibility Visibility
        {
            get { return r_Visibility; }
            set
            {
                if (r_Visibility != value)
                {
                    r_Visibility = value;
                    OnPropertyChanged();
                    OnPropertyChanged(() => IsVisible);
                }
            }
        }

        public bool IsVisible
        {
            get { return r_Visibility == Visibility.Visible; }
            set { Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }
    }
}
