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
        private ExFileInfo selectedFileInfo;
        private string currentImageFilePath;
        private ObservableCollection<ExFileInfo> currentDirectories = new ();
        private ExFileInfo currentDirectory;

        public FileListViewModel(string defaultDirectoryPath = null, IDialogService dialogService = null)
        {
            CurrentDirectories.Add(!string.IsNullOrWhiteSpace(defaultDirectoryPath)
                ? new ExFileInfo(new DirectoryInfo(defaultDirectoryPath))
                : new ExFileInfo(
                    new DirectoryInfo($"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}")));

            CurrentDirectory = CurrentDirectories.First();

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

        public FilteredListProvider FilteredListProvider { get; set; } = new ();

        public string CurrentDirectoryPath
        {
            get => currentDirectoryPath;
            set
            {
                try
                {
                    LoadFileAndDirectories(value);
                    CurrentDirectory.SetFileSystemInfo(new DirectoryInfo(value));
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

        public DelegateCommand<ExFileInfo> OpenDirectoryCommand => new ((param) =>
        {
            if (param is not { IsDirectory: true, })
            {
                return;
            }

            SelectedFileInfo.SetFileSystemInfo(param.FileSystemInfo);
            CurrentDirectoryPath = SelectedFileInfo.FileSystemInfo.FullName;
        });

        public DelegateCommand ShowFileCopyDialogCommand => new DelegateCommand(() =>
        {
            var p = new DialogParameters
            {
                { nameof(FileCopyDialogViewModel.CopyableDirectories), CurrentDirectories },
                {
                    nameof(FileCopyDialogViewModel.CurrentFiles),
                    new ObservableCollection<ExFileInfo>() { SelectedFileInfo, }
                },
            };
            dialogService.ShowDialog(nameof(FileCopyDialog), p, _ => { });
        });

        public DelegateCommand ShowTextInputDialogCommand => new DelegateCommand(() =>
        {
            var p = new DialogParameters { { nameof(TextInputDialogViewModel.Message), "ディレクトリを作成します。名前を入力してください。" }, };
            dialogService.ShowDialog(nameof(TextInputDialog), p, result =>
            {
                if (result.Result != ButtonResult.OK)
                {
                    return;
                }

                try
                {
                    result.Parameters.TryGetValue<string>(nameof(TextInputDialogViewModel.Text), out var t);
                    if (string.IsNullOrWhiteSpace(t))
                    {
                        return;
                    }

                    var info = new DirectoryInfo($"{CurrentDirectoryPath}\\{t}");
                    Directory.CreateDirectory(info.FullName);
                    FilteredListProvider.Add(new ExFileInfo(info));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
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