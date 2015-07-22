using Moen.KanColle.Dentan.View;
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
            Func<Action, DelegatedCommand> rCreateCommand = r => new DelegatedCommand(r, () => BrowserHost.Current != null && BrowserHost.Current.IsReady);

            return new[] 
            {
                new MenuItemViewModel("刷新", rCreateCommand(() => BrowserHost.Current.Refresh())),
                MenuSeparator.Default,
                new MenuItemViewModel("删除缓存", rCreateCommand(() => BrowserHost.Current.ClearCache(false))),
                new MenuItemViewModel("删除缓存和Cookie", rCreateCommand(() => BrowserHost.Current.ClearCache(true))),
                MenuSeparator.Default,
                new MenuItemViewModel("静音", rCreateCommand(() => BrowserHost.Current.SetVolume())),
                MenuSeparator.Default,
                new MenuItemViewModel("手动提取Flash", rCreateCommand(() => BrowserHost.Current.ExtractFlash())),
                MenuSeparator.Default,
                new MenuItemViewModel("100%", rCreateCommand(() => BrowserHost.Current.SetZoom(1.0))),
                new MenuItemViewModel("50%", rCreateCommand(() => BrowserHost.Current.SetZoom(0.5))),
                new MenuItemViewModel("80%", rCreateCommand(() => BrowserHost.Current.SetZoom(0.8))),
                new MenuItemViewModel("150%", rCreateCommand(() => BrowserHost.Current.SetZoom(1.5))),
                new MenuItemViewModel("200%", rCreateCommand(() => BrowserHost.Current.SetZoom(2.0))),
            };
        }
    }
}
