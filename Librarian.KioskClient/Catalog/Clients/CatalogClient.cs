using Librarian.KioskClient.Catalog.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Librarian.KioskClient.Catalog.Clients
{
    public class CatalogClient : ICatalogClient
    {
        private const string AuthEndpoint = "/Library/auth";
        private const string CatalogEndpoint = "/Library?titleTerm={0}&authorTerm={1}";
        private readonly string _apiKey;

        public CatalogClient() =>
            _apiKey = ConfigurationManager.AppSettings["ApiKey"] ?? throw new InvalidOperationException();

        public string SearchTitleTerm { get; set; } = "";
        public string SearchAuthorTerm { get; set; } = "";

        public async Task<CatalogResult> RetrieveAsync()
        {
            using (var client = new HttpClient())
            {
                var authToken = default(string);

                using (var authRes = await client.PostAsync(
                    BuildAuthUri(),
                    new StringContent("\"" + _apiKey + "\"", Encoding.UTF8, "application/json")))
                {
                    authRes.EnsureSuccessStatusCode();

                    authToken = await authRes.Content.ReadAsStringAsync();
                }

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);

                using (var catRes = await client.GetAsync(BuildCatalogUri()))
                {
                    if (!catRes.IsSuccessStatusCode)
                        throw new InvalidOperationException(await ExtractErrorMessageAsync(catRes));

                    return JsonConvert.DeserializeObject<CatalogResult>(
                        await catRes.Content.ReadAsStringAsync());
                }
            }
        }

        private string BuildAuthUri() =>
            String.Format(ConfigurationManager.AppSettings["Endpoint"] + AuthEndpoint);
        private string BuildCatalogUri() =>
            String.Format(ConfigurationManager.AppSettings["Endpoint"] + CatalogEndpoint, this.SearchTitleTerm ?? "", this.SearchAuthorTerm ?? "", _apiKey);

        private async static Task<string> ExtractErrorMessageAsync(HttpResponseMessage response)
        {
            var rawContent = default(string);

            try
            {
                rawContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Dictionary<string, string[]>>(rawContent)
                    .Values
                    .FirstOrDefault()?
                    .FirstOrDefault() ?? "";
            }
            catch
            {
                return String.IsNullOrWhiteSpace(rawContent) ? "Unknown reason" : "";
            }
        }
    }
}
