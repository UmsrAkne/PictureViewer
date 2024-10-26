using System.IO;
using Prism.Mvvm;

namespace PictureViewer.Models
{
    public class ExFileInfo : BindableBase
    {
        private bool isViewed;
        private Rating rating = Rating.NoRating;
        private bool isSelected;

        public ExFileInfo(FileSystemInfo f)
        {
            if (f is FileInfo fi)
            {
                FileInfo = fi;
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
    }
}