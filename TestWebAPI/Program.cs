using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Endpoints;
using TestWebAPI.Mappers;

var builder = WebApplication.CreateBuilder(args);

var dataBaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(x => x.UseNpgsql(dataBaseConnectionString));
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

builder.Configuration.GetConnectionString("DefaultConnection");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAppEndpoints();

app.Run();