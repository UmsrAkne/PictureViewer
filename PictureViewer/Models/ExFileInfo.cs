using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private char keyChar;

        public ExFileInfo(FileSystemInfo f)
        {
            if (f is FileInfo fi)
            {
                FileInfo = fi;
                Size = GetImageSize(fi.FullName);
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

        // [NotMapped]
        public char KeyChar { get => keyChar; set => SetProperty(ref keyChar, value); }

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

        private static Size GetImageSize(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath).ToLower();
            var isImageFile = new List<string> { ".jpg", ".jpeg", ".png", ".bmp", ".gif", }.Contains(fileExtension);

            if (!File.Exists(filePath) || !isImageFile)
            {
                return default;
            }

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // 読み込み後にファイルロックを解除
                bitmap.EndInit();
                return new Size((int)bitmap.Width, (int)bitmap.Height);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}