using HMI.Data;
using HMI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add database connection
var connectionString = builder.Configuration.GetConnectionString("UserConnection");
builder.Services.AddDbContext<UserDbContext>(options=>
    options.UseSqlServer(connectionString));

// Add identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

// Add services
builder.Services.AddScoped<IAuthService, AuthService>();

// Add Jwt Bearer
//builder.Services.AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    })
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("JwtConfig:Issuer").Value,
            ValidAudience = builder.Configuration.GetSection("JwtConfig:Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value))
        };
    });

//// <snippet_LocalizationConfigurationServices>
//builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

//builder.Services.AddMvc()
//    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
//    .AddDataAnnotationsLocalization();
//// </snippet_LocalizationConfigurationServices>

//// <snippet_RequestLocalizationOptionsConfiguration>
//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    var supportedCultures = new[] { "fr-FR", "fr" };
//    options.SetDefaultCulture(supportedCultures[0])
//        .AddSupportedCultures(supportedCultures)
//        .AddSupportedUICultures(supportedCultures);
//});
//// </snippet_RequestLocalizationOptionsConfiguration>

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("fr-FR"),
        new CultureInfo("fr")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "fr-FR", uiCulture: "fr-FR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
    {
        // My custom request culture logic
        return await Task.FromResult(new ProviderCultureResult("fr"));
    }));
});

// Add logging
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// Add HttpClient
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//// <snippet_ConfigureLocalization>
//var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
//app.UseRequestLocalization(localizationOptions.Value);
//// </snippet_ConfigureLocalization>
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();
