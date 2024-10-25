using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PictureViewer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TextInputDialogViewModel : BindableBase, IDialogAware
    {
        private string text;

        public event Action<IDialogResult> RequestClose;

        public string Title => string.Empty;

        public string Text { get => text; set => SetProperty(ref text, value); }

        public DelegateCommand<ButtonResult?> CloseCommand => new DelegateCommand<ButtonResult?>((result) =>
        {
            if (result == null)
            {
                RequestClose?.Invoke(new DialogResult());
            }

            var r = new DialogResult(result.Value);
            r.Parameters.Add(nameof(Text), Text);
            RequestClose?.Invoke(new DialogResult(result.Value));
        });

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}