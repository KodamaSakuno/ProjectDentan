using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Game
{
    [ViewID("Fleets")]
    public class FleetsView : Control
    {
        static FleetsView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FleetsView), new FrameworkPropertyMetadata(typeof(FleetsView)));
        }
    }
}
