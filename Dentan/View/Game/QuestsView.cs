using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Game
{
    [ViewID("Quests")]
    [GameVMBinding("Quests")]
    class QuestsView : Control
    {
        static QuestsView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuestsView), new FrameworkPropertyMetadata(typeof(QuestsView)));
        }
    }
}
