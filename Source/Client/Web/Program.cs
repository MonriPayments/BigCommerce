using Autofac;
using Autofac.Extensions.DependencyInjection;
using BigCommerceApi.Client.Web.Cache;
using BigCommerceApi.Domain.Services.Cache;
using BigCommerceApi.Domain.Services.Jwt;
using BigCommerceApi.Domain.Services.RestManagementApi;
using BigCommerceApi.Domain.Services.WSPay;
using BigCommerceApi.Domain.Services.WSPayForm;
using BigCommerceApi.Persistency.NHibernate;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;
using WebStudio.Common.Helpers;
using WebStudio.Logging.MSSQLServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddSingleton<IWebAppCache, WebAppCache>();

builder.Host.ConfigureContainer<ContainerBuilder>(_ => _
    .RegisterModule(new BigCommerceApi.Domain.Services.Module())
    .RegisterModule(new BigCommerceApi.Client.Web.Module("BigCommerceApi"))
    .RegisterModule(new BigCommerceApi.Persistency.NHibernate.Module(builder.Configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>())));

builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(MSSQLServerLoggerConfiguration)).Get<MSSQLServerLoggerConfiguration>());
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(FormWSPayConfiguration)).Get<FormWSPayConfiguration>()!);
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(WSPayPaymentGatewayConfiguration)).Get<WSPayPaymentGatewayConfiguration>()!);
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(BigCommerceConfiguration)).Get<BigCommerceConfiguration>()!);

builder.Services.AddSingleton<JwtTokenGenerator>();

builder.Services.AddControllers();

builder.Services.AddMvcCore();
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.RootDirectory = "/Pages";
    });
builder.Services.AddMvc().AddRazorRuntimeCompilation();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization();

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("hr-HR"),  // Croatian
    new CultureInfo("mk-MK"),  // Macedonian
    new CultureInfo("sr-Latn-ME"),  // Montenegrin
    new CultureInfo("sq-AL")   // Albanian
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BigCommerceApi",
        Version = "v1"
    });
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

// Add Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors(_ => _.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

app.Use(async (context, next) =>
{
    var cultureQuery = context.Request.Query["Lang"];

    if (!string.IsNullOrWhiteSpace(cultureQuery))
    {
        var culture = GetCultureInfo(cultureQuery);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    await next.Invoke();
});

CultureInfo GetCultureInfo(string lang)
{
    var langCultureInfoMapping = new Dictionary<string, string>
    {
        { "hr", "hr-HR" },
        { "en", "en-US" },
        { "mk", "mk-MK"},
        { "cg", "sr-Latn-ME"},
        { "al", "sq-AL"},
        { "sl", "sl-SI"}        
    };

    if (langCultureInfoMapping.TryGetValue(lang.ToLower(), out var fullCultureInfoName))
    {
        return new CultureInfo(fullCultureInfoName);
    }

    return new CultureInfo("en-US");
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

WebStudio.Entities.Core.Environment.IdGenerator = type => GuidUtility.NewSequentialGuid();

app.Run();
