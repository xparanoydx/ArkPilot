using System.Diagnostics;
using System.Windows;

namespace ArkPilot
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Github_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/xparanoydx/ArkPilot",
                UseShellExecute = true
            });
        }
    }
}