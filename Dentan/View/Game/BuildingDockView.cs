using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Game
{
    [ViewID("BuildingDocks")]
    class BuildingDockView : Control
    {
        static BuildingDockView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BuildingDockView), new FrameworkPropertyMetadata(typeof(BuildingDockView)));
        }
    }
}
