using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Game
{
    [ViewID("CompassData")]
    [GameVMBinding("CompassData.Model")]
    public class CompassDataView : Control
    {
        static CompassDataView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CompassDataView), new FrameworkPropertyMetadata(typeof(CompassDataView)));
        }
    }
}
