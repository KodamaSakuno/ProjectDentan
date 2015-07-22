using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View
{
    class PDMenu : Menu
    {
        static PDMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PDMenu), new FrameworkPropertyMetadata(typeof(PDMenu)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PDMenuItem();
        }
    }
    class PDMenuItem : MenuItem
    {
        static PDMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PDMenuItem), new FrameworkPropertyMetadata(typeof(PDMenuItem)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PDMenuItem();
        }
    }
}
