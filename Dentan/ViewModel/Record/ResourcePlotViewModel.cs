using Moen.KanColle.Dentan.Record;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.ViewModel.Record
{
    class ResourcePlotViewModel : PlotViewModelBase
    {
        LineSeries r_FuelSeries, r_BulletSeries, r_SteelSeries, r_BauxiteSeries;

        public ResourcePlotViewModel()
        {
            r_FuelSeries = new LineSeries() { Title = "燃料", XAxisKey = "DateTime", Color = OxyColors.GreenYellow, TextColor = OxyColors.White };
            r_BulletSeries = new LineSeries() { Title = "弹药", XAxisKey = "DateTime", Color = OxyColors.LightSeaGreen, TextColor = OxyColors.White };
            r_SteelSeries = new LineSeries() { Title = "钢材", XAxisKey = "DateTime", Color = OxyColors.LightSteelBlue, TextColor = OxyColors.White };
            r_BauxiteSeries = new LineSeries() { Title = "铝", XAxisKey = "DateTime", Color = OxyColors.YellowGreen, TextColor = OxyColors.White };

            Model.Series.Add(r_FuelSeries);
            Model.Series.Add(r_BulletSeries);
            Model.Series.Add(r_SteelSeries);
            Model.Series.Add(r_BauxiteSeries);
        }

        public override async Task LoadData()
        {
            var rRecords = await Task<List<ResourceRecord.Item>>.Run(() => RecordManager.Instance.Resource.GetRecords());

            foreach (var rRecord in rRecords)
            {
                var rDateTime = DateTimeAxis.ToDouble(rRecord.Time);

                r_FuelSeries.Points.Add(new DataPoint(rDateTime, rRecord.Fuel));
                r_BulletSeries.Points.Add(new DataPoint(rDateTime, rRecord.Bullet));
                r_SteelSeries.Points.Add(new DataPoint(rDateTime, rRecord.Steel));
                r_BauxiteSeries.Points.Add(new DataPoint(rDateTime, rRecord.Bauxite));
            }
        }
    }
}
