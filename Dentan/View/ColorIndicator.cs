using System.Windows.Media;

namespace Moen.KanColle.Dentan.View
{
    class ColorIndicator : Indicator
    {
        protected override void OnValueChanged(int rpValue)
        {
            base.OnValueChanged(rpValue);
            SetIndicatorColor();
        }
        protected override void OnMaximumChanged(int rpValue)
        {
            base.OnMaximumChanged(rpValue);
            SetIndicatorColor();
        }

        void SetIndicatorColor()
        {
            var rRadio = Value / (double)Maximum;
            if (rRadio <= 0.25)
                Foreground = Brushes.Red;
            else if (rRadio <= 0.5)
                Foreground = Brushes.Orange;
            else if (rRadio <= 0.75)
                Foreground = Brushes.Yellow;
            else
                Foreground = Brushes.Green;
        }
    }
}
