using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.View;
using System;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class RepairDockViewModel : ViewModel<RepairDock>
    {
        public int ID { get { return Model.ID; } }
        public RepairDockState State { get { return Model.State; } }
        public string Ship { get { return Model.Ship == null ? "-----" : Model.Ship.Info.Name; } }
        public string RemainingTime { get { return Model.RemainingTime.HasValue ? Model.RemainingTime.Value.ToString(@"hh\:mm\:ss") : "--:--:--"; } }
        public string CompleteTime { get { return Model.CompleteTime.HasValue ? Model.CompleteTime.Value.LocalDateTime.ToString() : null; } }

        public RepairDockViewModel(RepairDock rpModel)
            : base(rpModel)
        {
            rpModel.RepairCompleted += r =>
            {
                DispatcherUtil.UIDispatcher.BeginInvoke(new Action<RepairDockCompletedEventArgs>(e =>
                {
                    new NotificationWindow()
                    {
                        Title = "入渠完了",
                        Message = string.Format("「{0}」の入渠が完了しました", r.ShipName),
                    }.Show();
                }), r);
            };

            PropertyChangedObservable.Subscribe(OnPropertyChanged);
        }
    }
}
