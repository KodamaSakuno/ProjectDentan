using Moen.Collections;
using Moen.KanColle.Dentan.ViewModel.Preferences;
using Moen.UserInterface;
using System.Windows;

namespace Moen.KanColle.Dentan.ViewModel
{
    public class MainWindowViewModel : WindowViewModel
    {
        public MenuBarViewModel Menu { get; private set; }

        public ObservableRangeCollection<PaneViewModel> Panes { get; private set; }

        public StatusBarViewModel StatusBar { get; private set; }

        public GameViewModel Game { get; private set; }

        public PreferenceViewModel Preference { get; private set; }

        public WindowState WindowState
        {
            get { return Preference.Model.Window.State; }
            set
            {
                if (Preference.Model.Window.State != value)
                {
                    Preference.Model.Window.State = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Top
        {
            get { return Preference.Model.Window.Top; }
            set
            {
                if (Preference.Model.Window.Top != value)
                {
                    Preference.Model.Window.Top = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Left
        {
            get { return Preference.Model.Window.Left; }
            set
            {
                if (Preference.Model.Window.Left != value)
                {
                    Preference.Model.Window.Left = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Width
        {
            get { return Preference.Model.Window.Width; }
            set
            {
                if (Preference.Model.Window.Width != value)
                {
                    Preference.Model.Window.Width = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Height
        {
            get { return Preference.Model.Window.Height; }
            set
            {
                if (Preference.Model.Window.Height != value)
                {
                    Preference.Model.Window.Height = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindowViewModel()
        {
            Title = "Project Dentan（暂定）";
            Game = new GameViewModel();

            Menu = new MenuBarViewModel();

            StatusBar = new StatusBarViewModel();

            Preference = new PreferenceViewModel();

            Panes = new ObservableRangeCollection<PaneViewModel>();
        }
    }
}
