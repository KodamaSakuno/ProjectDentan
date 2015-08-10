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
                r_Message = $"{DateTime.Now}: {value.Replace(Environment.NewLine, " ")}";
                OnPropertyChanged(nameof(Message));
            }
        }

        public StatusBarViewModel()
        {
            r_Message = "(´･_･`)";

            ApiParsers.NewException += ApiParsers_NewException;
        }

        void ApiParsers_NewException(Exception e)
        {
            Message = "Exception: " + e.Message;
        }
    }
}
