using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.Email;
using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Infrastructure.Dependencies.Database;
using KoiCare.Infrastructure.Dependencies.Firebase.Authentication;
using KoiCare.Infrastructure.Dependencies.Localization;
using KoiCare.Infrastructure.Dependencies.LoggedUser;
using KoiCare.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Net;

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

            services.AddScoped<ILoggedUser, LoggedUser>();

            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            // Thêm các đăng ký cho IEmailService và EmailService
            services.AddTransient<IEmailService, EmailService>();

            // Cấu hình SmtpSettings từ appsettings.json
            services.Configure<SmtpSettings>(configuration.GetSection("Email:SmtpSettings"));

            // Đăng ký SmtpClient
            services.AddSingleton<SmtpClient>(sp =>
            {
                var smtpSettings = sp.GetRequiredService<IOptions<SmtpSettings>>().Value;
                // Kiểm tra các giá trị SMTP
                if (string.IsNullOrEmpty(smtpSettings.Host))
                {
                    throw new ArgumentNullException(nameof(smtpSettings.Host), "SMTP Host must not be null or empty.");
                }
                if (smtpSettings.Port <= 0) // Kiểm tra giá trị cổng
                {
                    throw new ArgumentOutOfRangeException(nameof(smtpSettings.Port), "SMTP Port must be a positive number.");
                }
                if (string.IsNullOrEmpty(smtpSettings.UserName) || string.IsNullOrEmpty(smtpSettings.Password))
                {
                    throw new ArgumentNullException("SMTP Username and Password must not be null or empty.");
                }
                return new SmtpClient(smtpSettings.Host)
                {
                    Port = smtpSettings.Port,
                    EnableSsl = smtpSettings.EnableSsl,
                    Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password)
                };
            });

            return services;
        }
    }
}