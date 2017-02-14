using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OneBreak.Controls
{
    public sealed partial class NewsImageControl : UserControl
    {
        public NewsImageControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ImageSourceProperty = 
            DependencyProperty.Register(nameof(ImageSrc), 
                typeof(string), typeof(NewsImageControl), 
                new PropertyMetadata(null, OnImageSourceChanged));

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NewsImageControl;
            control?.SetImageSource(e.NewValue);
        }

        public string ImageSrc
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        private void SetImageSource(object src)
        {
            var _image = new BitmapImage();
            var source = src as string;
            if (string.IsNullOrEmpty(source)) return;
            var uri = new Uri(source);
            _image.UriSource = uri;
            NewsImage.Source = _image;
        }

        public static readonly DependencyProperty PlaceholderImageSourceProperty = 
            DependencyProperty.Register(nameof(PlaceholderImageSource), 
                typeof(ImageSource), typeof(NewsImageControl), 
                new PropertyMetadata(null));

        public ImageSource PlaceholderImageSource
        {
            get { return (ImageSource)GetValue(PlaceholderImageSourceProperty); }
            set { SetValue(PlaceholderImageSourceProperty, value); }
        }

        private void NewsImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            PlaceholderFade?.Begin();
        }
    }
}
