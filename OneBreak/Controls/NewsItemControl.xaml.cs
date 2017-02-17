using OneBreak.Models;
using System;

namespace OneBreak.Controls
{
    public sealed partial class NewsItemControl
    {

        public event Action<NewsModel> OnStarredToggleClick;

        public NewsItemControl()
        {
            this.InitializeComponent();
        }

        private void StarredToggle_OnClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var news = DataContext as NewsModel;
            OnStarredToggleClick?.Invoke(news);
        }
    }
}
