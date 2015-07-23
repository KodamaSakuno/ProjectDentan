using Moen.KanColle.Dentan.ViewModel.Browser;
using System;
using System.Collections.Generic;

namespace Moen.KanColle.Dentan.ViewModel.Menu
{
    class BrowserMenuViewModel : MenuItemViewModel
    {
        public BrowserMenuViewModel()
            : base("浏览器(_B)") { }

        public override IEnumerable<object> CreateMenuItems()
        {
            Func<Action, DelegatedCommand> rCreateCommand = r => new DelegatedCommand(r, () => BrowserViewModel.Current != null && BrowserViewModel.Current.IsReady);

            return new[] 
            {
                new MenuItemViewModel("刷新", rCreateCommand(() => BrowserViewModel.Current.Refresh())),
                MenuSeparator.Default,
                new MenuItemViewModel("删除缓存", rCreateCommand(() => BrowserViewModel.Current.ClearCache(false))),
                new MenuItemViewModel("删除缓存和Cookie", rCreateCommand(() => BrowserViewModel.Current.ClearCache(true))),
                MenuSeparator.Default,
                new MenuItemViewModel("静音", rCreateCommand(() => BrowserViewModel.Current.SetVolume())),
                MenuSeparator.Default,
                new MenuItemViewModel("手动提取Flash", rCreateCommand(() => BrowserViewModel.Current.ExtractFlash())),
                MenuSeparator.Default,
                new MenuItemViewModel("100%", rCreateCommand(() => BrowserViewModel.Current.SetZoom(1.0))),
                new MenuItemViewModel("50%", rCreateCommand(() => BrowserViewModel.Current.SetZoom(0.5))),
                new MenuItemViewModel("80%", rCreateCommand(() => BrowserViewModel.Current.SetZoom(0.8))),
                new MenuItemViewModel("150%", rCreateCommand(() => BrowserViewModel.Current.SetZoom(1.5))),
                new MenuItemViewModel("200%", rCreateCommand(() => BrowserViewModel.Current.SetZoom(2.0))),
            };
        }
    }
}
