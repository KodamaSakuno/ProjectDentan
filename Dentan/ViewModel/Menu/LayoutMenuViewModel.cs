using Microsoft.Win32;
using Moen.KanColle.Dentan.View;
using System.Collections.Generic;

namespace Moen.KanColle.Dentan.ViewModel.Menu
{
    class LayoutMenuViewModel : MenuItemViewModel
    {
        public LayoutMenuViewModel()
            : base("面板布局") { }

        public override IEnumerable<object> CreateMenuItems()
        {
            return new[]
            {
                new MenuItemViewModel("锁定", new DelegatedCommand(() => { }, () => false)),
                MenuSeparator.Default,
                new MenuItemViewModel("加载", new DelegatedCommand(LoadLayout)),
                new MenuItemViewModel("保存", new DelegatedCommand(SaveLayout)),
            };
        }
        
        void LoadLayout()
        {
            var rDialog = new OpenFileDialog()
            {
                Filter = "布局文件(.config)|*.config",
                Title = "加载布局文件",
            };

            var rMainWindow = (MainWindow)App.Current.MainWindow;
            if (rDialog.ShowDialog(rMainWindow) ?? false)
                using (var rStream = rDialog.OpenFile())
                    rMainWindow.LoadLayout(rStream);
        }
        void SaveLayout()
        {
            var rDialog = new SaveFileDialog()
            {
                Filter = "布局文件(.config)|*.config",
                Title = "保存布局文件",
            };

            var rMainWindow = (MainWindow)App.Current.MainWindow;
            if (rDialog.ShowDialog(rMainWindow) ?? false)
                using (var rStream = rDialog.OpenFile())
                    rMainWindow.SaveLayout(rStream);
        }
    }
}
