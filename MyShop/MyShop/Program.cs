using MyShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MyShop.Application;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyShop.Application.Helpers;
using Microsoft.AspNetCore.Session;
using Serilog;


//using MyShop.Middleware;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddControllers()
//            .AddJsonOptions(options =>
//            {
//                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//            });


// Register services in the DI container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped<EnsureCartPaidAttribute>();
// Register Repositories and Services
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust session timeout as needed
    options.Cookie.HttpOnly = true; // Ensures the session cookie cannot be accessed by client-side scripts
    options.Cookie.IsEssential = true; // Marks the cookie as essential for the app
});
builder.Services.AddApplication();
// Add Controllers
builder.Services.AddControllers(options =>
{
    
}); 

// Add Authorization
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.OperationFilter<FileUploadOperationFilter>();

});

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
builder.Services.AddScoped<UsersHelper>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Allow any origin (e.g., localhost, production domain, etc.)
              .AllowAnyHeader()  // Allow any header
              .AllowAnyMethod(); // Allow any HTTP method (GET, POST, PUT, DELETE)
    });
});

// Build the application
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();

        app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseCors("AllowAllOrigins");
app.UseStaticFiles();
app.UseSession();
app.UseHttpsRedirection();
app.MapControllers();
//app.UseMiddleware<MyShop.Middleware.RequestResponseLoggingMiddleware>();
//app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
/*app.UseMiddleware<CreateProductMW>()*/

app.UseAuthorization();

app.MapControllers(); // Maps controller endpoints

// Run the application
app.Run();
