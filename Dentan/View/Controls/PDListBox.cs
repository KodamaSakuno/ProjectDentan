using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Controls
{
    public class PDListBox : ListBox
    {
        static PDListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PDListBox), new FrameworkPropertyMetadata(typeof(PDListBox)));
        }
    }
}
