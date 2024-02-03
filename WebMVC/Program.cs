using BLayer.Service;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<RwaMoviesContext>();
builder.Services.AddScoped<ServiceGenre>();
builder.Services.AddScoped<ServiceVideo>();
builder.Services.AddScoped<ServiceCountry>();
builder.Services.AddScoped<ServiceUser>();
builder.Services.AddScoped<ServiceTag>();


builder.Services.AddDbContext<RwaMoviesContext>(options =>
{
    options.UseSqlServer("server=.;Database=RwaMovies;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Video}/{action=Index}/{id?}");

app.Run();
