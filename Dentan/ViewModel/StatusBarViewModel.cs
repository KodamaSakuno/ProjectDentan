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
                if (r_Message != value)
                {
                    r_Message = value;
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
