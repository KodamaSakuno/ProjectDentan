using System.Windows;

namespace Moen.KanColle.Dentan.View.Game
{
    /// <summary>
    /// EquipmentsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EquipmentsWindow : Window
    {
        public EquipmentsWindow()
        {
            InitializeComponent();

            DataContext = App.Root.Game.Equipments;
        }
    }
}
