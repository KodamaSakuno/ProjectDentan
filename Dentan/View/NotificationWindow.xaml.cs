using Moen.SystemInterop;
using System;
using System.Linq;
using System.Windows;

namespace Moen.KanColle.Dentan.View
{
    /// <summary>
    /// NotificationWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NotificationWindow : Window
    {
        DateTime r_CreationTime;

        public static DependencyProperty MessageProperty;
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        static NotificationWindow()
        {
            MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(NotificationWindow), new FrameworkPropertyMetadata(string.Empty));
        }
        public NotificationWindow()
        {
            r_CreationTime = DateTime.Now;

            DataContext = this;

            InitializeComponent();
        }

        void DoubleAnimationCompleted(object sender, EventArgs e)
        {
            if (!IsMouseOver)
                Close();
        }

        public new void Show()
        {
            Topmost = true;

            base.Show();

            AdjustNotificationWindows();
        }

        void AdjustNotificationWindows()
        {
            var rScreenArea = default(NativeStructs.RECT);
            NativeMethods.User32.SystemParametersInfo(NativeConstants.SPI.SPI_GETWORKAREA, 0, ref rScreenArea, 0);
            Left = rScreenArea.Right;
            Top = rScreenArea.Bottom;

            var rLeft = rScreenArea.Width - ActualWidth - 6;
            var rTop = rScreenArea.Height;

            var rNotificationWindows = Application.Current.Windows.OfType<NotificationWindow>().OrderByDescending(r => r.r_CreationTime);
            foreach (var rWindow in rNotificationWindows.Take(3))
            {
                rTop = rTop - (int)ActualHeight - 6;

                rWindow.Left = rLeft;
                rWindow.Top = rTop;

                rWindow.Visibility = Visibility.Visible;
            }
            foreach (var rWindow in rNotificationWindows.Skip(3))
                rWindow.Close();
        }
    }
}
