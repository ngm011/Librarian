using Librarian.ApiPortal.Auth;
using Librarian.Data;
using Librarian.Services;
using Librarian.Services.GoogleBooks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Librarian.ApiPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var secKeyConfig = this.Configuration.Get<SecurityKeysConfiguration>();
            var authenticator = new JwtAuthenticator(secKeyConfig.SecretKey, secKeyConfig.ApiKey);

            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(c =>
            {
                c.RequireHttpsMetadata = false;
                c.SaveToken = true;
                c.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = authenticator.SymmetricSecurityKey
                };
            });

            services.AddDbContextFactory<CatalogContext>(ob =>
                ob.UseSqlServer(this.Configuration.GetConnectionString("LibrarianDB")),
                ServiceLifetime.Scoped);
            services.AddSingleton<IAuthenticator>(
                authenticator);
            services.AddTransient<ICatalogService>(sp =>
                new GoogleBooksCatalogService(secKeyConfig.GoogleApiKey));
            services.AddTransient<ICatalogUsagePrintService,
                CatalogUsagePrintService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Librarian", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Librarian v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
