using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArkPilot.Controls
{
    public partial class StatusBadge : UserControl
    {
        public StatusBadge()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => StatusText.Text;
            set => StatusText.Text = value;
        }

        public Brush BadgeColor
        {
            get => BadgeBorder.Background;
            set => BadgeBorder.Background = value;
        }
    }
}