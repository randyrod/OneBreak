﻿using OneBreak.Helpers;
using OneBreak.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace OneBreak.Models
{
    public class NewsModel : ViewModelBase
    {
        private const string NewsBodyParagraphTag = "p", NewsBodyImageTag = "img", NewsBodyHrefTag = "href", NewsBodySrcTag = "src";
        public string Title { get; set; }

        public string Description { get; set; }

        private List<KeyValuePair<string, string>> _newsContent;
        public List<KeyValuePair<string,string>> NewsContent
        {
            get { return _newsContent; }
            set
            {
                if (_newsContent == value) return;
                _newsContent = value;
                OnPropertyChanged();
            }
        }

        public string CoverArtImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public string Provider { get; set; }

        private bool _loadSuccess;
        [JsonIgnore]
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
        [JsonIgnore]
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

        private bool _loadingFailed;
        [JsonIgnore]
        public bool LoadingFailed
        {
            get { return _loadingFailed; }
            set
            {
                if (_loadingFailed == value) return;
                _loadingFailed = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> LoadNewsBody()
        {
            Loading = true;
            if (NewsContent != null && NewsContent.Count > 0)
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

            var newsContent = ParseNewsBody(newsBody);
            
            if(newsContent == null)
            {
                Loading = false;
                LoadingFailed = true;
                return false;
            }

            NewsContent = newsContent;
            Loading = false;
            return true;
        }

        private List<KeyValuePair<string, string>> ParseNewsBody(string rawHtml)
        {
            try
            {
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();

                htmlDoc.LoadHtml(rawHtml);

                var textualDiv = htmlDoc.DocumentNode.Descendants("div")
                    .Where(x => x.Attributes.Contains("class") && x.Attributes[0].Value == "textual");

                if (textualDiv == null || !textualDiv.Any()) return null; //Log Error

                var textualDivHtmlContent = textualDiv.FirstOrDefault().InnerHtml;

                if (string.IsNullOrEmpty(textualDivHtmlContent)) return null; //Log Error

                textualDivHtmlContent = RegexSanitize(textualDivHtmlContent);

                htmlDoc = null;

                htmlDoc = new HtmlAgilityPack.HtmlDocument();

                htmlDoc.LoadHtml(textualDivHtmlContent);

                var descendantNodes = htmlDoc.DocumentNode.Descendants();

                var newsContent = new List<KeyValuePair<string,string>>();
                
                foreach (var node in descendantNodes)
                {
                    if (node.Name == NewsBodyParagraphTag)
                    {
                        newsContent.Add(new KeyValuePair<string, string>(NewsBodyParagraphTag, node.InnerText));
                    }
                    else if (node.Name == NewsBodyImageTag)
                    {
                        var x = node.Attributes;
                        if (node.Attributes.Contains(NewsBodyHrefTag))
                        {
                            newsContent.Add(new KeyValuePair<string, string>(NewsBodyImageTag, node.Attributes[NewsBodyHrefTag].Value));
                        }
                        else if (node.Attributes.Contains(NewsBodySrcTag))
                        {
                            newsContent.Add(new KeyValuePair<string, string>(NewsBodyImageTag, node.Attributes[NewsBodySrcTag].Value));
                        }
                    }
                }

                return newsContent;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
