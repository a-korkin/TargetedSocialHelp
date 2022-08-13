using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization;

public sealed class ClaimsRequirement : IAuthorizationRequirement { }

public class ClaimsHandler : AuthorizationHandler<ClaimsRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimsRequirement requirement)
    {
        var user = context.User.Claims.FirstOrDefault(w => w.Type == "id");
        Console.WriteLine(user!.Value);
        return Task.CompletedTask;
    }
}
