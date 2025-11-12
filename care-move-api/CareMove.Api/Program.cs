using CareMove.Domain;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service;
using CareMove.Domain.Interface.Service.Service;
using CareMove.Domain.Service;
using CareMove.Domain.Service.Service;
using CareMove.Infrastructure.Context;
using CareMove.Infrastructure.Repository.Repository;
// --- Imports Adicionadas para JWT ---
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
// --- Fim Imports JWT ---

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticação JWT (Bearer). " +
                      "Insira 'Bearer' [espaço] e depois o seu token. " +
                      "Exemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JwtSettings:SecretKey não está configurada no appsettings.json...");
}
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
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36)) // versão do MySQL instalado
    )
);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITransportAssignmentRepository, TransportAssignmentRepository>();
builder.Services.AddTransient<ITransportRequestRepository, TransportRequestRepository>();
builder.Services.AddTransient<IVehicleRepository, VehicleRepository>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITransportAssignmentService, TransportAssignmentService>();
builder.Services.AddTransient<ITransportRequestService, TransportRequestService>();
builder.Services.AddTransient<IVehicleService, VehicleService>();
builder.Services.AddTransient<IAuthenticateService, AuthenticateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();