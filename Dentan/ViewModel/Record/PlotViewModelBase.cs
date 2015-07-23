using OxyPlot;
using OxyPlot.Axes;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.ViewModel.Record
{
    public abstract class PlotViewModelBase : ModelBase
    {
        PlotModel r_Model;
        public PlotModel Model
        {
            get { return r_Model; }
            set
            {
                if (r_Model != value)
                {
                    r_Model = value;
                    OnPropertyChanged();
                }
            }
        }
        
        protected PlotViewModelBase()
        {
            r_Model = new PlotModel();
            
            Model.Axes.Add(new DateTimeAxis()
            {
                Key = "DateTime",
                AxislineColor = OxyColors.White,
                TextColor = OxyColors.White,
                ExtraGridlineColor = OxyColors.White,
                MajorGridlineColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                MinorGridlineColor = OxyColors.White,
                TitleColor = OxyColors.White
            });
            Model.Axes.Add(new LinearAxis()
            {
                AxislineColor = OxyColors.White,
                TextColor = OxyColors.White,
                ExtraGridlineColor = OxyColors.White,
                MajorGridlineColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                MinorGridlineColor = OxyColors.White,
                TitleColor = OxyColors.White
            });

            Model.SelectionColor = OxyColors.White;
            Model.LegendTextColor = OxyColors.White;
            Model.LegendTitleColor = OxyColors.White;
            Model.PlotAreaBorderColor = OxyColors.White;
        }

        public abstract Task LoadData();
    }
}
