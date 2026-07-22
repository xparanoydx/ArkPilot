using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArkPilot.Controls
{
    public partial class GuardianButton : UserControl
    {
        public GuardianButton()
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
                typeof(GuardianButton),
                new PropertyMetadata(""));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(GuardianButton),
                new PropertyMetadata("Bouton"));

        public Brush BackgroundBrush
        {
            get => (Brush)GetValue(BackgroundBrushProperty);
            set => SetValue(BackgroundBrushProperty, value);
        }

        public static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register(
                nameof(BackgroundBrush),
                typeof(Brush),
                typeof(GuardianButton),
                new PropertyMetadata(Brushes.Transparent));

        public Brush ForegroundBrush
        {
            get => (Brush)GetValue(ForegroundBrushProperty);
            set => SetValue(ForegroundBrushProperty, value);
        }

        public static readonly DependencyProperty ForegroundBrushProperty =
            DependencyProperty.Register(
                nameof(ForegroundBrush),
                typeof(Brush),
                typeof(GuardianButton),
                new PropertyMetadata(Brushes.White));

        public Brush AccentBrush
        {
            get => (Brush)GetValue(AccentBrushProperty);
            set => SetValue(AccentBrushProperty, value);
        }

        public static readonly DependencyProperty AccentBrushProperty =
            DependencyProperty.Register(
                nameof(AccentBrush),
                typeof(Brush),
                typeof(GuardianButton),
                new PropertyMetadata(Brushes.DeepSkyBlue));
    }
}