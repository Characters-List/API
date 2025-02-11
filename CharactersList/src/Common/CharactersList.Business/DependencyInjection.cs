using CharactersList.Business.Interfaces;
using CharactersList.Business.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CharactersList.Business;

public static class DependencyInjection
{
	public static IServiceCollection AddBusiness(this IServiceCollection services)
	{
		services.AddScoped<ICharacterRepository, CharacterRepository>();
		services.AddScoped<ICharacterClassRepository, CharacterClassRepository>();

		return services;
	}
}