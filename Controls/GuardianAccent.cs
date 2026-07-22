using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Controls
{
    public class GuardianAccent : Control
    {
        static GuardianAccent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(GuardianAccent),
                new FrameworkPropertyMetadata(typeof(GuardianAccent)));
        }

        public static readonly DependencyProperty TypeProperty =
    DependencyProperty.Register(
        nameof(Type),
        typeof(GuardianAccentType),
        typeof(GuardianAccent),
        new FrameworkPropertyMetadata(
            GuardianAccentType.Line,
            FrameworkPropertyMetadataOptions.AffectsRender));

        public GuardianAccentType Type
        {
            get => (GuardianAccentType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
    }
}