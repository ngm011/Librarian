using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Librarian.KioskClient.MvvmInfrastructure
{
    public abstract class BaseViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constructors
        public BaseViewModel() =>
            _syncContext = SynchronizationContext.Current ?? throw new InvalidOperationException("ViewModel has to be created on main thread");
        #endregion

        #region SyncContext
        private readonly SynchronizationContext _syncContext;

        protected SynchronizationContext SyncContext => _syncContext;
        #endregion

        #region Property Change Management
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanging([CallerMemberName] string propertyName = null) =>
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            OnPropertyChanging(propertyName);

            field = value;

            OnPropertyChanged(propertyName);

            return true;
        }
        #endregion

        #region IsBusy
        private bool _isBusy = false;

        public bool IsBusy
        {
            get => _isBusy;
            private set => SetValue(ref _isBusy, value);
        }

        public virtual void SetAsBusy(bool isBusy) => IsBusy = isBusy;
        public virtual void SetAsBusy<T>(bool isBusy, T parameter) => SetAsBusy(isBusy);
        #endregion
    }
}
