using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Game
{
    [ViewID("Battle")]
    [GameVMBinding("Battle")]
    class BattleView : Control
    {
        static BattleView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BattleView), new FrameworkPropertyMetadata(typeof(BattleView)));
        }
    }
}
