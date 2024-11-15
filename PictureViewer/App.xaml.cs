using System.Windows;
using PictureViewer.Models;
using PictureViewer.Models.Dbs;
using PictureViewer.ViewModels;
using PictureViewer.Views;
using Prism.Ioc;
using Prism.Services.Dialogs;

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
            containerRegistry.RegisterSingleton<DatabaseContext>();

            var d = Container.Resolve<DatabaseContext>();
            d.Database.EnsureCreated();

            var repo = new Repository<ExFileInfo>(d);
            containerRegistry.RegisterInstance(repo);

            var imageFileService = new ImageFileService(repo);
            containerRegistry.RegisterInstance(imageFileService);

            containerRegistry.Register<IDialogService, DialogService>();
        }
    }
}