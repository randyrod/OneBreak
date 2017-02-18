using OneBreak.Helpers;
using OneBreak.Models;
using OneBreak.ViewModels;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneBreak.Pages
{
    public sealed partial class NewsPage
    {
        public NewsViewModel NewsViewModel { get; set; }

        private bool _onConnectedRegistered;

        public NewsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NewsViewModel = App.NewsViewModel;

            if(ConnectionHelper.IsConnected)
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

        private void NewsItem_OnClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            var news = e.ClickedItem as NewsModel;

            if (news == null) return;

            var idx = App.NewsViewModel.News.IndexOf(news);

            if (idx == -1) return;

            if(VisualStateTriggers.CurrentState == MobileState)
            {
                Frame.Navigate(typeof(NewsDetailPage), idx);
                return;
            }
            NewsDetailFrame.Navigate(typeof(NewsDetailPage), idx);
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

            LoadNews();

            UnregisterOnConnectedEvent();

            _onConnectedRegistered = false;
        }

        private void UnregisterOnConnectedEvent()
        {
            if (!_onConnectedRegistered) return;

            NetworkInformation.NetworkStatusChanged -= NetworkInformation_NetworkStatusChanged;

            _onConnectedRegistered = false;
        }
        #endregion
    }
}
