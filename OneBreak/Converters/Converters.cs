using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OneBreak.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        private bool _inverse;

        public bool Inverse
        {
            get { return _inverse; }
            set { _inverse = value; }
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is bool)) return false;

            var val = (bool)value;

            if(!Inverse)
            {
                var valRet = val ? Visibility.Visible : Visibility.Collapsed;
                return val ? Visibility.Visible : Visibility.Collapsed;
            }

            return val ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
