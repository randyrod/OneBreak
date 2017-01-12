using OneBreak.Helpers;
using OneBreak.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OneBreak.ViewModels
{
    public class NewsViewModel : ViewModelBase
    {
        private bool _loading, _newsLoading;
        private const string GpuProviderKey = "ProviderGpu";
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

        public NewsViewModel()
        {
            _news = new ObservableCollection<NewsModel>();
        }

        public async Task LoadNews()
        {
            Loading = true;
            var result = await NewsRequestHelper.GetGpuNews();
            //Note: this solution should be temporary, do not leave it here plz!!! :)

            var gpuProviderText = await App.GetStringFromResources(GpuProviderKey, false);
            

            if (result == null) return;

            foreach (var item in result)
            {
                item.Provider = gpuProviderText;
                News.Add(item);
            }

            Loading = false;
        }
    }
}
