using BLayer.Service;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped<RwaMoviesContext>();
builder.Services.AddScoped<ServiceGenre>();
builder.Services.AddScoped<ServiceVideo>();
builder.Services.AddScoped<ServiceCountry>();
builder.Services.AddScoped<ServiceUser>();
builder.Services.AddScoped<ServiceTag>();
builder.Services.AddScoped<ServiceImage>();

builder.Services.AddDbContext<RwaMoviesContext>(options =>
{
    options.UseSqlServer("name=ConnectionStrings:RwaMoviesConnStr");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=User}/{action=Index}/{id?}");
});


app.Run();
