using System.Windows;

namespace ArkPilot.Core
{
    public class DialogService
    {
        public void Information(
            string message,
            string title = "Information")
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public void Error(
            string message,
            string title = "Erreur")
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public bool Confirm(
            string message,
            string title = "Confirmation")
        {
            return MessageBox.Show(
                message,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes;
        }
    }
}