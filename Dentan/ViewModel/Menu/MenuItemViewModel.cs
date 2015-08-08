using Moen.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace Moen.KanColle.Dentan.ViewModel.Menu
{
    public class MenuItemViewModel : ModelBase
    {
        string r_Title;
        public string Title
        {
            get { return r_Title; }
            set
            {
                if (r_Title != value)
                {
                    r_Title = value;
                    OnPropertyChanged();
                }
            }
        }
        ICommand r_Command;
        public ICommand Command
        {
            get { return r_Command; }
            set
            {
                if (r_Command != value)
                {
                    r_Command = value;
                    OnPropertyChanged();
                }
            }
        }
        bool r_IsChecked;
        public bool IsChecked
        {
            get { return r_IsChecked; }
            set
            {
                if (r_IsChecked != value)
                {
                    r_IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        KeyGesture r_KeyGesture;
        public KeyGesture KeyGesture
        {
            get { return r_KeyGesture; }
            set
            {
                if (r_KeyGesture != value)
                {
                    r_KeyGesture = value;
                    InputGestureText = r_KeyGesture == null ? null : r_KeyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);

                    OnPropertyChanged(nameof(KeyGesture));
                }
            }
        }
        string r_InputGestureText;
        public string InputGestureText
        {
            get { return r_InputGestureText; }
            private set
            {
                if (r_InputGestureText != value)
                {
                    r_InputGestureText = value;
                    OnPropertyChanged(nameof(InputGestureText));
                }
            }
        }

        ObservableRangeCollection<object> r_Items;
        public ObservableRangeCollection<object> Items { get { return r_Items; } }

        public MenuItemViewModel(string rpText)
            : this(rpText, null) { }
        public MenuItemViewModel(string rpText, ICommand rpCommand)
            : this(rpText, rpCommand, null) { }
        public MenuItemViewModel(string rpText, ICommand rpCommand, KeyGesture rpKeyGesture)
        {
            Title = rpText;
            Command = rpCommand;
            KeyGesture = rpKeyGesture;

            r_Items = new ObservableRangeCollection<object>();

            var rItems = CreateMenuItems();
            if (rItems != null)
                r_Items.AddRange(rItems);
        }

        public virtual IEnumerable<object> CreateMenuItems() 
        {
            return null;
        }

        public void KeyBinding(InputBindingCollection rpBindings)
        {
            KeyBindingCore(rpBindings);

            foreach (var rMenu in Items.OfType<MenuItemViewModel>())
                rMenu.KeyBinding(rpBindings);
        }
        protected virtual void KeyBindingCore(InputBindingCollection rpBindings) { }
    }
}
