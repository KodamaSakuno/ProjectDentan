using Moen.KanColle.Dentan.ViewModel.Record;
using System.Windows;

namespace Moen.KanColle.Dentan.View.Record
{
    /// <summary>
    /// BattleHistoryWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BattleHistoryWindow : Window
    {
        BattleHistoryViewModel r_ViewModel;

        public BattleHistoryWindow()
        {
            InitializeComponent();

            Loaded += ResourceChartWindow_Loaded;

            DataContext = r_ViewModel = new BattleHistoryViewModel();
        }

        async void ResourceChartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await r_ViewModel.LoadData();
        }
    }
}
