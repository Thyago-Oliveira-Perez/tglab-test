using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TgLab.Application.Auth.Services;
using TgLab.Application.User.Interfaces;
using TgLab.Application.User.Services;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Application.Wallet.Services;
using TgLab.Infrastructure.Context;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TgLab.Application.Auth.Interfaces;
using Microsoft.OpenApi.Models;
using TgLab.Application.Bet.Services;
using TgLab.Application.Bet.Interfaces;
using TgLab.Application.Game;
using TgLab.Application.Transaction.Interfaces;
using TgLab.Application.Transaction.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Services
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ICryptService, CryptService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IWalletService, WalletService>();
builder.Services.AddTransient<IBetService, BetService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddSingleton<GameService>();

builder.Services.AddHostedService<GameService>();

builder.Services.AddControllers();

// Configuração do JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TgLab API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and the JWT token."
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<TgLabContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adicione a autenticação antes da autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
