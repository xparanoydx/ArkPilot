using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Controls
{
    public enum GuardianCornerPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    public class GuardianCorner : Control
    {
        static GuardianCorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(GuardianCorner),
                new FrameworkPropertyMetadata(typeof(GuardianCorner)));
        }

        public static readonly DependencyProperty PositionProperty =
    DependencyProperty.Register(
        nameof(Position),
        typeof(GuardianCornerPosition),
        typeof(GuardianCorner),
        new FrameworkPropertyMetadata(
            GuardianCornerPosition.TopLeft,
            FrameworkPropertyMetadataOptions.AffectsRender));

        public GuardianCornerPosition Position
        {
            get => (GuardianCornerPosition)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }
    }
}