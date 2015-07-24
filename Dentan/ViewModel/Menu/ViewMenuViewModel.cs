using Moen.KanColle.Dentan.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Moen.KanColle.Dentan.ViewModel.Menu
{
    class ViewMenuViewModel : MenuItemViewModel
    {
        public ViewMenuViewModel()
            : base("视图(_V)") { }

        public override IEnumerable<object> CreateMenuItems()
        {
            return new[] 
            { 
                CreateMenuItem("Admiral", "提督"),
                MenuSeparator.Default,
                CreateMenuItem("Fleets", "舰队"),
                CreateMenuItem("FleetsOverview", "舰队一览"),
                MenuSeparator.Default,
                CreateMenuItem("RepairDocks", "入渠"),
                CreateMenuItem("BuildingDocks", "建造"),
                MenuSeparator.Default,
                CreateMenuItem("ExpeditionOverview", "远征一览"),
                MenuSeparator.Default,
                CreateMenuItem("Quests", "任务"),
                MenuSeparator.Default,
                CreateMenuItem("CompassData", "信息"),
                CreateMenuItem("Battle", "战况"),
                MenuSeparator.Default,
                CreateMenuItem("BrowserHost", "浏览器"),
                MenuSeparator.Default,
                CreateMenuItem("Sessions", "网络会话"),
            };
        }

        object CreateMenuItem(string rpContentID, string rpTitle)
        {
            return new MenuItemViewModel(rpTitle, new DelegatedCommand(() =>
            {
                var rPane = App.Root.Panes.SingleOrDefault(r => r.ContentID == rpContentID);
                if (rPane != null)
                    rPane.Visibility = rPane.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                else
                {
                    var rContent = ViewFactory.GetContentFromID(rpContentID);
                    if (rContent == null) return;

                    var rPanes = App.Root.Panes;
                    rPanes.Add(new PaneViewModel() { ContentID = rpContentID, Title = rpTitle, Content = rContent });
                }
            }));
        }
    }
}
