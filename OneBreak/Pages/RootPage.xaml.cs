using System;
using OneBreak.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace OneBreak.Pages
{
    public sealed partial class RootPage
    {
        private bool _eventsRegistered;
        public RootPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            RootFrame.Navigate(typeof(NewsPage));
            InitializeNavigationItems();
            RegisterEvents();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            UnregisterEvents();
        }

        private void InitializeNavigationItems()
        {
            NewsLeftNavItem.NavigationPage = typeof(NewsPage);
        }

        private void HamburgerButton_OnClick(object sender, RoutedEventArgs e)
        {
            BaseSplitView.IsPaneOpen = !BaseSplitView.IsPaneOpen;
        }

        private void BaseSplitView_OnClosed(SplitView sender, object args)
        {
            HamburgerButton.IsChecked = false;
        }

        private void LeftNavItem_OnClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = e.ClickedItem as LeftNavItemControl;
            if (clickedItem?.NavigationPage == null) return;

            NavigateToPage(clickedItem.NavigationPage);
        }

        private void CommandBarNewsButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(NewsPage));
        }

        private void NavigateToPage(Type page)
        {
            if (RootFrame.CurrentSourcePageType == page) return;

            RootFrame.Navigate(page);
        }

        protected override void RegisterEvents()
        {
            if (_eventsRegistered) return;
            base.RegisterEvents();
            SystemNavigationManager.GetForCurrentView().BackRequested += RootPage_BackRequested;
            _eventsRegistered = true;
        }

        protected override void UnregisterEvents()
        {
            if (!_eventsRegistered) return;
            base.UnregisterEvents();
            SystemNavigationManager.GetForCurrentView().BackRequested -= RootPage_BackRequested;
            _eventsRegistered = false;
        }

        private void RootPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if(RootFrame.CanGoBack)
            {
                RootFrame.GoBack();
                e.Handled = true;
            }
        }
    }
}
