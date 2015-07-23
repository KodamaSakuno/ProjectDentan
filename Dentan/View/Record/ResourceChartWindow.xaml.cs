using Moen.KanColle.Dentan.ViewModel.Record;
using System.Windows;

namespace Moen.KanColle.Dentan.View.Record
{
    /// <summary>
    /// ResourceChartWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ResourceChartWindow : Window
    {
        ResourcePlotViewModel r_ViewModel;

        public ResourceChartWindow()
        {
            InitializeComponent();

            Loaded += ResourceChartWindow_Loaded;

            DataContext = r_ViewModel = new ResourcePlotViewModel();
        }

        async void ResourceChartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await r_ViewModel.LoadData();

            Plot.InvalidatePlot(true);
        }
    }
}
