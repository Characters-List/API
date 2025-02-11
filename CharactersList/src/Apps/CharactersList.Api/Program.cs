using System.Security.Claims;
using Asp.Versioning;
using CharactersList.Api.Configuration.Auth;
using CharactersList.Business;
using CharactersList.Infrastructure;
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
					.AllowAnyHeader()
					.AllowAnyMethod();
			}
		);
	}
);

builder
	.Services
	.AddControllers();

builder.Services.AddOpenApi();
builder
	.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.Authority = builder.Configuration["Auth0:Domain"];
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
			"user",
			policy => policy.Requirements.Add(
				new HasPermissionRequirement("user", builder.Configuration["Auth0:Domain"]!)
			)
		);
		options.AddPolicy(
			"admin",
			policy => policy.Requirements.Add(
				new HasPermissionRequirement("admin", builder.Configuration["Auth0:Domain"]!)
			)
		);
	});

builder.Services.AddApiVersioning(options =>
	{
		options.DefaultApiVersion = new ApiVersion(1);
		options.ReportApiVersions = true;
		options.AssumeDefaultVersionWhenUnspecified = true;
		options.ApiVersionReader = ApiVersionReader.Combine(
			new UrlSegmentApiVersionReader(),
			new HeaderApiVersionReader("X-Api-Version"));
	})
	.AddMvc()
	.AddApiExplorer(options =>
	{
		options.GroupNameFormat = "'v'V";
		options.SubstituteApiVersionInUrl = true;
	});

builder.Services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();
builder.Services.AddInfrastructure(builder.Configuration).AddBusiness();

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