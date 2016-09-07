using OneBreak.Helpers;
using OneBreak.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OneBreak.ViewModels
{
    public class NewsPageViewModel : ViewModelBase
    {
        private bool _loading;
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

        public NewsPageViewModel()
        {
            _news = new ObservableCollection<NewsModel>();
        }

        public async Task LoadNews()
        {
            Loading = true;
            var result = await NewsRequestHelper.GetGpuNews();

            if (result == null) return;

            foreach (var item in result)
            {
                News.Add(item);
            }

            Loading = false;
        }
    }
}
