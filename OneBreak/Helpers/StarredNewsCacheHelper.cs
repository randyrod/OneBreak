using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneBreak.Models;
using Newtonsoft.Json;

namespace OneBreak.Helpers
{
    public class StarredNewsCacheHelper
    {
        private const string _starredCacheFolder = "StarredNews";
        private const string _starredNewsFile = "StarredCache";

        public async Task SaveStarredCache(List<NewsModel> newsList)
        {
            if (newsList == null) return;

            try
            {
                var json = JsonConvert.SerializeObject(newsList);

                if (string.IsNullOrEmpty(json)) return;

                await CacheHelper.CreateChache(_starredNewsFile, _starredCacheFolder, json);
            }
            catch (Exception)
            {
                //Log ex
            }
        }

        public async Task<List<NewsModel>> ReadStarredCache()
        {
            try
            {
                var json = await CacheHelper.GetChacheContent(_starredNewsFile, _starredCacheFolder);

                if (string.IsNullOrEmpty(json)) return null;

                var news = JsonConvert.DeserializeObject<List<NewsModel>>(json);

                return news;
            }
            catch (Exception)
            {
                //todo: Log ex
                return null;
            }
        }
    }
}
