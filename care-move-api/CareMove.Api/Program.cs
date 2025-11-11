using CareMove.Domain;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;
using CareMove.Domain.Service.Service;
using CareMove.Infrastructure.Context;
using CareMove.Infrastructure.Repository.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DataBase Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36)) // versão do MySQL instalado
    )
);

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repositories
builder.Services.AddTransient<IUserRepository, UserRepository>();

// Services
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
