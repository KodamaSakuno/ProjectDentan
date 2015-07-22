using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PreferenceModel = Moen.KanColle.Dentan.Model.Preference;

namespace Moen.KanColle.Dentan.View
{
    /// <summary>
    /// FirstRunWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FirstRunWindow : Window
    {
        static IEqualityComparer<string> r_Comparer = new DelegatedEqualityComparer<string>((x, y) => x.Equals(y, StringComparison.OrdinalIgnoreCase), r => r.GetHashCode());

        public FirstRunWindow()
        {
            InitializeComponent();

            Loaded += FirstRunWindow_Loaded;
        }

        void FirstRunWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonConcatWithLogbook.Visibility = Visibility.Collapsed;
            ButtonConcatWithSGo.Visibility = Visibility.Collapsed;
            ButtonConcatWithEO.Visibility = Visibility.Collapsed;

            foreach (var rProcess in Process.GetProcesses().Select(r => r.ProcessName))
            {
                if (rProcess.Equals("LogBook", StringComparison.OrdinalIgnoreCase))
                    ButtonConcatWithLogbook.Visibility = Visibility.Visible;
                else if (rProcess.Equals("ShimakazeGo", StringComparison.OrdinalIgnoreCase))
                    ButtonConcatWithSGo.Visibility = Visibility.Visible;
                else if (rProcess.Equals("ElectronicObserver", StringComparison.OrdinalIgnoreCase))
                    ButtonConcatWithEO.Visibility = Visibility.Visible;
            }
        }

        public static bool IsRequired()
        {
            var rOtherTool = new string[]
            {
                "LogBook",
                "ElectronicObserver",
                "ShimakazeGo",
            };
            return Process.GetProcesses().Select(r => r.ProcessName).Intersect(rOtherTool, r_Comparer).Any();
        }

        void ButtonNothing_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void ButtonConcatWithLogbook_Click(object sender, RoutedEventArgs e)
        {
            PreferenceModel.Current.UpstreamProxy.Enabled = true;
            PreferenceModel.Current.UpstreamProxy.Port = 8888;
            Close();
        }

        void ButtonConcatWithSGo_Click(object sender, RoutedEventArgs e)
        {
            PreferenceModel.Current.UpstreamProxy.Enabled = true;
            PreferenceModel.Current.UpstreamProxy.Port = 8099;
            PreferenceModel.Current.UpstreamProxy.UseSSL = true;
            PreferenceModel.Current.Cache.Enabled = false;
            Close();
        }

        void ButtonConcatWithEO_Click(object sender, RoutedEventArgs e)
        {
            PreferenceModel.Current.UpstreamProxy.Enabled = true;
            PreferenceModel.Current.UpstreamProxy.Port = 40620;
            Close();
        }
    }
}
