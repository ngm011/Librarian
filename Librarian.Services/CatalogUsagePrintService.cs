using Librarian.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Librarian.Services
{
    public class CatalogUsagePrintService : ICatalogUsagePrintService
    {
        private readonly IDbContextFactory<CatalogContext> _catalogContextFactory;

        public CatalogUsagePrintService(IDbContextFactory<CatalogContext> catalogContextFactory) =>
            _catalogContextFactory = catalogContextFactory ?? throw new ArgumentNullException(nameof(catalogContextFactory));

        public Task TrackAsync(string originatingIP, string originatingHost, string query, int resultCount)
        {
            if (originatingIP is null) throw new ArgumentNullException(nameof(originatingIP));
            if (String.IsNullOrWhiteSpace(originatingIP)) throw new ArgumentException($"{nameof(originatingIP)} has to be specified", nameof(originatingIP));
            if (originatingHost is null) throw new ArgumentNullException(nameof(originatingHost));
            if (String.IsNullOrWhiteSpace(originatingHost)) throw new ArgumentException($"{nameof(originatingHost)} has to be specified", nameof(originatingHost));
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (String.IsNullOrWhiteSpace(query)) throw new ArgumentException($"{nameof(query)} has to be specified", nameof(query));
            if (resultCount < 0) throw new ArgumentOutOfRangeException(nameof(resultCount), $"{nameof(resultCount)} cannot be negative");

            return InternalTrackAsync(originatingIP, originatingHost, query, resultCount);
        }

        private async Task InternalTrackAsync(string originatingIP, string originatingHost, string query, int resultCount)
        {
            var ctx = _catalogContextFactory.CreateDbContext();

            throw new CatalogUsagePrintServiceTrackingException("tst", null);

            ctx.UsagePrints?.Add(
                new CatalogUsagePrintDto()
                {
                    OriginatingIP = originatingIP,
                    OriginatingHost = originatingHost,
                    Query = query,
                    ResultCount = resultCount
                });

            try
            {
                await ctx.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw new CatalogUsagePrintServiceTrackingException("Unable to flush tracking data", ex);
            }
        }
    }
}
