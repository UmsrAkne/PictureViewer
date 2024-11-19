using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PictureViewer.Models;
using PictureViewer.Models.Dbs;
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
        private ExFileInfo selectedFileInfo;
        private string currentImageFilePath;
        private ImageFileService imageFileService;
        private DirectoryListViewModel directoryListViewModel = new ();

        public FileListViewModel(string defaultDirectoryPath = null, IDialogService dialogService = null, ImageFileService imageFileService = null)
        {
            DirectoryListViewModel.Directories.Add(!string.IsNullOrWhiteSpace(defaultDirectoryPath)
                ? new ExFileInfo(new DirectoryInfo(defaultDirectoryPath))
                : new ExFileInfo(
                    new DirectoryInfo($"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}")));

            DirectoryListViewModel.SelectedItem = DirectoryListViewModel.Directories.First();

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
            this.imageFileService = imageFileService;
        }

        public FilteredListProvider FilteredListProvider { get; set; } = new ();

        public DirectoryListViewModel DirectoryListViewModel
        {
            get => directoryListViewModel;
            set => SetProperty(ref directoryListViewModel, value);
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

        public DelegateCommand<ExFileInfo> OpenDirectoryCommand => new ((param) =>
        {
            if (param is not { IsDirectory: true, })
            {
                return;
            }

            SelectedFileInfo.SetFileSystemInfo(param.FileSystemInfo);
            DirectoryListViewModel.CurrentPath = SelectedFileInfo.FileSystemInfo.FullName;
        });

        public DelegateCommand ShowFileCopyDialogCommand => new DelegateCommand(() =>
        {
            var p = new DialogParameters
            {
                {
                    nameof(FileCopyDialogViewModel.CopyableDirectories),
                    new ObservableCollection<ExDirectoryInfo>(
                    DirectoryListViewModel.Directories
                        .Where(d => d.IsDirectory && d.FileSystemInfo.FullName != DirectoryListViewModel.SelectedItem.FileSystemInfo.FullName)
                        .Select(d => new ExDirectoryInfo((DirectoryInfo)d.FileSystemInfo)))
                },
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

                    var info = new DirectoryInfo($"{DirectoryListViewModel.CurrentPath}\\{t}");
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

        private async Task LoadFileAndDirectories(string directoryPath)
        {
            var f = Directory.GetFiles(directoryPath)
                .Select(p => imageFileService.GetOrCreateExFileAsync(p));

            var fileResults = await Task.WhenAll(f);

            var d = Directory.GetDirectories(directoryPath)
                .Select(p => new ExFileInfo(new DirectoryInfo(p)));

            FilteredListProvider.Replace(fileResults.Concat(d).ToList());
        }
    }
}