using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PictureViewer.Views.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 入力が文字列か確認
            if (value is not string path)
            {
                return null;
            }

            if (!File.Exists(path))
            {
                return null;
            }

            var extension = Path.GetExtension(path).ToLower();
            var allowedExtensions = new[] { ".jpg", ".png", ".bmp", ".gif", ".webp", };

            if (!allowedExtensions.Contains(extension))
            {
                return null;
            }

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // 画像をメモリにロードしてファイルをロックしない
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}