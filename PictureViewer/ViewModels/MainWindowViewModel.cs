using PictureViewer.Models;
using Prism.Mvvm;

namespace PictureViewer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        public TextWrapper TextWrapper { get; set; }

        public FileListViewModel FileListViewModel { get; set; } = new ();
    }
}