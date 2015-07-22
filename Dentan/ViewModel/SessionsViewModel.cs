using Moen.KanColle.Dentan.Proxy;
using System;
using System.Collections.ObjectModel;

namespace Moen.KanColle.Dentan.ViewModel
{
    public class SessionsViewModel : ModelBase
    {
        ObservableCollection<Session> r_Sessions;
        public ReadOnlyObservableCollection<Session> Sessions { get; private set; }

        public SessionsViewModel()
        {
            r_Sessions = new ObservableCollection<Session>();
            Sessions = new ReadOnlyObservableCollection<Session>(r_Sessions);

            KanColleGame.Current.Proxy.NewSession += r => DispatcherUtil.UIDispatcher.BeginInvoke(new Action<Session>(r_Sessions.Add), r);
        }
    }
}
