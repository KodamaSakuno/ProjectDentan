using Moen.KanColle.Dentan.Data.Raw;
using System.Threading;

namespace Moen.KanColle.Dentan.Api.Parser
{
    [Api("api_start2")]
    class ApiStartParser : ApiParser<RawApiStart>
    {
        public static ManualResetEventSlim SuccessEvent { get; private set; }

        static ApiStartParser()
        {
            SuccessEvent = new ManualResetEventSlim(false);
        }

        public override void Process(RawApiStart rpData)
        {
            if (ResultCode == 201)
            {
                Game.RaiseTokenOutdatedEvent();

                return;
            }

            Game.UpdateBaseInfo(rpData);

            SuccessEvent.Set();
        }
    }
}
