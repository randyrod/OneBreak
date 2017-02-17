using OneBreak.Helpers;
using OneBreak.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OneBreak.Models
{
    public class NewsModel : ViewModelBase
    {
        public string Title { get; set; }

        public string Description { get; set; }

        private string _newsBody;
        public string NewsBody
        {
            get { return _newsBody; }
            set
            {
                if (_newsBody == value) return;
                _newsBody = value;
                OnPropertyChanged();
            }
        }

        public string CoverArtImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public string Provider { get; set; }

        private bool _loadSuccess;

        public bool LoadSuccess
        {
            get { return _loadSuccess; }
            set
            {
                if (_loadSuccess == value) return;
                _loadSuccess = value;
                OnPropertyChanged();
            }
        }

        private bool _loading;
        public bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                if (_loading == value) return;
                _loading = value;
                OnPropertyChanged();
            }
        }

        private bool _starred;
        public bool Starred
        {
            get { return _starred; }
            set
            {
                if (_starred == value) return;
                _starred = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> LoadNewsBody()
        {
            Loading = true;
            if (!string.IsNullOrEmpty(NewsBody))
            {
                Loading = false;
                return true;
            }

            var newsBody = await NewsRequestHelper.GetGpuNewsBody(OriginalUrl);

            if (newsBody == null)
            {
                Loading = false;
                return false;
            }

            newsBody = SanitizeNewsBody(newsBody);

            if (newsBody == null)
            {
                Loading = false;
                return false;
            }

            NewsBody = newsBody;

            Loading = false;
            return true;
        }

        private string SanitizeNewsBody(string rawHtml)
        {
            try
            {

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();

                htmlDoc.LoadHtml(rawHtml);

                var textualDiv = htmlDoc.DocumentNode.Descendants("div")
                    .Where(x => x.Attributes.Contains("class") && x.Attributes[0].Value == "textual");

                if (textualDiv == null) return null; //Log Error

                var textualDivHtmlContent = textualDiv.FirstOrDefault().InnerHtml;

                if (string.IsNullOrEmpty(textualDivHtmlContent)) return null; //Log Error

                textualDivHtmlContent = RegexSanitize(textualDivHtmlContent);

                htmlDoc = null;

                htmlDoc = new HtmlAgilityPack.HtmlDocument();

                htmlDoc.LoadHtml(textualDivHtmlContent);

                var descendantNodes = htmlDoc.DocumentNode.Descendants();

                var result = "";

                foreach (var node in descendantNodes)
                {
                    if(node.Name == "p")
                    {
                        result += node.InnerText;
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string RegexSanitize(string rawBody)
        {
            const string BreakLineHtmlPattern = "<br>|<\\/br>";
            const string StrongPattern = "<strong>|<\\/strong>";
            const string SmallPattern = "<small>|<\\/small>";
            const string ExtraSpacePattern = "\\s\\s+";
            const string DashPattern = "&ndash;";
            const string SpecialCharacterPattern = "&\\w+;";

            try
            {
                var regex = new Regex(ExtraSpacePattern);

                rawBody = regex.Replace(rawBody, " ");

                regex = new Regex(StrongPattern);

                rawBody = regex.Replace(rawBody, "");

                regex = new Regex(SmallPattern);

                rawBody = regex.Replace(rawBody, "");

                regex = new Regex(BreakLineHtmlPattern);

                rawBody = regex.Replace(rawBody, "\n");

                regex = new Regex(DashPattern);

                rawBody = regex.Replace(rawBody, "-");

                regex = new Regex(SpecialCharacterPattern);

                rawBody = regex.Replace(rawBody, "");

                return rawBody;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
