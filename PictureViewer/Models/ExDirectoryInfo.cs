using System.IO;
using Prism.Mvvm;

namespace PictureViewer.Models
{
    public class ExDirectoryInfo : BindableBase
    {
        private DirectoryInfo directoryInfo;
        private string keyString;

        public ExDirectoryInfo(DirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo;
        }

        public DirectoryInfo DirectoryInfo
        {
            get => directoryInfo;
            set
            {
                RaisePropertyChanged(nameof(FullPath));
                RaisePropertyChanged(nameof(Name));
                SetProperty(ref directoryInfo, value);
            }
        }

        public string KeyString { get => keyString; set => SetProperty(ref keyString, value); }

        public string FullPath => DirectoryInfo.FullName;

        public string Name => DirectoryInfo.Name;
    }
}