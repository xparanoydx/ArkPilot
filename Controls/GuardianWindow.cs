using System.Windows;

namespace ArkPilot.Controls
{
    public class GuardianWindow : Window
    {
        static GuardianWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(GuardianWindow),
                new FrameworkPropertyMetadata(typeof(GuardianWindow)));
        }
    }
}