using System;

namespace Moen.KanColle.Dentan.View
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GameVMBindingAttribute : Attribute
    {
        public string Path { get; private set; }

        public GameVMBindingAttribute(string rpPath)
        {
            Path = rpPath;
        }
    }
}
