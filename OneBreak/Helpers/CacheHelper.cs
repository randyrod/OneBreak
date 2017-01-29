using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneBreak.Helpers
{
    public class CacheHelper
    {
        public static async Task<string> GetChacheContent(string fileName, string folderName)
        {
            var cacheFolder = await GetCacheFolder(fileName);

            if (cacheFolder == null) return null;

            var cacheFile = await cacheFolder.GetFileAsync(fileName);

            if (cacheFile == null) return null;

            try
            {
                var content = await FileIO.ReadTextAsync(cacheFile);
                return content;
            }
            catch (Exception)
            {
                //todo: log ex
                return null;
            }
        }

        public static async Task CreateChache(string fileName, string folderName, string content)
        {
            if (string.IsNullOrEmpty(fileName) || 
                string.IsNullOrEmpty(folderName) || 
                string.IsNullOrEmpty(content)) return;

            var cacheFolder = await GetCacheFolder(folderName);

            if (cacheFolder == null) return;

            var cacheFile = await GetCacheFile(fileName, cacheFolder);

            if (cacheFile == null) return;
            try
            {
                await FileIO.WriteTextAsync(cacheFile, content);
            }
            catch (Exception)
            {
                //todo: log ex
            }
        }

        private static async Task<StorageFolder> GetCacheFolder(string folderName)
        {
            if (string.IsNullOrEmpty(folderName)) return null;

            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;

                var folder = await localFolder.CreateFolderAsync(folderName, CreationCollisionOption.ReplaceExisting);

                return folder;
            }
            catch (Exception)
            {
                //todo: log ex
                return null;
            }
        }

        private static async Task<StorageFile> GetCacheFile(string fileName, StorageFolder cacheFolder)
        {
            if (string.IsNullOrEmpty(fileName) || cacheFolder == null) return null;

            try
            {
                var cacheFile = await cacheFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

                return cacheFile;
            }
            catch (Exception)
            {
                //todo: log exception
                return null;
            }
        }
    }
}
