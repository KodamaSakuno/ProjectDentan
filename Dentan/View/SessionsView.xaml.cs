using Moen.KanColle.Dentan.Proxy;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View
{
    [ViewID("Sessions")]
    public partial class SessionsView : UserControl
    {
        public SessionsView()
        {
            InitializeComponent();

            ListBox.ItemContainerGenerator.ItemsChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                    ScrollToLastItem();
            };

            Loaded += SessionsView_Loaded;
        }


        void SessionsView_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollToLastItem();
        }

        void ScrollToLastItem()
        {
            var rItems = ListBox.Items;
            if (rItems.Count == 0)
                return;

            var rItem = rItems[rItems.Count - 1] as Session;
            if (rItem == null)
                return;

            ListBox.ScrollIntoView(rItem);
        }
    }
}
