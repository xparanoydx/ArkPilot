using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Controls.Guardian
{
    public class GuardianPanel : ContentControl
    {
        static GuardianPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(GuardianPanel),
                new FrameworkPropertyMetadata(typeof(GuardianPanel)));
        }
    }
}