using OneBreak.Models;
using OneBreak.ViewModels;
using Windows.UI.Xaml.Navigation;

namespace OneBreak.Pages
{
    public sealed partial class NewsPage
    {
        public NewsViewModel NewsViewModel { get; set; }

        public NewsPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NewsViewModel = App.NewsViewModel;

            await App.NewsViewModel.LoadNews();

            await App.NewsViewModel.LoadStarredNews();

        }

        private void NewsItem_OnClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            var news = e.ClickedItem as NewsModel;

            if (news == null) return;

            var idx = App.NewsViewModel.News.IndexOf(news);

            if (idx == -1) return;
            NewsDetailFrame.Navigate(typeof(NewsDetailPage), idx);
        }

        private void NewsItemControl_OnStarredToggleClick(NewsModel news)
        {
            if (news == null) return;

            NewsViewModel.StarUnstarNews(news);
        }
    }
}
