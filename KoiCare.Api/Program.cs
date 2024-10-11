using KoiCare.Application.Features.Account;
using KoiCare.Application.Abtractions.Authentication;
using KoiCare.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.Options;
using System.Net.Mail;

var allowSpecificOrigins = "_koiCareAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add Infrastructure services (đã bao gồm IEmailService, EmailService, SmtpClient)
builder.Services.AddInfrastructure(builder.Configuration);

// Configure RouteOptions
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateUser).Assembly));

// Configure Swagger with JWT authentication
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    c.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Add Controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI services (đã được cấu hình ở trên, không cần thêm lại)
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(); // Không cần gọi lại nếu đã gọi ở trên

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(allowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseRequestLocalization();

app.MapControllers();

app.Run();
