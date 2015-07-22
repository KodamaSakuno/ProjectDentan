using System;

namespace Moen.KanColle.Dentan.View
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewIDAttribute : Attribute
    {
        public string ID { get; private set; }

        public ViewIDAttribute(string rpID)
        {
            ID = rpID;
        }
    }
}
