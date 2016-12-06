using OneBreak.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneBreak.Pages
{
    public sealed partial class NewsDetailPage : Page
    {
        public NewsModel CurrentNews { get; set; }
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

            LoadNewsBody(idx);
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
    }
}
