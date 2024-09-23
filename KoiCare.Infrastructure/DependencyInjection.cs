using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Infrastructure.Dependencies.Database;
using KoiCare.Infrastructure.Dependencies.Firebase.Authentication;
using KoiCare.Infrastructure.Dependencies.Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KoiCare.Infrastructure
{
    public static class DependencyInjection
    {
        private static readonly string[] ConfigureOptions = ["en-US", "vi-VN"];

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(configuration.GetSection("FirebaseAdminSdk").Value)
            });

            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddHttpContextAccessor();

            services.AddHttpClient<IJwtProvider, JwtProvider>();

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = configuration.GetSection("Authentication:ValidIssuer").Value;
                    options.Audience = configuration.GetSection("Authentication:Audience").Value;
                    options.TokenValidationParameters.ValidIssuer = configuration.GetSection("Authentication:ValidIssuer").Value;
                });

            services.AddLocalization(options => options.ResourcesPath = "");
            services.AddScoped<IAppLocalizer, AppLocalizer>();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = ConfigureOptions;
                options.SetDefaultCulture(supportedCultures[0]);
                options.AddSupportedCultures(supportedCultures);
                options.AddSupportedUICultures(supportedCultures);
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("KoiCare.Infrastructure"));
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
