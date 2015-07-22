using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Moen.KanColle.Dentan.ViewModel
{
    public abstract class ViewModel<T> : ModelBase, IDisposable
        where T : ModelBase
    {
        public T Model { get; private set; }

        public IConnectableObservable<string> PropertyChangedObservable { get; private set; }
        IDisposable r_Subscriptions;

        protected ViewModel(T rpModel)
        {
            Model = rpModel;

            PropertyChangedObservable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                rpHandler => Model.PropertyChanged += rpHandler,
                rpHandler => Model.PropertyChanged -= rpHandler)
                .Select(r => r.EventArgs.PropertyName).Publish();
            r_Subscriptions = PropertyChangedObservable.Connect();
        }

        public void Dispose()
        {
            r_Subscriptions.Dispose();
        }
    }
}
