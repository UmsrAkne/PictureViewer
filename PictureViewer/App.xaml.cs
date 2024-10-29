using System.Windows;
using PictureViewer.ViewModels;
using PictureViewer.Views;
using Prism.Ioc;

namespace PictureViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<TextInputDialog, TextInputDialogViewModel>();
            containerRegistry.RegisterDialog<FileCopyDialog, FileCopyDialogViewModel>();
        }
    }
}