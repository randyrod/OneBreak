using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneBreak.Controls
{
    public sealed partial class NewsDetailControl : UserControl
    {
        public static readonly DependencyProperty NewsContentProperty = DependencyProperty.Register(nameof(NewsContent), 
            typeof(List<KeyValuePair<string, string>>), typeof(NewsDetailControl), new PropertyMetadata(null, new PropertyChangedCallback(NewsContent_Changed)));

        private static void NewsContent_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NewsDetailControl;
            control?.GenerateContent();
        }

        public List<KeyValuePair<string,string>> NewsContent
        {
            get
            {
                return (List<KeyValuePair<string, string>>)GetValue(NewsContentProperty);
            }
            set
            {
                SetValue(NewsContentProperty, value);
            }
        }


        private void GenerateContent()
        {
            if (NewsContent == null || NewsContent.Count <= 0) return;
            NewsContentStackPanel.Children.Clear();
            foreach (var item in NewsContent)
            {
                switch (item.Key)
                {
                    case "p":
                        var tb = new TextBlock
                        {
                            Text = item.Value,
                            Style = NewsTextBlockStyle,
                            TextWrapping = TextWrapping.WrapWholeWords
                        };

                        NewsContentStackPanel.Children.Add(tb);
                        break;
                    case "img":
                        var imageControl = new NewsImageControl
                        {
                            ImageSrc = item.Value,
                            Style = ImageControStyle
                        };
                        
                        NewsContentStackPanel.Children.Add(imageControl);
                        break;
                }
            }
        }

        public NewsDetailControl()
        {
            this.InitializeComponent();
        }
    }
}
