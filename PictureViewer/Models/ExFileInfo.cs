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

        public FileSystemInfo FileSystemInfo { get; }

        public bool IsViewed { get => isViewed; set => SetProperty(ref isViewed, value); }

        public Rating Rating { get => rating; set => SetProperty(ref rating, value); }

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }

        private FileInfo FileInfo { get; set; }

        private DirectoryInfo DirectoryInfo { get; set; }
    }
}