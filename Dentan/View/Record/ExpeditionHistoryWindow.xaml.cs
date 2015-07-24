using Moen.KanColle.Dentan.ViewModel.Record;
using System.Windows;

namespace Moen.KanColle.Dentan.View.Record
{
    /// <summary>
    /// ExpeditionHistoryWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ExpeditionHistoryWindow : Window
    {
        ExpeditionHistoryViewModel r_ViewModel;

        public ExpeditionHistoryWindow()
        {
            InitializeComponent();

            DataContext = r_ViewModel = new ExpeditionHistoryViewModel();

            Loaded += async (s, e) => await r_ViewModel.LoadData();
        }
    }
}
