using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TwitterBook.Authorization;

public class WorksForCompanyHandler : AuthorizationHandler<WorksForCompanyRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WorksForCompanyRequirement requirement)
    {
        var emailAddress = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        if (emailAddress.EndsWith(requirement.DomainName))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        context.Fail();
        return Task.CompletedTask;
    }
}