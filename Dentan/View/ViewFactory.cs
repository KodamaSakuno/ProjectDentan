using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Moen.KanColle.Dentan.View
{
    static class ViewFactory
    {
        static Dictionary<string, Type> r_TypeMaps;
        static Dictionary<Type, string> r_GameVMBinding;

        public static HashSet<string> LoadedIDs { get; } = new HashSet<string>();
        static Dictionary<string, FrameworkElement> r_LoadedElements = new Dictionary<string, FrameworkElement>();

        static ViewFactory()
        {
            var rAssembly = typeof(ViewFactory).Assembly;
            var rTypes = from rType in rAssembly.GetTypes()
                         where rType.IsSubclassOf(typeof(FrameworkElement))
                         let rAttribute = rType.GetCustomAttributes(typeof(ViewIDAttribute), false).OfType<ViewIDAttribute>().SingleOrDefault()
                         where rAttribute != null
                         select new { ID = rAttribute.ID, Type = rType };
            r_TypeMaps = rTypes.ToDictionary(r => r.ID, r => r.Type);

            var rTypes2 = from rType in r_TypeMaps.Values
                          let rAttribute = rType.GetCustomAttributes(typeof(GameVMBindingAttribute), false).OfType<GameVMBindingAttribute>().SingleOrDefault()
                          where rAttribute != null
                          select new { Type = rType, BindingPath = rAttribute.Path };
            r_GameVMBinding = rTypes2.ToDictionary(r => r.Type, r => r.BindingPath);
        }
        
        public static FrameworkElement GetContentFromID(string rpID)
        {
            Type rType;
            if (!r_TypeMaps.TryGetValue(rpID, out rType))
                return null;

            if (LoadedIDs.Add(rpID))
            {
                FrameworkElement rElement;
                if (!r_LoadedElements.TryGetValue(rpID, out rElement))
                {
                    rElement = (FrameworkElement)Activator.CreateInstance(rType);
                    r_LoadedElements.Add(rpID, rElement);
                }

                string rBindingPath;
                if (!r_GameVMBinding.TryGetValue(rElement.GetType(), out rBindingPath))
                    rElement.DataContext = App.Root.Game;
                else
                    rElement.SetBinding(FrameworkElement.DataContextProperty, new Binding(rBindingPath) { Source = App.Root.Game });

                return rElement;
            }

            return null;
        }
    }
}
