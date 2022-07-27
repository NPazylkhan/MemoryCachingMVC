using MemoryCachingMVC.Models;
using MemoryCachingMVC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=cacheappdb;Trusted_Connection=True;";

// внедрение зависимости Entity Framework
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));

// внедрение зависимости UserService
builder.Services.AddTransient<UserService>();

// добавление кэширования
builder.Services.AddMemoryCache();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
