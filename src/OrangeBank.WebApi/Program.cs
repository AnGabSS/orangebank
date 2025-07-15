using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrangeBank.Application.Services;
using OrangeBank.Core.Domain.Interfaces;
using OrangeBank.Core.Domain.Security;
using OrangeBank.Infrastructure.Data;
using OrangeBank.Infrastructure.Repositories;
using OrangeBank.Infrastructure.Security;
using System.Text;

Env.Load("../../.env");

var builder = WebApplication.CreateBuilder(args);

// Configuração do MySQL
builder.Services.AddDbContext<OrangeBankContext>(options =>
    options.UseMySql(
        $"server={Env.GetString("DB_HOST")};" +
        $"port={Env.GetString("DB_PORT")};" +
        $"database={Env.GetString("DB_NAME")};" +
        $"user={Env.GetString("DB_USER")};" +
        $"password={Env.GetString("DB_PASSWORD")};",
        ServerVersion.AutoDetect($"server={Env.GetString("DB_HOST")};" +
                               $"port={Env.GetString("DB_PORT")};" +
                               $"user={Env.GetString("DB_USER")};" +
                               $"password={Env.GetString("DB_PASSWORD")};")
    ));

// Configuração JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET"))),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        // Configuração dos eventos (corrigido)
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Token inválido: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado com sucesso");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICheckingAccountRepository, CheckingAccountRepository>();
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CheckingAccountService>();
builder.Services.AddControllers();


// Configuração do OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Rotas (exemplo mantido)
//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

app.MapGet("/test", () => { return "API FUNCIONANDO"; });
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}