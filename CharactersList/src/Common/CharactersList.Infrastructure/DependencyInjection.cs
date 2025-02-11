using CharactersList.Domain.Entities;
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
		services.AddSingleton<IMongoClient>(_ =>
			new MongoClient(
				configuration.GetSection("CharactersListDatabaseSettings:ConnectionString").Value
			)
		);
		services.AddSingleton<IMongoDatabase>(
			provider => provider
				.GetService<IMongoClient>()!
				.GetDatabase(
					configuration.GetSection("CharactersListDatabaseSettings:DatabaseName").Value
				)
		);

		services.AddScoped<IEntityContext<Character>, CharacterContext>();
		services.AddScoped<IEntityContext<CharacterClass>, CharacterClassContext>();

		return services;
	}
}