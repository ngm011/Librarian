using Librarian.Services.Result;
using System.Threading.Tasks;

namespace Librarian.Services
{
    public interface ICatalogService
    {
        string SearchTitleTerm { get; set; }
        string SearchAuthorTerm { get; set; }

        Task<CatalogResult> RetrieveAsync();
    }
}
