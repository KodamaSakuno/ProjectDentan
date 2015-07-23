using Moen.KanColle.Dentan.ViewModel.Browser;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PreferenceModel = Moen.KanColle.Dentan.Model.Preference;

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
            r_Browser.BrowserReady += Browser_BrowserReady;

            Loaded += (s, e) =>
            {
                DataContext = r_Browser;
                r_Browser.Load();
            };
        }

        void Browser_BrowserReady()
        {
            if (PreferenceModel.Current.Browser.GameToken.IsNullOrEmpty())
                r_Browser.Navigate(PreferenceModel.Current.Browser.Homepage);
            else
            {
                EntrySelectionBorder.Visibility = Visibility.Visible;
                Browser.Visibility = Visibility.Collapsed;
            }
        }

        void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                r_Browser.Navigate(((TextBox)sender).Text);
        }

        void ButtonLoginWithToken_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EntrySelectionBorder.Visibility = Visibility.Collapsed;
            Browser.Visibility = Visibility.Visible;
            r_Browser.Navigate(PreferenceModel.Current.Browser.GameToken);
        }
        void ButtonRelogin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EntrySelectionBorder.Visibility = Visibility.Collapsed;
            Browser.Visibility = Visibility.Visible;
            r_Browser.Navigate(PreferenceModel.Current.Browser.Homepage);
        }
    }
}
