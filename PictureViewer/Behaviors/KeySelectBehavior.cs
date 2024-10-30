using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using PictureViewer.ViewModels;

namespace PictureViewer.Behaviors
{
    public class KeySelectBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not ListBox lb || lb.Items.Count == 0)
            {
                return;
            }

            if (e.Key is < Key.A or > Key.Z)
            {
                return;
            }

            if (lb.DataContext is not FileCopyDialogViewModel vm)
            {
                return;
            }

            vm.CopyFile(e.Key.ToString().ToLower());
        }
    }
}