using System;

namespace Moen.KanColle.Dentan.Api
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ApiAttribute : Attribute
    {
        public string Path { get; private set; }

        public ApiAttribute(string rpPath)
        {
            Path = rpPath;
        }
    }
}
