using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace ArkPilot.Controls
{
    [ContentProperty(nameof(CardContent))]
    public partial class GuardianCard : UserControl
    {
        public GuardianCard()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(GuardianCard),
                new PropertyMetadata("Titre"));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty CardContentProperty =
            DependencyProperty.Register(
                nameof(CardContent),
                typeof(object),
                typeof(GuardianCard),
                new PropertyMetadata(null));

        public object? CardContent
        {
            get => GetValue(CardContentProperty);
            set => SetValue(CardContentProperty, value);
        }
    }
}