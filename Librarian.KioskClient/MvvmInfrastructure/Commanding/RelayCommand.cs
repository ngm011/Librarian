using System;
using System.Windows.Input;

namespace Librarian.KioskClient.MvvmInfrastructure.Commanding
{
    public class RelayCommand : ICommand
    {
        #region Private State
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        #endregion

        #region Constructors
        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        #endregion

        #region CanExecute
        public bool CanExecute() => CanExecuteInternal();
        bool ICommand.CanExecute(object parameter) => CanExecuteInternal();

        private bool CanExecuteInternal()
        {
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
        public void Execute() => ExecuteInternal();
        void ICommand.Execute(object parameter) => ExecuteInternal();

        private void ExecuteInternal()
        {
            if (!this.CanExecute()) return;

            _execute();
        }
        #endregion
    }

    public class RelayCommand<T> : ICommand
    {
        #region Private State
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;
        #endregion

        #region Constructors
        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        #endregion

        #region CanExecute
        public bool CanExecute(T parameter) => CanExecuteInternal(parameter);
        bool ICommand.CanExecute(object parameter) => CanExecuteInternal(parameter);

        private bool CanExecuteInternal(object parameter)
        {
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
        public void Execute(T parameter) => ExecuteInternal(parameter);
        void ICommand.Execute(object parameter) => ExecuteInternal(parameter);

        private void ExecuteInternal(object parameter)
        {
            var paramValue = GetParameterValue(parameter);

            if (!this.CanExecute(paramValue)) return;

            _execute(paramValue);
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
