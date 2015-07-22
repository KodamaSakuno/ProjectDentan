using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View.Game
{
    [ViewID("FleetsOverview")]
    class FleetsOverviewView : Control
    {
        static FleetsOverviewView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FleetsOverviewView), new FrameworkPropertyMetadata(typeof(FleetsOverviewView)));
        }
    }
}
