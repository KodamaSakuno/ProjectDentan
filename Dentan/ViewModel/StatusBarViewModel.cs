using Moen.KanColle.Dentan.Api;
using System;
namespace Moen.KanColle.Dentan.ViewModel
{
    public class StatusBarViewModel : ModelBase
    {
        string r_Message;
        public string Message
        {
            get { return r_Message; }
            set
            {
                var rMessage = value.Replace(Environment.NewLine, " ");
                if (r_Message != rMessage)
                {
                    r_Message = rMessage;
                    OnPropertyChanged();
                }
            }
        }

        public StatusBarViewModel()
        {
            Message = "(´･_･`)";

            ApiParsers.NewException += ApiParsers_NewException;
        }

        void ApiParsers_NewException(Exception e)
        {
            Message = "Exception: " + e.Message;
        }
    }
}
