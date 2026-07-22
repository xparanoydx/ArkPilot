using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArkPilot.Controls
{
    public partial class GuardianStatCard : UserControl
    {
        public GuardianStatCard()
        {
            InitializeComponent();
        }

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(string),
                typeof(GuardianStatCard),
                new PropertyMetadata(""));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(GuardianStatCard),
                new PropertyMetadata(""));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(string),
                typeof(GuardianStatCard),
                new PropertyMetadata("--"));

        public string Footer
        {
            get => (string)GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }

        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register(
                nameof(Footer),
                typeof(string),
                typeof(GuardianStatCard),
                new PropertyMetadata(""));

        public Brush AccentBrush
        {
            get => (Brush)GetValue(AccentBrushProperty);
            set => SetValue(AccentBrushProperty, value);
        }

        public static readonly DependencyProperty AccentBrushProperty =
            DependencyProperty.Register(
                nameof(AccentBrush),
                typeof(Brush),
                typeof(GuardianStatCard),
                new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty ValueBrushProperty =
    DependencyProperty.Register(
        nameof(ValueBrush),
        typeof(Brush),
        typeof(GuardianStatCard),
        new PropertyMetadata(Brushes.White));

        public Brush ValueBrush
        {
            get => (Brush)GetValue(ValueBrushProperty);
            set => SetValue(ValueBrushProperty, value);
        }

        public static readonly DependencyProperty IconFontSizeProperty =
    DependencyProperty.Register(
        nameof(IconFontSize),
        typeof(double),
        typeof(GuardianStatCard),
        new PropertyMetadata(28d));

        public double IconFontSize
        {
            get => (double)GetValue(IconFontSizeProperty);
            set => SetValue(IconFontSizeProperty, value);
        }
    }
}