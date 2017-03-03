using OneBreak.Helpers;
using OneBreak.Models;
using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneBreak.Pages
{
    public sealed partial class NewsDetailPage : BasePage
    {
        public NewsModel CurrentNews { get; set; }

        private bool _onConnectedRegistered, _backButtonRegistered;

        public NewsDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CurrentNews = App.NewsViewModel.LastSelectedNews;

            LoadNewsBody();

            RegisterBackButton();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            UnregisterOnConnected();
            UnregisterBackButton();
        }

        private async Task LoadNewsBody()
        {
            if (!string.IsNullOrEmpty(CurrentNews.NewsBody) || CurrentNews.Loading) return;
            if (ConnectionHelper.IsConnected)
            {
                await CurrentNews.LoadNewsBody();
            }
            else
            {
                RegisterOnConnected();
            }
        }

        #region Events
        private void RegisterOnConnected()
        {
            if (_onConnectedRegistered) return;

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;

            _onConnectedRegistered = true;
        }

        private void UnregisterOnConnected()
        {
            if (!_onConnectedRegistered) return;

            NetworkInformation.NetworkStatusChanged -= NetworkInformation_NetworkStatusChanged;

            _onConnectedRegistered = false;
        }

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (!ConnectionHelper.IsConnected) return;

            LoadNewsBody();

            UnregisterOnConnected();
        }

        private void RegisterBackButton()
        {
            if (_backButtonRegistered) return;

            var navigationManager = SystemNavigationManager.GetForCurrentView();
            navigationManager.BackRequested += NavigationManager_BackRequested;
            if(DeviceFamilyHelper.CurrentDeviceFamily == "Desktop")
            {
                navigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }

            _backButtonRegistered = true;
        }

        private void UnregisterBackButton()
        {
            if (!_backButtonRegistered) return;
            var navigationManager = SystemNavigationManager.GetForCurrentView();
            navigationManager.BackRequested -= NavigationManager_BackRequested;

            if (DeviceFamilyHelper.CurrentDeviceFamily == "Desktop")
            {
                navigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

            _backButtonRegistered = false;
        }

        private void NavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }
        #endregion

        private void NewsDetailPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if(e.NewSize.Width >= 720)
            {
                if(Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        private void ViewOriginalButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentNews.OriginalUrl)) return;
            CurrentNews.LoadingFailed = false;

            var webView = NewsWebView ?? (WebView)FindName("NewsWebView");

            if (webView == null)
            {
                CurrentNews.LoadingFailed = true;
                return;
            }

            webView.Visibility = Windows.UI.Xaml.Visibility.Visible;
            webView.Navigate(new Uri(CurrentNews.OriginalUrl));
        }
    }
}
