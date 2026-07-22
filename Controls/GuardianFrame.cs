using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Controls
{
    public class GuardianFrame : ContentControl
    {
        static GuardianFrame()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(GuardianFrame),
                new FrameworkPropertyMetadata(typeof(GuardianFrame)));
        }
    }
}