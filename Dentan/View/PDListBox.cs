using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View
{
    public class PDListBox : ListBox
    {
        static PDListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PDListBox), new FrameworkPropertyMetadata(typeof(PDListBox)));
        }
    }
}
