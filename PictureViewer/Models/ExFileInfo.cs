using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace PictureViewer.Models
{
    public class ExFileInfo : BindableBase
    {
        private bool isViewed;
        private Rating rating = Rating.NoRating;
        private bool isSelected;
        private Size size;

        public ExFileInfo(FileSystemInfo f)
        {
            if (f is FileInfo fi)
            {
                FileInfo = fi;
                Size = GetImageSizeFromStream(fi.FullName);
            }
            else
            {
                DirectoryInfo = (DirectoryInfo)f;
            }

            FileSystemInfo = f;
        }

        public bool IsDirectory => DirectoryInfo != null;

        public FileSystemInfo FileSystemInfo { get; private set; }

        public bool IsViewed { get => isViewed; set => SetProperty(ref isViewed, value); }

        public Rating Rating { get => rating; set => SetProperty(ref rating, value); }

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }

        public int Width { get; set; }

        public int Height { get; set; }

        // [NotMapped]
        public Size Size
        {
            get => size;
            set
            {
                Width = value.Width;
                Height = value.Height;
                SetProperty(ref size, value);
            }
        }

        private FileInfo FileInfo { get; set; }

        private DirectoryInfo DirectoryInfo { get; set; }

        /// <summary>
        /// このインスタンスが保持している FileInfo または DirectoryInfo を置き換えます。
        /// </summary>
        /// <param name="f">セットする FileInfo or DirectoryInfo</param>
        public void SetFileSystemInfo(FileSystemInfo f)
        {
            FileInfo = null;
            DirectoryInfo = null;
            FileSystemInfo = null;

            IsViewed = false;
            Rating = Rating.NoRating;

            if (f is FileInfo fi)
            {
                FileInfo = fi;
            }
            else
            {
                DirectoryInfo = (DirectoryInfo)f;
            }

            FileSystemInfo = f;

            RaisePropertyChanged(nameof(IsDirectory));
            RaisePropertyChanged(nameof(FileSystemInfo));
        }

        private static Size GetImageSizeFromStream(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath).ToLower();
            var isImageFile = new List<string> { ".jpg", ".jpeg", ".png", ".bmp", ".gif", }.Contains(fileExtension);

            if (!File.Exists(filePath) || !isImageFile)
            {
                return default;
            }

            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var decoder = BitmapDecoder.Create(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None);
                var width = decoder.Frames[0].PixelWidth;
                var height = decoder.Frames[0].PixelHeight;
                return new Size(width, height);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        /// <summary>
        /// 指定した画像ファイルを読み込み、指定された縦サイズのサムネイルを生成します。
        /// </summary>
        /// <param name="filePath">画像ファイルのパス</param>
        /// <param name="desiredHeight">生成するサムネイルの縦サイズ（ピクセル単位）</param>
        /// <returns>サムネイル画像の BitmapSource オブジェクト</returns>
        private static BitmapSource GenerateThumbnail(string filePath, int desiredHeight)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定された画像ファイルが見つかりません。", filePath);
            }

            var originalImage = new BitmapImage();
            originalImage.BeginInit();
            originalImage.UriSource = new Uri(filePath);
            originalImage.CacheOption = BitmapCacheOption.OnLoad; // ファイルを完全に読み込む
            originalImage.EndInit();

            // 元の画像の縦横比を保ちながら、縦サイズに基づいて横サイズを計算
            var scale = (double)desiredHeight / originalImage.PixelHeight;

            // サムネイル画像の生成
            var thumbnail = new TransformedBitmap(originalImage, new ScaleTransform(scale, scale));

            // SaveBitmapSourceToFile(thumbnail, outputPath);
            return thumbnail;
        }

        /// <summary>
        /// BitmapSource を指定のパスに PNG 形式で保存します。
        /// </summary>
        /// <param name="bitmapSource">保存する BitmapSource</param>
        /// <param name="filePath">保存先のファイルパス</param>
        private static void SaveBitmapSourceToFile(BitmapSource bitmapSource, string filePath)
        {
            var encoder = new PngBitmapEncoder(); // PNG エンコーダを使用
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            // 指定のパスにファイルを保存
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            encoder.Save(fs);
        }
    }
}