using Microsoft.AspNetCore.Authorization;

namespace CharactersList.Api.Configuration.Auth;

public class HasPermissionHandler: AuthorizationHandler<HasPermissionRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
	{
		if (!context.User.HasClaim(c => c.Type == "permissions" && c.Issuer == requirement.Issuer))
			return Task.CompletedTask;

		IEnumerable<string> permissions = context.User
			.FindAll(c => c.Type == "permissions" && c.Issuer == requirement.Issuer).Select(c => c.Value);

		if (permissions.Any(p => p == requirement.Permission))
			context.Succeed(requirement);

		return Task.CompletedTask;
	}
}