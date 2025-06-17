using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Endpoints;
using TestWebAPI.Mappers;

var builder = WebApplication.CreateBuilder(args);

var dataBaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(x => x.UseNpgsql(dataBaseConnectionString));
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapIdentityApi<IdentityUser>();
app.MapAppEndpoints();
app.Run();