using Moen.KanColle.Dentan.Browser;
using Moen.KanColle.Dentan.ViewModel.Browser;
using System.Diagnostics;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View
{
    [ViewID("BrowserHost")]
    public partial class BrowserView : UserControl
    {
        BrowserViewModel r_Browser;

        public BrowserView()
        {
            InitializeComponent();

            r_Browser = new BrowserViewModel();

            Loaded += (s, e) =>
            {
                DataContext = r_Browser;
                r_Browser.Load();
            };
        }
    }
}
