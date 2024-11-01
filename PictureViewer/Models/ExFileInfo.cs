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
    }
}