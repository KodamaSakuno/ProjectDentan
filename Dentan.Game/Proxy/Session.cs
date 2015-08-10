using System;
using System.Diagnostics;
using FiddlerSession = Fiddler.Session;

namespace Moen.KanColle.Dentan.Proxy
{
    public enum SessionStatus { Request, Responsed, Cached, LoadedFromCache, Error }

    public class Session : ModelBase
    {
        internal Stopwatch Stopwatch { get; set; }
        internal FiddlerSession FiddlerSession { get; set; }

        SessionStatus r_Status;
        public SessionStatus Status
        {
            get { return r_Status; }
            set
            {
                if (r_Status != value)
                {
                    r_Status = value;
                    OnPropertyChanged();
                }
            }
        }
        int? r_StatusCode;
        public int? StatusCode
        {
            get { return r_StatusCode; }
            set
            {
                if (r_StatusCode != value)
                {
                    r_StatusCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FullUrl { get; private set; }
        public string Url { get; private set; }

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

        long? r_ContentLength;
        public long? ContentLength
        {
            get { return r_ContentLength; }
            set
            {
                if (r_ContentLength != value)
                {
                    r_ContentLength = value;
                    OnPropertyChanged();
                }
            }
        }

        long r_LoadedBytes;
        public long LoadedBytes
        {
            get { return r_LoadedBytes; }
            set
            {
                if (r_LoadedBytes != value)
                {
                    r_LoadedBytes = value;
                    OnPropertyChanged();
                }
            }
        }

        string r_TotalTime;
        public string TotalTime
        {
            get { return r_TotalTime; }
            set
            {
                if (r_TotalTime != value)
                {
                    r_TotalTime = value;
                    OnPropertyChanged();
                }
            }
        }
        string r_ParseTime;
        public string ParseTime
        {
            get { return r_ParseTime; }
            set
            {
                if (r_ParseTime != value)
                {
                    r_ParseTime = value;
                    OnPropertyChanged();
                }
            }
        }

        Exception r_Exception;
        public Exception Exception
        {
            get { return r_Exception; }
            set
            {
                if (r_Exception != value)
                {
                    r_Exception = value;
                    OnPropertyChanged();
                }
            }
        }

        string r_Name;
        public string Name
        {
            get { return r_Name; }
            set
            {
                if (r_Name != value)
                {
                    r_Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public Session(string rpUrl)
            : this(rpUrl, rpUrl) { }
        public Session(string rpFullUrl, string rpUrl)
        {
            FullUrl = rpFullUrl;
            Url = rpUrl;
        }
    }
}
