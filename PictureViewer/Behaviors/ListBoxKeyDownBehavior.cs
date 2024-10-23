using System;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using PictureViewer.Models;

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
                case Key.G:
                    lb.SelectedIndex = Keyboard.Modifiers == ModifierKeys.Shift
                        ? lb.Items.Count - 1
                        : 0;

                    break;
            }

            lb.ScrollIntoView(lb.SelectedItem);

            if (e.Key is Key.J or Key.K)
            {
                // key が j, k の場合はカーソル移動なので、ここで処理を中断する
                return;
            }

            if (e.Key is >= Key.F or < Key.A)
            {
                if (e.Key != Key.N)
                {
                    return;
                }
            }

            if (lb.SelectedItem is not ExFileInfo item)
            {
                return;
            }

            var r = e.Key != Key.N
                ? (Rating)Enum.Parse(typeof(Rating), e.Key.ToString())
                : Rating.NoRating;

            item.Rating = r;
        }
    }
}