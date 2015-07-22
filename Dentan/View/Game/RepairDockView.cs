using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Game
{
    [ViewID("RepairDocks")]
    class RepairDockView : Control
    {
        static RepairDockView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RepairDockView), new FrameworkPropertyMetadata(typeof(RepairDockView)));
        }
    }
}
