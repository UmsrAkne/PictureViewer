using System;
using System.Globalization;
using System.Windows.Data;
using PictureViewer.Models;

namespace PictureViewer.Views.Converters
{
    public class RatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }

            var r = (Rating)value;
            return r == Rating.NoRating ? "-" : ((Rating)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}