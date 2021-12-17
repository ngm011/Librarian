using Librarian.ApiPortal.Exceptions;
using Librarian.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Librarian.ApiPortal.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ExternalException ex)
            {
                var ms = (Dictionary<string, IList<string>>)ex;
                var keys = ms.Keys.Select(k => k == "" ? "{Empty}" : k);
                var csv = String.Join(", ", keys);

                _logger.LogDebug($"External Exception for: {csv}.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var body = JsonConvert.SerializeObject(ms);

                await context.Response.WriteAsync(body);
            }
            catch (CatalogServiceRetrievalException ex)
            {
                _logger.LogError(ex, $"Catalog Service service failed to be connected to for path: {context.Request.Path}");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            catch (CatalogUsagePrintServiceTrackingException ex)
            {
                _logger.LogError(ex, $"Catalog Tracking Print service was unable to track the usage: {context.Request.Path}");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Unhandled exception.");
                throw;
            }
        }
    }

}
