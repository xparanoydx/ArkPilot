using System.Windows;

namespace ArkPilot.Views
{
    public partial class TransferProgressWindow : Window
    {
        public TransferProgressWindow()
        {
            InitializeComponent();
        }

        public void SetFile(string file)
        {
            FileText.Text = file;
        }

        public void SetProgress(double percent)
        {
            Progress.Value = percent;

            PercentText.Text = $"{percent:0}%";
        }
    }
}