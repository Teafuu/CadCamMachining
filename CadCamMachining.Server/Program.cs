using AspNetCore.Identity.MongoDbCore.Infrastructure;
using CadCamMachining.Server.Hub;
using CadCamMachining.Server.Models;
using CadCamMachining.Server.Repositories;
using CadCamMachining.Server.Repositories.Interfaces;
using CadCamMachining.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var connString = Environment.GetEnvironmentVariable("MONGODB_CONNECTIONSTRING")
                 ?? builder.Configuration.GetSection("MongoDbSettings:ConnectionString").Value;
var databaseString = Environment.GetEnvironmentVariable("MONGODB_DATABASE")
                     ?? builder.Configuration.GetSection("MongoDbSettings:DatabaseName").Value;


// Configure MongoDB settings
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = connString;
    options.DatabaseName = databaseString;
});

// Register the MongoDB client
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
}); 


// Add Identity services with MongoDB
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
        connString, 
        databaseString)
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IItemTypeRepository, ItemTypeRepository>();
builder.Services.AddSingleton<IItemRepository, ItemRepository>();
builder.Services.AddSingleton<ILayoutRepository, LayoutRepository>();

builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ItemTypeService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
});

builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapRazorPages();
app.MapControllers();

app.MapHub<ItemHub>("/itemHub");
app.MapFallbackToFile("index.html");

app.MapSwagger().RequireAuthorization();

app.Run();
