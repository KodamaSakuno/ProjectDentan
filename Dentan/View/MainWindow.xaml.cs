using Moen.KanColle.Dentan.ViewModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace Moen.KanColle.Dentan.View
{
    public partial class MainWindow : Window
    {
        const string DockConfigFile = @"Preference\Dock.config";

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.Root.Menu.KeyBinding(InputBindings);

            LoadLayout(DockConfigFile);
        }
        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveLayout(DockConfigFile);
        }

        public void LoadLayout(string rpFileName)
        {
            var rSerializer = new XmlLayoutSerializer(DockingManager);
            rSerializer.LayoutSerializationCallback += Serializer_LayoutSerializationCallback;

            if (File.Exists(rpFileName))
            {
                var rDirectory = Path.GetDirectoryName(rpFileName);
                if (!Directory.Exists(rDirectory))
                    Directory.CreateDirectory(rDirectory);

                rSerializer.Deserialize(rpFileName);
            }
        }
        public void LoadLayout(Stream rpStream)
        {
            var rSerializer = new XmlLayoutSerializer(DockingManager);
            rSerializer.LayoutSerializationCallback += Serializer_LayoutSerializationCallback;

            ViewFactory.LoadedIDs.Clear();
            App.Root.Panes.Clear();
            rSerializer.Deserialize(rpStream);
        }
        public void SaveLayout(string rpFileName)
        {
            var rSerializer = new XmlLayoutSerializer(DockingManager);

            var rDirectory = Path.GetDirectoryName(rpFileName);
            if (!Directory.Exists(rDirectory))
                Directory.CreateDirectory(rDirectory);

            rSerializer.Serialize(rpFileName);
        }
        public void SaveLayout(Stream rpStream)
        {
            var rSerializer = new XmlLayoutSerializer(DockingManager);

            rSerializer.Serialize(rpStream);
        }

        void Serializer_LayoutSerializationCallback(object sender, LayoutSerializationCallbackEventArgs e)
        {
            if (e.Model.ContentId == "EnemyFleet")
                e.Model.ContentId = "CompassData";

            var rPane = new PaneViewModel() { Content = ViewFactory.GetContentFromID(e.Model.ContentId), ContentID = e.Model.ContentId, Title = e.Model.Title };
            e.Content = rPane;
            App.Root.Panes.Add(rPane);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show(this, "要关闭 Project Dentan 吗？", "Project Dentan", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No) == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            base.OnClosing(e);
        }
    }
}
