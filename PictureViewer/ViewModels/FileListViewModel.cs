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
        public ObservableCollection<ExFileInfo> Files { get; set; } = new ();

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