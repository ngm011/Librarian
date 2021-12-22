using Librarian.KioskClient.Catalog.ViewModels;
using Librarian.KioskClient.MvvmInfrastructure;
using Librarian.KioskClient.MvvmInfrastructure.Commanding;
using System;
using System.Windows.Input;

namespace Librarian.KioskClient.Hub.ViewModels
{
    public class HubVM : BaseViewModel
    {
        public HubVM(Action<SearchType> navigateToCatalog)
        {
            _navigateToCatalog = navigateToCatalog ?? throw new ArgumentNullException(nameof(navigateToCatalog));
            NavigateToCatalogCommand = new RelayCommand<SearchType>(NavigateToCatalog);
        }

        private readonly Action<SearchType> _navigateToCatalog;

        public ICommand NavigateToCatalogCommand { get; }

        public void NavigateToCatalog(SearchType searchType) =>
            _navigateToCatalog(searchType);
    }
}
