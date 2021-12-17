using Librarian.Services.GoogleBooks.Response;
using Librarian.Services.Result;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Librarian.Services.GoogleBooks
{
    public class GoogleBooksCatalogService : ICatalogService
    {
        private const string Endpoint = "https://www.googleapis.com/books/v1/volumes?q=+intitle:{0}+inauthor:{1}&key={2}";
        private readonly string _apiKey;

        public GoogleBooksCatalogService(string apiKey)
        {
            if (apiKey is null) throw new ArgumentNullException(nameof(apiKey));
            if (String.IsNullOrWhiteSpace(apiKey)) throw new ArgumentException($"{nameof(apiKey)} has to be specified", nameof(apiKey));

            _apiKey = apiKey;
        }

        public string SearchTitleTerm { get; set; } = "";
        public string SearchAuthorTerm { get; set; } = "";

        public async Task<CatalogResult> RetrieveAsync()
        {
            var bookRes = default(GoogleBookResponse);

            using (var client = new HttpClient())
                try
                {
                    using (var res = await client.GetAsync(BuildUri()))
                        bookRes = JsonConvert.DeserializeObject<GoogleBookResponse>(
                            await res.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    throw new CatalogServiceRetrievalException("Unable to retrieve library results", ex);
                }

            if (bookRes?.Items is null || bookRes.Items.Length == 0)
                return new CatalogResult(Array.Empty<BookInfo>());

            return new CatalogResult(bookRes.Items.Select(v =>
                new BookInfo(
                    v.Id ?? "",
                    v.VolumeInfo?.Title ?? "",
                    v.VolumeInfo?.Subtitle ?? "",
                    v.VolumeInfo?.Authors ?? Array.Empty<string>(),
                    v.VolumeInfo?.ImageLinks?.Thumbnail ?? "")).ToArray());
        }

        private string BuildUri() =>
            String.Format(Endpoint, this.SearchTitleTerm ?? "", this.SearchAuthorTerm ?? "", _apiKey);
    }
}
