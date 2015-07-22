using System.Collections.Generic;

namespace Moen.KanColle.Dentan.ViewModel.Menu
{
    class HelpMenuViewModel : MenuItemViewModel
    {
        public HelpMenuViewModel()
            : base("帮助(_H)") { }

        public override IEnumerable<object> CreateMenuItems()
        {
            return new[]
            {
                new MenuItemViewModel("版本信息(_A)"),
            };
        }
    }
}
