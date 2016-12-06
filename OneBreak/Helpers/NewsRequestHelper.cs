using OneBreak.Models;
using OneBreakUtils;
using OneBreakUtils.HttpStrings;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OneBreak.Helpers
{
    public class NewsRequestHelper
    {
        public static async Task<List<NewsModel>> GetGpuNews()
        {
            var parameters = new HttpRequestParameters
            {
                Url = StringResources.GpuRssUrl,
                Method = HttpMethod.Get
            };

            var result = await HttpRequestHelper.GetStringFromUrl(parameters);

            if (string.IsNullOrEmpty(result)) return null;

            var rssContent = XmlSerializationHelper.DeserializeStringToXmlObject<GpuRss>(result);

            if (rssContent == null) return null;

            var news = new List<NewsModel>();
            var itemsCount = rssContent.Channel.Item.Count;
            var rgx = new Regex("\\&\\w+\\;$");
            foreach (var gpNews in rssContent.Channel.Item)
            {
                if (string.IsNullOrEmpty(gpNews.Title) ||
                    string.IsNullOrEmpty(gpNews.Description) ||
                    string.IsNullOrEmpty(gpNews.Link2) ||
                    gpNews.Enclosure == null)
                {
                    continue;
                }

                gpNews.Description = rgx.Replace(gpNews.Description, "...");

                var newNews = new NewsModel
                {
                    Title = gpNews.Title,
                    Description = gpNews.Description,
                    CoverArtImageUrl = gpNews.Enclosure.Url,
                    OriginalUrl = gpNews.Link2
                };

                news.Add(newNews);
            }
            return news;
        }

        public static async Task<string> GetGpuNewsBody(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            var parameters = new HttpRequestParameters
            {
                Url = url,
                Method = HttpMethod.Get
            };

            var result = await HttpRequestHelper.GetStringFromUrl(parameters);

            return string.IsNullOrEmpty(result) ? null : result;

        }
    }
}
