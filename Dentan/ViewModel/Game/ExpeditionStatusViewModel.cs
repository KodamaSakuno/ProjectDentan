using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.View;
using System;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class ExpeditionStatusViewModel : ViewModel<Expedition>
    {
        public int ID { get { return Model.Fleet.ID; } }
        public Fleet Fleet { get { return Model.Fleet; } }
        public int ExpeditionID { get { return Model.ExpeditionID; } }
        public string Name { get { return Model.Info == null ? "-----" : Model.Info.Name; } }
        public string RemainingTime { get { return Model.RemainingTime.HasValue ? Model.RemainingTime.Value.ToString(@"hh\:mm\:ss") : "--:--:--"; } }
        public string CompleteTime { get { return Model.CompleteTime.HasValue ? Model.CompleteTime.Value.LocalDateTime.ToString() : null; } }

        public ExpeditionStatusViewModel(Expedition rpModel)
            : base(rpModel)
        {
            rpModel.ExpeditionReturned += r =>
            {
                DispatcherUtil.UIDispatcher.BeginInvoke(new Action<ExpeditionReturnedEventArgs>(e =>
                {
                    new NotificationWindow()
                    {
                        Title = "遠征帰投",
                        Message = string.Format("「{0}」が遠征「{1}」から帰投しました", r.FleetName, r.ExpeditionName),
                    }.Show();
                }), r);
            };

            PropertyChangedObservable.Subscribe(OnPropertyChanged);
        }
    }
}
