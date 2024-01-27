using BLayer.Service;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ServiceGenre>();
builder.Services.AddScoped<ServiceTag>();
builder.Services.AddScoped<ServiceVideo>();
builder.Services.AddScoped<ServiceUser>();
builder.Services.AddScoped<ServiceCountry>();



builder.Services.AddDbContext<RwaMoviesContext>(options =>
{
    options.UseSqlServer("name=ConnectionStrings:RwaMoviesConnStr");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
