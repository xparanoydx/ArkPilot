using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Controls
{
    public partial class PageHeader : UserControl
    {
        public PageHeader()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => TitleText.Text;
            set => TitleText.Text = value;
        }

        public string Subtitle
        {
            get => SubtitleText.Text;
            set => SubtitleText.Text = value;
        }

        public string Status
        {
            get => StatusText.Text;
            set => StatusText.Text = value;
        }
    }
}