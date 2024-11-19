using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PictureViewer.Commands
{
    public class AsyncDelegateCommand : ICommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;

        public AsyncDelegateCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync();
        }

        private async Task ExecuteAsync()
        {
            await execute();
        }
    }
}