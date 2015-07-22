using Moen.KanColle.Dentan.Data;
using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View
{
    class EquipmentIcon : Control
    {
        public static readonly DependencyProperty TypeProperty;
        public EquipmentIconType Type
        {
            get { return (EquipmentIconType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public static readonly DependencyProperty IDProperty;
        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        static EquipmentIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EquipmentIcon), new FrameworkPropertyMetadata(typeof(EquipmentIcon)));

            TypeProperty = DependencyProperty.Register("Type", typeof(EquipmentIconType), typeof(EquipmentIcon), new UIPropertyMetadata(EquipmentIconType.None));
            IDProperty = DependencyProperty.Register("ID", typeof(int), typeof(EquipmentIcon), new UIPropertyMetadata(0));
        }
    }
}
