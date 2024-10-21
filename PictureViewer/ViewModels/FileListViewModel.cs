using System.Collections.ObjectModel;
using PictureViewer.Models;
using Prism.Mvvm;

namespace PictureViewer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FileListViewModel : BindableBase
    {
        public ObservableCollection<ExFileInfo> Files { get; set; } = new ();
    }
}