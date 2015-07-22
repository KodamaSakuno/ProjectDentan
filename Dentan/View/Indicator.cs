using System.Windows;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.View
{
    class Indicator : Control
    {
        public static readonly DependencyProperty MaximumProperty;
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty;
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MedianProperty;
        public int Median
        {
            get { return (int)GetValue(MedianProperty); }
            set { SetValue(MedianProperty, value); }
        }

        public static readonly DependencyProperty ShowBlinkingProperty;
        public bool ShowBlinking
        {
            get { return (bool)GetValue(ShowBlinkingProperty); }
            private set { SetValue(ShowBlinkingProperty, value); }
        }

        FrameworkElement r_Track;
        FrameworkElement r_Indicator;
        FrameworkElement r_MedianIndicator;

        static Indicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Indicator), new FrameworkPropertyMetadata(typeof(Indicator)));

            MaximumProperty = DependencyProperty.Register("Maximum", typeof(int), typeof(Indicator),
                new FrameworkPropertyMetadata(100, 
                    (s, e) => ((Indicator)s).OnMaximumChanged((int)e.NewValue)));
            ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(Indicator),
                new FrameworkPropertyMetadata(0,
                    (s, e) => ((Indicator)s).OnValueChanged((int)e.NewValue), 
                    (s, rpValue) => ((int)rpValue).Clamp(0, ((Indicator)s).Maximum)));
            MedianProperty = DependencyProperty.Register("Median", typeof(int), typeof(Indicator),
                new FrameworkPropertyMetadata(0,
                    (s, e) => ((Indicator)s).OnMedianChanged((int)e.NewValue)));

            ShowBlinkingProperty = DependencyProperty.Register("ShowBlinking", typeof(bool), typeof(Indicator),
                new FrameworkPropertyMetadata(false));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (r_Track != null)
                r_Track.SizeChanged -= Track_SizeChanged;

            r_Indicator = (FrameworkElement)Template.FindName("PART_Indicator", this);
            r_Track = (FrameworkElement)Template.FindName("PART_Track", this);
            r_MedianIndicator = (FrameworkElement)Template.FindName("PART_MedianIndicator", this);

            if (r_Track != null)
                r_Track.SizeChanged += Track_SizeChanged;
        }

        void Track_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetIndicatorLength(Value);
            SetMedianIndicatorLength(Median);
        }
        protected virtual void OnMaximumChanged(int rpValue)
        {
            SetIndicatorLength(Value);
        }
       protected virtual  void OnValueChanged(int rpValue)
        {
            //ShowBlinking = rpValue < Median;
            SetIndicatorLength(rpValue);
        }
        protected virtual void OnMedianChanged(int rpValue)
        {
            //ShowBlinking = rpValue > Value;
            SetMedianIndicatorLength(rpValue);
        }

        void SetMedianIndicatorLength(int rpValue)
        {
            if (r_MedianIndicator != null && Maximum != 0)
                r_MedianIndicator.Width = rpValue / (double)Maximum * r_Track.ActualWidth;
        }

        void SetIndicatorLength(int rpValue)
        {
            if (r_Track != null && r_Indicator != null && Maximum != 0)
                r_Indicator.Width = rpValue / (double)Maximum * r_Track.ActualWidth;
        }
    }
}
