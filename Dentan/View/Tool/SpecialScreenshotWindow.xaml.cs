using Moen.KanColle.Dentan.ViewModel.Browser;
using Moen.KanColle.Dentan.ViewModel.Tool;
using Moen.SystemInterop;
using System;
using System.Windows;
using System.Windows.Interop;

namespace Moen.KanColle.Dentan.View.Tool
{
    /// <summary>
    /// SpecialScreenshotWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SpecialScreenshotWindow : Window
    {
        public SpecialScreenshotWindow()
        {
            InitializeComponent();
            
            Loaded += SpecialScreenshotWindow_Loaded;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var rBrowser = BrowserViewModel.Current.BrowserControl as HwndHost;
            if (rBrowser == null)
                return;

            NativeStructs.RECT rRect;
            NativeMethods.User32.GetWindowRect(rBrowser.Handle, out rRect);

            Left = rRect.Left;
            Top = rRect.Bottom;
        }

        void SpecialScreenshotWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new SpecialScreenshotViewModel();
        }
    }
}
