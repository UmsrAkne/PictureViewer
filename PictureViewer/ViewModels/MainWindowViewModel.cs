using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using PictureViewer.Models;
using Prism.Mvvm;

namespace PictureViewer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            SetDummyData();
        }

        public TextWrapper TextWrapper { get; set; }

        public FileListViewModel FileListViewModel { get; set; } = new ();

        [Conditional("DEBUG")]
        private void SetDummyData()
        {
            FileListViewModel = new FileListViewModel
            {
                Files = new ObservableCollection<ExFileInfo>()
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
                },
            };
        }
    }
}