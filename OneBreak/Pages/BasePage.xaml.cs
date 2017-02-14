using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneBreak.Pages
{
    public partial class BasePage : Page
    {
        private bool _eventsRegistered;

        public BasePage()
        {
            this.InitializeComponent();
        }

        protected virtual void RegisterEvents()
        {
            //if (_eventsRegistered) return;
            //SystemNavigationManager.GetForCurrentView().BackRequested += BasePage_BackRequested;
            //_eventsRegistered = true;
        }

        private void BasePage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack && !e.Handled)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        protected virtual void UnregisterEvents()
        {
            //if (!_eventsRegistered) return;
            //SystemNavigationManager.GetForCurrentView().BackRequested -= BasePage_BackRequested;
            //_eventsRegistered = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }
    }
}
