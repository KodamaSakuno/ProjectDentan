using Moen.Collections;
using Moen.KanColle.Dentan.ViewModel.Menu;
using System.Collections.ObjectModel;

namespace Moen.KanColle.Dentan.ViewModel
{
    public class MenuBarViewModel : ModelBase
    {
        ObservableRangeCollection<MenuItemViewModel> r_Menus;
        public ReadOnlyObservableCollection<MenuItemViewModel> Menus { get; private set; }

        public MenuBarViewModel()
        {
            r_Menus = new ObservableRangeCollection<MenuItemViewModel>();
            Menus = new ReadOnlyObservableCollection<MenuItemViewModel>(r_Menus);

            r_Menus.Add(new BrowserMenuViewModel());
            r_Menus.Add(new ViewMenuViewModel());
            r_Menus.Add(new ToolMenuViewModel());
            r_Menus.Add(new HelpMenuViewModel());
        }
    }
}
