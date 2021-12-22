using Librarian.KioskClient.Catalog.Clients;
using Librarian.KioskClient.Catalog.ViewModels;
using Librarian.KioskClient.Hub.ViewModels;
using Librarian.KioskClient.MvvmInfrastructure;
using Librarian.KioskClient.MvvmInfrastructure.Commanding;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace Librarian.KioskClient.Shell.ViewModels
{
    public class ShellVM : BaseViewModel
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private const int Inactivity = 60_000;
        private EventHandler _onInactive;

        public ShellVM()
        {
            NavigateToHomeCommand = new RelayCommand(NavigateToHome);
            NavigateToHome();
        }

        private BaseViewModel _content;

        public BaseViewModel Content
        {
            get => _content;
            private set
            {
                if (SetValue(ref _content, value))
                {
                    OnPropertyChanging(nameof(IsHomeVisible));
                    OnPropertyChanged(nameof(IsHomeVisible));
                }
            }
        }

        public bool IsHomeVisible =>
            this.Content is not HubVM;

        public ICommand NavigateToHomeCommand { get; }

        public void NavigateToHome()
        {
            ToggleInactivityWatcher();

            this.Content = new HubVM(st => 
            {
                ToggleInactivityWatcher();

                this.Content = new CatalogVM(
                    () => new CatalogClient(),
                    ToggleInactivityWatcher) 
                { 
                    SearchType = st 
                };
            });
        }

        private void ToggleInactivityWatcher()
        {
            _timer.Stop();

            _timer.Tick -= _onInactive;
            _timer.Interval = TimeSpan.FromMilliseconds(Inactivity);
            _onInactive = (s, args) => this.Content = new HubVM(st =>
                this.Content = new CatalogVM(
                    () => new CatalogClient(), 
                    ToggleInactivityWatcher) 
                { 
                    SearchType = st 
                });
            _timer.Tick += _onInactive;

            _timer.Start();
        }
    }
}
