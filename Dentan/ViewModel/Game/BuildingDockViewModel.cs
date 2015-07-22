using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.View;
using System;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class BuildingDockViewModel : ViewModel<BuildingDock>
    {
        public int ID { get { return Model.ID; } }
        public BuildingDockState State { get { return Model.State; } }
        public string Ship { get { return Model.Ship == null ? "-----" : Model.Ship.Name; } }
        public string RemainingTime { get { return Model.RemainingTime.HasValue ? Model.RemainingTime.Value.ToString(@"hh\:mm\:ss") : "--:--:--"; } }
        public string CompleteTime { get { return Model.CompleteTime.HasValue ? Model.CompleteTime.Value.LocalDateTime.ToString() : null; } }

        public int Fuel { get { return Model.Fuel; } }
        public int Bullet { get { return Model.Bullet; } }
        public int Steel { get { return Model.Steel; } }
        public int Bauxite { get { return Model.Bauxite; } }
        public int DevelopmentMaterial { get { return Model.DevelopmentMaterial; } }

        public BuildingDockViewModel(BuildingDock rpModel)
            : base(rpModel)
        {
            rpModel.BuildingCompleted += r =>
            {
                DispatcherUtil.UIDispatcher.BeginInvoke(new Action<BuildingDockCompletedEventArgs>(e =>
                {
                    new NotificationWindow()
                    {
                        Title = "建造完了",
                        Message = string.Format("「{0}」の建造が完了しました", r.ShipName),
                    }.Show();
                }), r);
            };

            PropertyChangedObservable.Subscribe(OnPropertyChanged);
        }
    }
}
