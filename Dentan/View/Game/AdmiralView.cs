using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Game
{
    [ViewID("Admiral")]
    class AdmiralView : Control
    {
        static AdmiralView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdmiralView), new FrameworkPropertyMetadata(typeof(AdmiralView)));
        }
    }
}
