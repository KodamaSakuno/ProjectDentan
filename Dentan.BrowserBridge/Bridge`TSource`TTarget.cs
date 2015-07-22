using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.Browser.Bridge
{
    public class Bridge<TSource, TTarget>
    {
        ServiceHost r_Host;
        ChannelFactory<TTarget> r_ChannelFactory;
        NetNamedPipeBinding r_Binding;

        public TTarget Proxy { get; private set; }

        public Bridge(object rpInstance, string rpUrl, string rpAddress)
        {
            r_Binding = new NetNamedPipeBinding();
            r_Binding.ReceiveTimeout = TimeSpan.MaxValue;

            r_Host = new ServiceHost(rpInstance, new[] { new Uri(rpUrl) });
            r_Host.AddServiceEndpoint(typeof(TSource), r_Binding, rpAddress);
            r_Host.Open();
        }

        public void Connect(string rpDestination)
        {
            if (r_ChannelFactory == null)
                r_ChannelFactory = new ChannelFactory<TTarget>(r_Binding, new EndpointAddress(rpDestination));

            Proxy = r_ChannelFactory.CreateChannel();
        }

        public void Post(Action rpAction)
        {
            Task.Run(rpAction);
        }
    }
}
