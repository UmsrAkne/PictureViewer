using System;
using System.Collections.ObjectModel;
using System.IO;
using PictureViewer.Models;
using PictureViewer.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PictureViewer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FileCopyDialogViewModel : BindableBase, IDialogAware
    {
        private ObservableCollection<ExFileInfo> copyableDirectories = new ();
        private ObservableCollection<ExFileInfo> currentFiles = new ();

        public event Action<IDialogResult> RequestClose;

        public string Title => nameof(FileCopyDialog);

        public ObservableCollection<ExFileInfo> CurrentFiles
        {
            get => currentFiles;
            set => SetProperty(ref currentFiles, value);
        }

        public ObservableCollection<ExFileInfo> CopyableDirectories
        {
            get => copyableDirectories;
            set => SetProperty(ref copyableDirectories, value);
        }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        /// <summary>
        /// 入力された prefix をインデックスに変換し、CopyableDirectories の該当インデックスのディレクトリにファイルをコピーします。
        /// </summary>
        /// <param name="prefix">インデックスに変換するプレフィックスです。変換は a = 0, b = 1 .. という具合です。</param>
        public void CopyFile(char prefix)
        {
            var targetIndex = prefix - 'a';
            if (targetIndex < 0 || targetIndex >= copyableDirectories.Count)
            {
                return;
            }

            var dest = CopyableDirectories[targetIndex];
            foreach (var exFileInfo in CurrentFiles)
            {
                File.Copy(exFileInfo.FileSystemInfo.FullName, dest.FileSystemInfo.FullName);
            }
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (!parameters.TryGetValue(nameof(CopyableDirectories), out ObservableCollection<ExFileInfo> d))
            {
                return;
            }

            if (d == null)
            {
                return;
            }

            CopyableDirectories = d;
            const int initialIndex = 'a';
            for (var i = 0; i < CopyableDirectories.Count; i++)
            {
                CopyableDirectories[i].KeyChar = (char)(initialIndex + i);
            }
        }
    }
}