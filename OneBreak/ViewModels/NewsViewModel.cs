﻿using OneBreak.Helpers;
using OneBreak.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OneBreak.ViewModels
{
    public class NewsViewModel : ViewModelBase
    {
        private bool _loading, _newsLoading, _starredLoading;
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

        public bool StarredLoading
        {
            get { return _starredLoading; }
            set
            {
                if (_starredLoading == value) return;
                _starredLoading = value;
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

        private NewsModel _lastSelectedNews;
        public NewsModel LastSelectedNews
        {
            get { return _lastSelectedNews; }
            set
            {
                if (_lastSelectedNews == value) return;
                _lastSelectedNews = value;
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

            if (result == null || result.Count <= 0)
            {
                Loading = false;
                return;
            }

            if (News.Count > 0 && News[0].Title == result[0].Title)
            {
                Loading = false;
                return;
            }

            foreach (var item in result)
            {
                item.Provider = gpuProviderText;
                News.Add(item);
            }

            Loading = false;
        }

        public async Task LoadStarredNews()
        {
            StarredLoading = true;
            var list = await _starredCacheHelper.ReadStarredCache();

            if (list == null || list.Count <= 0)
            {
                StarredLoading = false;
                return;
            }

            foreach (var item in list)
            {
                var toAdd = item;
                foreach (var current in News)
                {
                    if(item.Title == current.Title)
                    {
                        toAdd = current;
                        break;
                    }
                }
                toAdd.Starred = true;
                StarredNews.Add(toAdd);
            }
            StarredLoading = false;
        }

        public void StarUnstarNews(NewsModel news)
        {

            if(news.Starred)
            {
                UnstarNews(news);
            }
            else
            {
                StarNews(news);
            }
        }

        private async Task<bool> StarNews(NewsModel starred)
        {
            if (starred == null || StarredNews.Contains(starred)) return false;

            if(string.IsNullOrEmpty(starred.NewsBody))
            {
                await starred.LoadNewsBody();
            }

            starred.Starred = true;
            StarredNews.Add(starred);
            SaveStarredCache();
            return true;
        }

        private bool UnstarNews(NewsModel unstarred)
        {
            if (unstarred == null || !StarredNews.Contains(unstarred)) return false;
            var result = StarredNews.Remove(unstarred);

            unstarred.Starred = false;

            if(result)
            {
                SaveStarredCache();
            }

            return result;
        }

        private async void SaveStarredCache()
        {
            var currentList = StarredNews.ToList();

            await _starredCacheHelper.SaveStarredCache(currentList);
        }
    }
}
