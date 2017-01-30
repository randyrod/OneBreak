using OneBreak.Helpers;
using OneBreak.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OneBreak.ViewModels
{
    public class NewsViewModel : ViewModelBase
    {
        private bool _loading, _newsLoading;
        private const string GpuProviderKey = "ProviderGpu";

        private StarredNewsCacheHelper _starredCacheHelper = new StarredNewsCacheHelper();
        public bool Loading
        {
            get { return _loading; }
            set
            {
                if (_loading == value) return;
                _loading = value;
                OnPropertyChanged();
            }
        }

        public bool NewsLoading
        {
            get { return _newsLoading; }
            set
            {
                if (_newsLoading == value) return;
                _newsLoading = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NewsModel> _news;
        public ObservableCollection<NewsModel> News {
            get { return _news; }
            set
            {
                if (_news == value) return;

                _news = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NewsModel> _starredNews;
        public ObservableCollection<NewsModel> StarredNews
        {
            get { return _starredNews; }
            set
            {
                if (_starredNews == value) return;

                _starredNews = value;
                OnPropertyChanged();
            }
        }

        public NewsViewModel()
        {
            _news = new ObservableCollection<NewsModel>();
            _starredNews = new ObservableCollection<NewsModel>();
        }

        public async Task LoadNews()
        {
            Loading = true;
            var result = await NewsRequestHelper.GetGpuNews();

            //Note: this solution should be temporary, do not leave it here plz!!! :)
            var gpuProviderText = await App.GetStringFromResources(GpuProviderKey, false);
            
            if (result == null || result.Count <= 0) return;

            if (News.Count > 0 && News[0].Title == result[0].Title) return;

            foreach (var item in result)
            {
                item.Provider = gpuProviderText;
                News.Add(item);
            }

            Loading = false;
        }

        public async Task LoadStarredNews()
        {
            var list = await _starredCacheHelper.ReadStarredCache();

            if (list == null || list.Count <= 0) return;

            foreach (var item in list)
            {
                StarredNews.Add(item);
            }
        }

        public bool StarNews(NewsModel starred)
        {
            if (starred == null) return false;
            StarredNews.Add(starred);
            SaveStarredCache();
            return true;
        }

        public bool UnstarNews(NewsModel unstarred)
        {
            if (unstarred == null || !StarredNews.Contains(unstarred)) return false;
            var result = StarredNews.Remove(unstarred);

            if(result)
            {
                SaveStarredCache();
            }

            return result;
        }

        private void SaveStarredCache()
        {
            var currentList = StarredNews.ToList();

            _starredCacheHelper.SaveStarredCache(currentList);
        }


    }
}
