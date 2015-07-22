using Moen.KanColle.Dentan.ViewModel.Menu;
using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View
{
    public class MenuSeparatorStyleSelector : StyleSelector
    {
        static Style r_SeparatorStyle;

        static MenuSeparatorStyleSelector()
        {
            var rTemplate = new ControlTemplate(typeof(MenuItem));
            rTemplate.VisualTree = new FrameworkElementFactory(typeof(Separator));
            rTemplate.VisualTree.SetValue(Separator.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);

            r_SeparatorStyle = new Style(typeof(MenuItem));
            r_SeparatorStyle.Setters.Add(new Setter(MenuItem.TemplateProperty, rTemplate));
        }

        public override Style SelectStyle(object rpItem, DependencyObject rpContainer)
        {
            return rpItem is MenuSeparator ? r_SeparatorStyle : null;
        }
    }
}
