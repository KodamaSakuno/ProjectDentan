﻿using Moen.KanColle.Dentan.View.Game;
using Moen.KanColle.Dentan.View.Preference;
using System.Collections.Generic;

namespace Moen.KanColle.Dentan.ViewModel.Menu
{
    class ToolMenuViewModel : MenuItemViewModel
    {
        public ToolMenuViewModel()
            : base("工具(_T)") { }

        public override IEnumerable<object> CreateMenuItems()
        {
            return new[]
            {
                new MenuItemViewModel("舰娘一览", new DelegatedCommand(() => { }, () => false)),
                new MenuItemViewModel("装备一览", new DelegatedCommand(() => new EquipmentsWindow().Show())),
                MenuSeparator.Default,
                new MenuItemViewModel("资源图表", new DelegatedCommand(() => { }, () => false)),
                new MenuItemViewModel("出击记录", new DelegatedCommand(() => { }, () => false)),
                new MenuItemViewModel("经验记录", new DelegatedCommand(() => { }, () => false)),
                new MenuItemViewModel("远征记录", new DelegatedCommand(() => { }, () => false)),
                new MenuItemViewModel("建造&开发记录", new DelegatedCommand(() => { }, () => false)),
                MenuSeparator.Default,
                new MenuItemViewModel("设置(_P)", new DelegatedCommand(() => new PreferenceWindow().ShowDialog())),
                new MenuItemViewModel("初次设置向导(_W)", new DelegatedCommand(App.ShowFirstRunWindowIfRequired)),
            };
        }
    }
}
