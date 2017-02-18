using OneBreak.Helpers;
using OneBreak.Models;
using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Navigation;

namespace OneBreak.Pages
{
    public sealed partial class NewsDetailPage : BasePage
    {
        public NewsModel CurrentNews { get; set; }

        private bool _onConnectedRegistered;

        private int _pendingNewsLoad;
        public NewsDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter == null)
            {
                //Show Error Message
                return;
            }

            int idx = -1;
            try
            {
                idx = (int)e.Parameter;
            }
            catch (Exception)
            {
                //Show Error Message
                return;
            }

            if(ConnectionHelper.IsConnected)
            {
                LoadNewsBody(idx);
            }
            else
            {
                _pendingNewsLoad = idx;
                RegisterOnConnected();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            UnregisterOnConnected();
        }

        private async Task LoadNewsBody(int index)
        {
            if(index <= -1 || index >= App.NewsViewModel.News.Count)
            {
                //Show Error Message
                return;
            }

            var news = App.NewsViewModel.News[index];

            CurrentNews = news;
            
            await CurrentNews.LoadNewsBody();

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

            LoadNewsBody(_pendingNewsLoad);

            UnregisterOnConnected();
        }
        #endregion
    }
}
