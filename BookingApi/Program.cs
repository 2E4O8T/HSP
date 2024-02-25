using BookingApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.
// Add jwt bearer
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

// Add database connection
var connectionString = builder.Configuration.GetConnectionString("BookingConnection");
builder.Services.AddDbContext<BookingApiDbContext>(options =>
    options.UseSqlServer(connectionString));

// check appointment availability
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//// <snippet_ConfigureLocalization>
//var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
//app.UseRequestLocalization(localizationOptions.Value);
//// </snippet_ConfigureLocalization>
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
