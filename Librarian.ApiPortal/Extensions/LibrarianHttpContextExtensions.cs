using Librarian.Services;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace Librarian.ApiPortal.Extensions
{
    public static class LibrarianHttpContextExtensions
    {
        public static Task TrackCatalogQueryAsync(this HttpContext context, ICatalogUsagePrintService catalogUsagePrintService, int resultCount)
        {
            var ip = context.Connection.RemoteIpAddress;
            var host = Dns.GetHostEntry(ip)?.HostName ?? "Unknown";
            var query = context.Request.QueryString.ToString();

            return catalogUsagePrintService.TrackAsync(
                originatingIP: ip.MapToIPv4().ToString(),
                originatingHost: host,
                query: query,
                resultCount: resultCount);
        }
    }
}
