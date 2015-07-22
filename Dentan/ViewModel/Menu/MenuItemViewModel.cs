using Moen.Collections;
using System.Collections.Generic;
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

        ObservableRangeCollection<object> r_Items;
        public ObservableRangeCollection<object> Items { get { return r_Items; } }

        public MenuItemViewModel(string rpText)
            : this(rpText, null) { }
        public MenuItemViewModel(string rpText, ICommand rpCommand)
        {
            Title = rpText;
            Command = rpCommand;

            r_Items = new ObservableRangeCollection<object>();

            var rItems = CreateMenuItems();
            if (rItems != null)
                r_Items.AddRange(rItems);
        }

        public virtual IEnumerable<object> CreateMenuItems() 
        {
            return null;
        }
    }
}
