using System;
using System.Windows.Input;

namespace PdfBarcodeSplitter.ViewModels.Commands
{
    internal class SimpleCommand : ICommand
    {
        private readonly Action _action;

        public SimpleCommand(Action action, bool executable)
        {
            _action = action;
            _executable = executable;
        }

        private bool _executable;
        public bool Executable
        {
            get => _executable;
            set
            {
                if (_executable != value)
                {
                    _executable = value;
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        #region ICommand
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return _executable;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke();
        }
        #endregion
    }
}
