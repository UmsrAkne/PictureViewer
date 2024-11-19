using System.Collections.ObjectModel;
using PictureViewer.Models;
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
        public ExFileInfo SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }

        /// <summary>
        /// 現在選択しているディレクトリのフルパスを取得・設定します。
        /// </summary>
        public string CurrentPath { get => currentPath; set => SetProperty(ref currentPath, value); }
    }
}