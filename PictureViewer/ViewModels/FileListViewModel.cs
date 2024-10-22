using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using PictureViewer.Models;
using Prism.Mvvm;

namespace PictureViewer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FileListViewModel : BindableBase
    {
        private string currentDirectoryPath;
        private ObservableCollection<ExFileInfo> files = new ();
        private ExFileInfo selectedFileInfo;
        private string currentImageFilePath;

        public ObservableCollection<ExFileInfo> Files
        {
            get => files;
            set => SetProperty(ref files, value);
        }

        public string CurrentDirectoryPath
        {
            get => currentDirectoryPath;
            set
            {
                try
                {
                    LoadFileAndDirectories(value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ディレクトリの読み取りに失敗しました。");
                    Console.WriteLine(e);
                    return;
                }

                SetProperty(ref currentDirectoryPath, value);
            }
        }

        public ExFileInfo SelectedFileInfo
        {
            get => selectedFileInfo;
            set
            {
                SetProperty(ref selectedFileInfo, value);
                CurrentImageFilePath = value.FileSystemInfo.FullName;
            }
        }

        public string CurrentImageFilePath
        {
            get => currentImageFilePath;
            private set => SetProperty(ref currentImageFilePath, value);
        }

        private void LoadFileAndDirectories(string directoryPath)
        {
            var f = Directory.GetFiles(directoryPath)
                .Select(p => new ExFileInfo(new FileInfo(p)));

            var d = Directory.GetDirectories(directoryPath)
                .Select(p => new ExFileInfo(new DirectoryInfo(p)));

            Files = new ObservableCollection<ExFileInfo>(f.Concat(d).ToList());
        }
    }
}