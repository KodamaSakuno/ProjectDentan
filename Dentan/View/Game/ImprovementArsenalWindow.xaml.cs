using Moen.KanColle.Dentan.ViewModel.Game;
using System;
using System.Windows;

namespace Moen.KanColle.Dentan.View.Game
{
    /// <summary>
    /// ImprovementArsenalWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ImprovementArsenalWindow : Window
    {
        public ImprovementArsenalWindow()
        {
            InitializeComponent();

            Loaded += (s, e) => DataContext = new ImprovementArsenalViewModel();
        }
    }
}
