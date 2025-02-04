using Microsoft.EntityFrameworkCore;
using ATS.Infrastructure.Data;
using MediatR;
using ATS.Application;
using ATS.Application.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using System.Text.Json;
using DinkToPdf;
using System.Runtime.InteropServices;
using DinkToPdf.Contracts;
using ATS_CV.MiddleWare;
using Microsoft.Extensions.Hosting;
using Environment.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Text.RegularExpressions;



var builder = WebApplication.CreateBuilder(args);
//var wkhtmlToPdfLibraryPath=("C:\\Users\\user\\Downloads\\wkhtmltopdf\\bin");
// Change this path to where your wkhtmltox folder is
//DinkToPdf.GlobalConfig.SetLibraryPath(wkhtmlToPdfLibraryPath);
string pathToDll = Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll");
string pathToso = Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.so");
string pathTodylib = Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dylib");


IntPtr nativeLibrary;



if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    nativeLibrary = NativeLibrary.Load(pathToDll);
    Console.WriteLine($"Loaded Windows DLL from {pathToDll}");
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    nativeLibrary = NativeLibrary.Load(pathToso);
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    nativeLibrary = NativeLibrary.Load(pathTodylib);
}
else
{
    throw new InvalidOperationException("Unknown platform.");
}

if (nativeLibrary == IntPtr.Zero)
{
    Console.WriteLine("Failed to load native library.");
}
else
{
    Console.WriteLine("Native library loaded successfully.");

    // Initialize DinkToPdf or perform other tasks
}
builder.Services.AddControllers();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<GlobalExceptionHandler>();
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();
//builder.Services.AddLocalization(options => options.ResourcesPath = "Resourses/Resource");

builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust session timeout as needed
    options.Cookie.HttpOnly = true; // Ensures the session cookie cannot be accessed by client-side scripts
    options.Cookie.IsEssential = true; // Marks the cookie as essential for the app
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
{
    Title = "My API",
    Version = "v1"
}));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<TokenHelper>();
builder.Services.AddScoped<UserHelper>();
builder.Services.AddSingleton<DEVService>();
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Allow any origin (e.g., localhost, production domain, etc.)
              .AllowAnyHeader()  // Allow any header
              .AllowAnyMethod(); // Allow any HTTP method (GET, POST, PUT, DELETE)
    });
});

builder.Services.AddApplication();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options =>
{

});
builder.Services.AddMediatR(typeof(Program));

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(); 

builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddHostedService<MyHostedService>();


//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    var supportedCultures = new[] { "en", "ar" };
//    options.SetDefaultCulture("en")
//           .AddSupportedCultures(supportedCultures)
//           .AddSupportedUICultures(supportedCultures);
//});
builder.Services.Configure<RequestLocalizationOptions>(
   options =>
   {
       var supportedCultures = new List<CultureInfo>()
       {
                new CultureInfo("en"),
                new CultureInfo("ar"),
                
       };

       options.DefaultRequestCulture = new RequestCulture("en");
       options.SupportedCultures = supportedCultures;
       options.SupportedUICultures = supportedCultures;
       //options.RequestCultureProviders = new[] { new Culture.CustomRequestCultureProvider { IndexOfCulture = 1, IndexofUICulture = 1 } };
   });
//builder.Services.Configure<RouteOptions>(options =>
//{
//    options.ConstraintMap.Add("culture", typeof(Culture.LanguageRouteConstraint));
//});

var app = builder.Build();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<GlobalExceptionHandler>();
        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        if (exception != null)
        {
            await exceptionHandler.TryHandleAsync(context, exception, CancellationToken.None);
        }
    });
});
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
//var supportedCultures = new[] { "en", "ar" };
//var localizationOptions = new RequestLocalizationOptions()
//    .SetDefaultCulture("en")
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures)
//    .AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider());

//app.UseRequestLocalization(localizationOptions);

//app.UseRequestLocalization(new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture("en"),
//    SupportedCultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("ar") },
//    SupportedUICultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("ar") }
//});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<ValidationMiddleware>();

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    ApplyCurrentCultureToResponseHeaders = true
});

app.Run();
