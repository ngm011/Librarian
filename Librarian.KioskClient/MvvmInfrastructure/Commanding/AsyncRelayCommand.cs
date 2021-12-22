using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Librarian.KioskClient.MvvmInfrastructure.Commanding
{
    public class AsyncRelayCommand : ICommand
    {
        #region Private State
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly Action<bool> _onExecuting;
        private bool _isExecuting = false;
        private bool _allowConcurrency = false;
        #endregion

        #region Constructors
        public AsyncRelayCommand(Func<Task> execute, Action<bool> onExecuting) : this(execute, onExecuting, null) { }

        public AsyncRelayCommand(Func<Task> execute, Action<bool> onExecuting, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _onExecuting = onExecuting;
            _canExecute = canExecute;
            _allowConcurrency = false;
        }

        public AsyncRelayCommand(Func<Task> execute, Action<bool> onExecuting, Func<bool> canExecute, bool allowConcurrency)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _onExecuting = onExecuting;
            _canExecute = canExecute;
            _allowConcurrency = allowConcurrency;
        }
        #endregion

        #region CanExecute
        public bool CanExecute() => CanExecuteInternal();
        bool ICommand.CanExecute(object parameter) => CanExecuteInternal();

        private bool CanExecuteInternal()
        {
            if (_isExecuting && !_allowConcurrency) return false;
            if (_canExecute == null) return true;

            return _canExecute();
        }

        public void Invalidate() => CommandManager.InvalidateRequerySuggested();

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute == null) return;

                CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute == null) return;

                CommandManager.RequerySuggested -= value;
            }
        }
        #endregion

        #region Execute
        public void Execute() => _ = ExecuteInternalAsync();
        void ICommand.Execute(object parameter) => _ = ExecuteInternalAsync();

        private async Task ExecuteInternalAsync()
        {
            if (!this.CanExecute()) return;

            OnExecuting(true);

            try
            {
                await _execute();
            }
            finally
            {
                OnExecuting(false);
            }
        }
        #endregion

        #region OnExecuting
        private void OnExecuting(bool isExecuting)
        {
            _isExecuting = isExecuting;

            this.Invalidate();

            _onExecuting?.Invoke(isExecuting);
        }
        #endregion
    }

    public class AsyncRelayCommand<T> : ICommand
    {
        #region Private State
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;
        private readonly Action<bool, T> _onExecuting;
        private bool _isExecuting = false;
        private bool _allowConcurrency = false;
        #endregion

        #region Constructors
        public AsyncRelayCommand(Func<T, Task> execute, Action<bool, T> onExecuting) : this(execute, onExecuting, null) { }

        public AsyncRelayCommand(Func<T, Task> execute, Action<bool, T> onExecuting, Func<T, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _onExecuting = onExecuting;
            _canExecute = canExecute;
            _allowConcurrency = false;
        }

        public AsyncRelayCommand(Func<T, Task> execute, Action<bool, T> onExecuting, Func<T, bool> canExecute, bool allowConcurrency)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _onExecuting = onExecuting;
            _canExecute = canExecute;
            _allowConcurrency = allowConcurrency;
        }
        #endregion

        #region CanExecute
        public bool CanExecute(T parameter) => CanExecuteInternal(parameter);
        bool ICommand.CanExecute(object parameter) => CanExecuteInternal(parameter);

        private bool CanExecuteInternal(object parameter)
        {
            if (_isExecuting && !_allowConcurrency) return false;
            if (_canExecute == null) return true;

            var paramValue = GetParameterValue(parameter);

            return _canExecute(paramValue);
        }

        public void Invalidate() => CommandManager.InvalidateRequerySuggested();

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute == null) return;

                CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute == null) return;

                CommandManager.RequerySuggested -= value;
            }
        }
        #endregion

        #region Execute
        public void Execute(T parameter) => _ = ExecuteInternalAsync(parameter);
        void ICommand.Execute(object parameter) => _ = ExecuteInternalAsync(parameter);

        private async Task ExecuteInternalAsync(object parameter)
        {
            var paramValue = GetParameterValue(parameter);

            if (!this.CanExecute(paramValue)) return;

            OnExecuting(true, paramValue);

            try
            {
                await _execute(paramValue);
            }
            finally
            {
                OnExecuting(false, paramValue);
            }
        }
        #endregion

        #region OnExecuting
        private void OnExecuting(bool isExecuting, T parameter)
        {
            _isExecuting = isExecuting;

            this.Invalidate();

            _onExecuting?.Invoke(isExecuting, parameter);
        }
        #endregion

        #region Helpers
        private static T GetParameterValue(object parameter)
        {
            var paramValue = parameter;

            if (paramValue == null) return typeof(T).IsValueType ? default(T) : (T)paramValue;

            if (!(parameter is T) && parameter is IConvertible)
                paramValue = Convert.ChangeType(parameter, typeof(T), null);

            if (!(paramValue is T)) throw new ArgumentOutOfRangeException(nameof(parameter));

            return (T)paramValue;
        }
        #endregion
    }
}
