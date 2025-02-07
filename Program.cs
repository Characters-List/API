using System.Security.Claims;
using CharactersList.Configuration.Auth;
using CharactersList.Configuration.Database;
using CharactersList.Models.Database;
using CharactersList.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

string allowedOrigins = "AllowedSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            allowedOrigins,
            policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:4200",
                        "https://localhost:4200"
                    )
                    .AllowCredentials()
                    .AllowAnyHeader();
            }
        );
    }
);

// Add services to the container.
builder.Services.Configure<CharactersListDatabaseSettings>(
    builder.Configuration.GetSection("CharactersListDatabaseSettings")
);

builder.Services.AddSingleton<DatabaseService<Character>>();
builder.Services.AddSingleton<DatabaseService<CharacterClass>>();

builder
    .Services
    .AddControllers();

builder.Services.AddOpenApi();
builder
    .Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy(
            "read:user",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("read:user", builder.Configuration["Auth0:Domain"]!)
            )
        );
    });

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(allowedOrigins);

app.Run();
