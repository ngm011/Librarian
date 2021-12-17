using System.Threading.Tasks;

namespace Librarian.Services
{
    public interface ICatalogUsagePrintService
    {
        Task TrackAsync(string originatingIP, string originatingHost, string query, int resultCount);
    }
}
