using Moen.KanColle.Dentan.Utils;
using Moen.KanColle.Dentan.View.Tool;
using Moen.KanColle.Dentan.ViewModel.Browser;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Moen.KanColle.Dentan.ViewModel.Menu
{
    class BrowserMenuViewModel : MenuItemViewModel
    {
        MenuItemViewModel r_TakeScreenshotToClipboard, r_TakeScreenshotToFile;

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
                r_TakeScreenshotToClipboard = new MenuItemViewModel("截图到剪贴板", rCreateCommand(() => ScreenCapturer.Instance.TakeScreenshotAndOutput(rpOutputToClipboard: true)), new KeyGesture(Key.F2)),
                r_TakeScreenshotToFile = new MenuItemViewModel("截图为文件", rCreateCommand(() => ScreenCapturer.Instance.TakeScreenshotAndOutput(rpOutputToClipboard: false)), new KeyGesture(Key.F8)),
                new MenuItemViewModel("特殊截图工具", rCreateCommand(() => new SpecialScreenshotWindow() { Owner = App.Current.MainWindow }.Show())),
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

        protected override void KeyBindingCore(InputBindingCollection rpBindings)
        {
            rpBindings.Add(new KeyBinding(r_TakeScreenshotToClipboard.Command, r_TakeScreenshotToClipboard.KeyGesture));
            rpBindings.Add(new KeyBinding(r_TakeScreenshotToFile.Command, r_TakeScreenshotToFile.KeyGesture));
        }
    }
}
