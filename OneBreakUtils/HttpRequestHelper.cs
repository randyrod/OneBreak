using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneBreakUtils
{
    public class HttpRequestHelper
    {
        private const int RetryCount = 3;

        public static async Task<HttpResponseMessage> SendRequestWithRetry(HttpRequestParameters parameters) => 
            await SendRequest(parameters.Url, parameters.Content, parameters.Method);

        public static async Task<string> GetStringFromUrl(HttpRequestParameters parameters)
        {
            if (parameters == null) return null;

            try
            {
                var response = await SendRequest(parameters.Url, parameters.Content, parameters.Method);

                if (response == null || !response.IsSuccessStatusCode) return null;

                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static async Task<HttpResponseMessage> SendRequest(string url, string content, HttpMethod method)
        {
            if (string.IsNullOrEmpty(url)) return null;
            var uri = new Uri(url);
            var client = new HttpClient();
            HttpResponseMessage response = null;

            for (int i = 0; i < RetryCount; i++)
            {
                try
                {
                    if(method == HttpMethod.Get)
                    {
                        response = await client.GetAsync(url);
                    }
                    else if(method == HttpMethod.Post)
                    {
                        if (string.IsNullOrEmpty(content)) return null;

                        response = await client.PostAsync(url, new StringContent(content));
                    }

                    if (response == null) continue;

                    if (!response.IsSuccessStatusCode) continue;

                    return response;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }
    }

    public class HttpRequestParameters
    {
        public string Url { get; set; }

        public string Content { get; set; }

        public HttpMethod Method { get; set; }
    }
}
