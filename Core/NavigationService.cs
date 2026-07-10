using System.Windows.Controls;

namespace ArkPilot.Core
{
    public class NavigationService
    {
        private readonly Frame frame;

        public NavigationService(Frame frame)
        {
            this.frame = frame;
        }

        public void Navigate(Page page)
        {
            frame.Navigate(page);
        }

        public void GoBack()
        {
            if (frame.CanGoBack)
                frame.GoBack();
        }
    }
}