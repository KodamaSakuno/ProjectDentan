namespace Moen.KanColle.Dentan.ViewModel.Menu
{
    public class MenuSeparator : MenuItemViewModel
    {
        static MenuSeparator r_Default = new MenuSeparator();
        public static MenuSeparator Default { get { return r_Default; } }

        MenuSeparator()
            : base("-")
        {

        }
    }
}
