using CharactersList.Domain.Entities;
using CharactersList.Infrastructure.Configuration;
using CharactersList.Infrastructure.Persistence.MongoDB.Contexts;
using CharactersList.Infrastructure.Persistence.MongoDB.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CharactersList.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddSingleton<DatabaseSettings>(_ =>
				configuration.GetSection("CharactersListDatabaseSettings").Get<DatabaseSettings>()!
			)
			.AddSingleton<IMongoClient>(
				provider => new MongoClient(
						provider.GetService<DatabaseSettings>()!.ConnectionString
				)
			)
			.AddSingleton<IMongoDatabase>(
				provider => provider
					.GetService<IMongoClient>()!
					.GetDatabase(provider.GetService<DatabaseSettings>()!.DatabaseName)
			);

		services.AddScoped<IEntityContext<Character>, CharacterContext>();
		services.AddScoped<IEntityContext<CharacterClass>, CharacterClassContext>();

		return services;
	}
}