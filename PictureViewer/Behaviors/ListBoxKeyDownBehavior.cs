using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace PictureViewer.Behaviors
{
    public class ListBoxKeyDownBehavior : Behavior<ListBox>
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

            switch (e.Key)
            {
                case Key.J:
                    if (lb.SelectedIndex < lb.Items.Count - 1)
                    {
                        lb.SelectedIndex++;
                    }

                    break;
                case Key.K:
                    if (lb.SelectedIndex > 0)
                    {
                        lb.SelectedIndex--;
                    }

                    break;
            }

            lb.ScrollIntoView(lb.SelectedItem);
        }
    }
}