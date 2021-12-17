using Librarian.ApiPortal.Auth;
using Librarian.ApiPortal.Exceptions;
using Librarian.ApiPortal.Extensions;
using Librarian.ApiPortal.Models;
using Librarian.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Librarian.ApiPortal.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly IAuthenticator _authenticator;
        private readonly ICatalogService _catalogService;
        private readonly ICatalogUsagePrintService _catalogUsagePrintService;

        public LibraryController(
            IAuthenticator authenticator,
            ICatalogService catalogService,
            ICatalogUsagePrintService catalogUsagePrintService)
        {
            _authenticator = authenticator;
            _catalogService = catalogService;
            _catalogUsagePrintService = catalogUsagePrintService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(SearchRequest searchRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            EnsureMaintenance();

            _catalogService.SearchTitleTerm = searchRequest.TitleTerm;
            _catalogService.SearchAuthorTerm = searchRequest.AuthorTerm;

            var result = await _catalogService.RetrieveAsync();

            await this.HttpContext.TrackCatalogQueryAsync(
                _catalogUsagePrintService,
                result.Infos.Length);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] string apiKey)
        {
            if (_authenticator.Authenticate(apiKey) is string token)
                return Ok(token);
            else
                return Unauthorized();
        }

        private static void EnsureMaintenance()
        {
            var now = DateTime.Now;

            if (now.Hour == 0 && now.Minute <= 33)
                throw new ExternalException("Maintenance", "The daily maintenance is being performed on the service");
        }
    }
}
