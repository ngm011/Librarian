using Librarian.KioskClient.Catalog.Models;
using System.Threading.Tasks;

namespace Librarian.KioskClient.Catalog.Clients
{
    public interface ICatalogClient
    {
        string SearchTitleTerm { get; set; }
        string SearchAuthorTerm { get; set; }

        public Task<CatalogResult> RetrieveAsync();
    }
}
