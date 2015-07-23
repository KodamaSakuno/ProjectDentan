using Moen.KanColle.Dentan.Record;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.ViewModel.Record
{
    class MaterialPlotViewModel : PlotViewModelBase
    {
        LineSeries r_BucketSeries;

        public MaterialPlotViewModel()
        {
            r_BucketSeries = new LineSeries() { Title = "高速修复材", XAxisKey = "DateTime", TextColor = OxyColors.White };
        }

        public override async Task LoadData()
        {
            var rRecords = await Task<List<ResourceRecord.Item>>.Run(() => RecordManager.Instance.Resource.GetRecords());

            foreach (var rRecord in rRecords)
            {
                var rDateTime = DateTimeAxis.ToDouble(rRecord.Time);

                r_BucketSeries.Points.Add(new DataPoint(rDateTime, rRecord.Bucket));
            }
        }
    }
}
