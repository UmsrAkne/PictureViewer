using System;
using System.Collections.ObjectModel;
using System.IO;
using PictureViewer.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace PictureViewer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DirectoryListViewModel : BindableBase
    {
        private ObservableCollection<ExFileInfo> directories = new ();
        private ExFileInfo selectedItem;
        private string currentPath = string.Empty;

        public ObservableCollection<ExFileInfo> Directories
        {
            get => directories;
            set => SetProperty(ref directories, value);
        }

        /// <summary>
        /// Directories プロパティに含まれるアイテムのうち、現在選択している ExFileInfo を取得・設定します。
        /// </summary>
        public ExFileInfo SelectedItem
        {
            get => selectedItem;
            set
            {
                CurrentPath = value?.FullPath;
                SetProperty(ref selectedItem, value);
            }
        }

        /// <summary>
        /// 現在選択しているディレクトリのフルパスを取得・設定します。
        /// </summary>
        public string CurrentPath { get => currentPath; set => SetProperty(ref currentPath, value); }

        public DelegateCommand AddCurrentDirectoryCommand => new DelegateCommand(() =>
        {
            Directories.Add(new ExFileInfo(new DirectoryInfo(CurrentPath)));
        });

        /// <summary>
        /// 現在のディレクトリを閉じるコマンドです。<br/>
        /// 引数として渡された param が、Directories に含まれている場合、そのディレクトリをリストから削除します。<br/>
        /// ディレクトリが削除された後、削除したディレクトリと同じインデックスのディレクトリ、それが不可能なときは、リスト末尾のディレクトリを選択状態にします。<br/>
        /// 削除後にディレクトリが残っていない場合は、選択項目は変更されません。
        /// null または Directories" に存在しないディレクトリが渡された場合、処理は行われません。
        /// </summary>
        public DelegateCommand<ExFileInfo> CloseCurrentDirectoryCommand => new DelegateCommand<ExFileInfo>((param) =>
        {
            if (param == null || !Directories.Contains(param))
            {
                return;
            }

            var index = Directories.IndexOf(param);
            Directories.RemoveAt(index);

            if (Directories.Count > 0)
            {
                SelectedItem = Directories[Math.Min(Directories.Count - 1, index)];
            }
        });
    }
}