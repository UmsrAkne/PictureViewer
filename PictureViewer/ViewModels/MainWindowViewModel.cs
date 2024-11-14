using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using PictureViewer.Models;
using PictureViewer.Models.Dbs;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PictureViewer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            SetDummyData();
        }

        public MainWindowViewModel(IDialogService dialogService, IContainerProvider containerProvider)
        {
            FileListViewModel = new FileListViewModel(string.Empty, dialogService, containerProvider.Resolve<ImageFileService>());
            SetDummyData();
        }

        public TextWrapper TextWrapper { get; } = new ();

        public FileListViewModel FileListViewModel { get; private set; }

        [Conditional("DEBUG")]
        private void SetDummyData()
        {
            FileListViewModel ??= new FileListViewModel();

            FileListViewModel.FilteredListProvider.AddRange(
                new List<ExFileInfo>()
                {
                    new (new FileInfo("test1.png")),
                    new (new FileInfo("test2.png")),
                    new (new FileInfo("test3.png")),
                    new (new FileInfo("test4.png")),
                    new (new FileInfo("test5.png")),
                    new (new FileInfo("test6.png")),
                    new (new FileInfo("test7.png")),
                    new (new FileInfo("test8.png")),
                    new (new FileInfo("test9.png")),
                    new (new FileInfo("test10.png")),
                    new (new DirectoryInfo("testDirectory1.png")),
                    new (new DirectoryInfo("testDirectory2.png")),
                    new (new DirectoryInfo("testDirectory3.png")),
                    new (new DirectoryInfo("testDirectory4.png")),
                    new (new DirectoryInfo("testDirectory5.png")),
                    new (new DirectoryInfo("testDirectory6.png")),
                    new (new DirectoryInfo("testDirectory7.png")),
                    new (new DirectoryInfo("testDirectory8.png")),
                });

            FileListViewModel.CurrentDirectories.AddRange(new List<ExFileInfo>()
            {
                new (new DirectoryInfo($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\test")),
                new (new DirectoryInfo($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\images")),
                new (new DirectoryInfo("testDir1")),
                new (new DirectoryInfo("testDir1")),
                new (new DirectoryInfo("testDir1")),
                new (new DirectoryInfo("testDir1")),
            });
        }
    }
}