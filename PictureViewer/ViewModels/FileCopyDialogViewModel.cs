using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        private ObservableCollection<ExDirectoryInfo> copyableDirectories = new ();
        private ObservableCollection<ExFileInfo> currentFiles = new ();

        public event Action<IDialogResult> RequestClose;

        public string Title => nameof(FileCopyDialog);

        public ObservableCollection<ExFileInfo> CurrentFiles
        {
            get => currentFiles;
            set => SetProperty(ref currentFiles, value);
        }

        public ObservableCollection<ExDirectoryInfo> CopyableDirectories
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
        public void CopyFile(string prefix)
        {
            var targetIndex = prefix.First() - 'a';
            if (targetIndex < 0 || targetIndex >= copyableDirectories.Count)
            {
                return;
            }

            var dest = CopyableDirectories[targetIndex];
            foreach (var exFileInfo in CurrentFiles)
            {
                File.Copy(exFileInfo.FileSystemInfo.FullName, dest.FullPath);
            }
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var containsKeys = parameters.ContainsKey(nameof(CopyableDirectories))
                               && parameters.ContainsKey(nameof(CurrentFiles));

            if (!containsKeys)
            {
                return;
            }

            var d = parameters.GetValue<ObservableCollection<ExDirectoryInfo>>(nameof(CopyableDirectories));
            var f = parameters.GetValue<ObservableCollection<ExFileInfo>>(nameof(CurrentFiles));

            if (d == null || f == null)
            {
                return;
            }

            CopyableDirectories = d;
            CurrentFiles = f;

            const int initialIndex = 'a';
            for (var i = 0; i < CopyableDirectories.Count; i++)
            {
                CopyableDirectories[i].KeyString = ((char)(initialIndex + i)).ToString();
            }
        }
    }
}