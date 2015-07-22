using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Game
{
    public class SlotsView : Control
    {
        static SlotsView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SlotsView), new FrameworkPropertyMetadata(typeof(SlotsView)));
        }
    }
}
