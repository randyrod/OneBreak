using OneBreak.Helpers;
using OneBreak.Models;
using OneBreak.ViewModels;
using System;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneBreak.Pages
{
    public sealed partial class NewsPage
    {
        public NewsViewModel NewsViewModel { get; set; }

        private bool _onConnectedRegistered, _registeredForNewsBody;

        private const double MasterColumnWidth = 460;

        public NewsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NewsViewModel = App.NewsViewModel;
            if (NewsViewModel.LastSelectedNews == null)
            {
                //If it is null, binding doesn't work, as the progress bar has nothing to bind to and defaults to visible
                NewsViewModel.LastSelectedNews = new NewsModel();
            }

            if (ConnectionHelper.IsConnected && !NewsViewModel.Loading)
            {
                LoadNews();
            }
            else
            {
                RegisterOnConnectedEvent();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            UnregisterOnConnectedEvent();
        }

        private async void LoadNews()
        {
            await App.NewsViewModel.LoadNews();

            await App.NewsViewModel.LoadStarredNews();
        }

        private async void NewsItem_OnClick(object sender, ItemClickEventArgs e)
        {
            var news = e.ClickedItem as NewsModel;

            if (news == null) return;

            if(NewsWebView != null && NewsWebView.Visibility == Windows.UI.Xaml.Visibility.Visible)
            {
                NewsWebView.Navigate(new Uri("about:blank"));
                NewsWebView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            NewsViewModel.LastSelectedNews = news;

            if (VisualStateTriggers.CurrentState == MobileState)
            {
                Frame.Navigate(typeof(NewsDetailPage));
                return;
            }

            if (ConnectionHelper.IsConnected)
            {
                if ((news.NewsContent == null || news.NewsContent.Count <= 0) && !news.Loading)
                {
                    await NewsViewModel.LastSelectedNews.LoadNewsBody();
                }
            }
            else
            {
                NewsViewModel.LastSelectedNews = news;
                RegisterOnConnectedEvent();
                _registeredForNewsBody = true;
            }
        }

        private void NewsItemControl_OnStarredToggleClick(NewsModel news)
        {
            if (news == null) return;

            NewsViewModel.StarUnstarNews(news);
        }

        #region Events
        private void RegisterOnConnectedEvent()
        {
            if (_onConnectedRegistered) return;

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            _onConnectedRegistered = true;
        }

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (!ConnectionHelper.IsConnected) return;

            if (_registeredForNewsBody)
            {
                if (NewsViewModel.LastSelectedNews.NewsContent == null ||
                    NewsViewModel.LastSelectedNews.NewsContent.Count <= 0)
                {
                    NewsViewModel.LastSelectedNews.LoadNewsBody();
                    _registeredForNewsBody = false;
                }
            }
            else
            {
                LoadNews();
            }

            UnregisterOnConnectedEvent();
        }

        private void UnregisterOnConnectedEvent()
        {
            if (!_onConnectedRegistered) return;

            NetworkInformation.NetworkStatusChanged -= NetworkInformation_NetworkStatusChanged;

            _onConnectedRegistered = false;
        }
        #endregion

        private void ViewOriginalButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewsViewModel.LastSelectedNews.OriginalUrl)) return;
            NewsViewModel.LastSelectedNews.LoadingFailed = false;

            WebView webView;
            
            if (NewsWebView == null || NewsWebView.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                webView = (WebView)FindName("NewsWebView");
            }
            else
            {
                webView = NewsWebView;
            }

            if (webView == null)
            {
                NewsViewModel.LastSelectedNews.LoadingFailed = true;
                return;
            }

            webView.Visibility = Windows.UI.Xaml.Visibility.Visible;
            webView.Navigate(new Uri(NewsViewModel.LastSelectedNews.OriginalUrl));
        }

        private void VisualStateTriggers_CurrentStateChanged(object sender, Windows.UI.Xaml.VisualStateChangedEventArgs e)
        {
            if(e.NewState == MobileState && (e.OldState == DesktopState || e.OldState == DesktopState1) 
                && (NewsViewModel.LastSelectedNews.NewsContent != null && NewsViewModel.LastSelectedNews.NewsContent.Count >= 0))
            {
                Frame.Navigate(typeof(NewsDetailPage), null, new SuppressNavigationTransitionInfo());
            }
        }
    }
}
