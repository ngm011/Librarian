using Librarian.KioskClient.Catalog.Clients;
using Librarian.KioskClient.Catalog.Models;
using Librarian.KioskClient.MvvmInfrastructure;
using Librarian.KioskClient.MvvmInfrastructure.Commanding;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Librarian.KioskClient.Catalog.ViewModels
{
    public class CatalogVM : BaseViewModel
    {
        private readonly Func<ICatalogClient> _clientFactory;
        private readonly Action _toggleInactivityWatcher;

        public CatalogVM(Func<ICatalogClient> clientFactory, Action toggleInactivityWatcher)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(_clientFactory));
            _toggleInactivityWatcher = toggleInactivityWatcher ?? throw new ArgumentNullException(nameof(toggleInactivityWatcher));
            SearchCommand = new AsyncRelayCommand(Search, b => SetAsBusy(b));
        }

        private CatalogResult _catalog;

        public CatalogResult Catalog
        {
            get => _catalog;
            private set => SetValue(ref _catalog, value);
        }

        private string _searchKeywod;

        public string SearchKeyword
        {
            get => _searchKeywod;
            set
            {
                if (SetValue(ref _searchKeywod, value))
                    _toggleInactivityWatcher();
            }
        }

        private string _lastSearchedKeyword;

        public string LastSearchedKeyword
        {
            get => _lastSearchedKeyword;
            private set => SetValue(ref _lastSearchedKeyword, value);
        }

        private SearchType _searchType;

        public SearchType SearchType
        {
            get => _searchType;
            set => SetValue(ref _searchType, value);
        }

        public ICommand SearchCommand { get; }

        private string _errorDisplay;

        public string ErrorDisplay 
        {
            get => _errorDisplay;
            private set => SetValue(ref _errorDisplay, value);
        }

        public async Task Search()
        {
            this.Catalog = new CatalogResult();
            this.ErrorDisplay = null;

            _toggleInactivityWatcher();

            var keyword = this.SearchKeyword;

            this.SearchKeyword = "";
            this.LastSearchedKeyword = keyword;

            SetAsBusy(true);

            var client = _clientFactory();

            client.SearchTitleTerm = this.SearchType == SearchType.ByTitle ?
                keyword : "";
            client.SearchAuthorTerm = this.SearchType == SearchType.ByAuthor ?
                keyword : "";

            try
            {
                this.Catalog = await client.RetrieveAsync();
            }
            catch (Exception ex)
            {
                this.ErrorDisplay = $"Oooops, something went wrong ({ex.Message})";
            }
            finally
            {
                SetAsBusy(false);
            }
        }
    }
}
