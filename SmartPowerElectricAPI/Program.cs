using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartPowerElectricAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);


// Agrega servicios de autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (UsuarioController.IsTokenRevoked(token))
                {
                    context.Fail("Token inválido o revocado");
                }

                return Task.CompletedTask;
            }
        };
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUnidadMedidumRepository, UnidadMedidumRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ITipoMaterialRepository, TipoMaterialRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddDbContext<SmartPowerElectricContext>(ServiceLifetime.Scoped);

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
