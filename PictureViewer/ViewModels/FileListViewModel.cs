using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using PictureViewer.Models;
using PictureViewer.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PictureViewer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FileListViewModel : BindableBase, IDisposable
    {
        private readonly IDialogService dialogService;
        private readonly FileSystemWatcher fileSystemWatcher = new ();
        private string currentDirectoryPath;
        private ObservableCollection<ExFileInfo> files = new ();
        private ExFileInfo selectedFileInfo;
        private string currentImageFilePath;
        private ObservableCollection<ExFileInfo> currentDirectories = new ();
        private ExFileInfo currentDirectory;

        public FileListViewModel(IDialogService dialogService = null)
        {
            fileSystemWatcher.Filter = "*.png";
            fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            fileSystemWatcher.Created += (_, e) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    FilteredListProvider.Add(new ExFileInfo(new FileInfo(e.FullPath)));
                });
            };

            this.dialogService = dialogService;
        }

        public ObservableCollection<ExFileInfo> Files
        {
            get => files;
            set => SetProperty(ref files, value);
        }

        public FilteredListProvider FilteredListProvider { get; set; } = new ();

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
                    fileSystemWatcher.EnableRaisingEvents = false;
                    return;
                }

                SetProperty(ref currentDirectoryPath, value);
                fileSystemWatcher.Path = value;
                fileSystemWatcher.EnableRaisingEvents = true;
            }
        }

        public ObservableCollection<ExFileInfo> CurrentDirectories
        {
            get => currentDirectories;
            set => SetProperty(ref currentDirectories, value);
        }

        public ExFileInfo CurrentDirectory
        {
            get => currentDirectory;
            set
            {
                if (value != null)
                {
                    CurrentDirectoryPath = value.FileSystemInfo.FullName;
                }

                SetProperty(ref currentDirectory, value);
            }
        }

        public ExFileInfo SelectedFileInfo
        {
            get => selectedFileInfo;
            set
            {
                SetProperty(ref selectedFileInfo, value);
                if (value == null)
                {
                    return;
                }

                value.IsViewed = true;
                CurrentImageFilePath = value.FileSystemInfo.FullName;
            }
        }

        public string CurrentImageFilePath
        {
            get => currentImageFilePath;
            private set => SetProperty(ref currentImageFilePath, value);
        }

        public DelegateCommand AddCurrentDirectoryCommand => new DelegateCommand(() =>
        {
            CurrentDirectories.Add(new ExFileInfo(new DirectoryInfo(CurrentDirectoryPath)));
        });

        public DelegateCommand<ExFileInfo> CloseCurrentDirectoryCommand => new DelegateCommand<ExFileInfo>((param) =>
        {
            if (param == null || !CurrentDirectories.Contains(param))
            {
                return;
            }

            var index = CurrentDirectories.IndexOf(param);
            CurrentDirectories.RemoveAt(index);

            if (CurrentDirectories.Count > 0)
            {
                CurrentDirectory = CurrentDirectories[Math.Min(CurrentDirectories.Count - 1, index)];
            }
        });

        public DelegateCommand ShowTextInputDialogCommand => new DelegateCommand(() =>
        {
            dialogService.ShowDialog(nameof(TextInputDialog), new DialogParameters(), result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                }
            });
        });

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            fileSystemWatcher.Dispose();
        }

        private void LoadFileAndDirectories(string directoryPath)
        {
            var f = Directory.GetFiles(directoryPath)
                .Select(p => new ExFileInfo(new FileInfo(p)));

            var d = Directory.GetDirectories(directoryPath)
                .Select(p => new ExFileInfo(new DirectoryInfo(p)));

            FilteredListProvider.Replace(f.Concat(d).ToList());
        }
    }
}