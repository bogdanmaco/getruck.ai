using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace loadmaster_api.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                return Task.CompletedTask;
            }

            // permissions are stored as claim "permissions" with comma-separated values
            var perms = context.User.FindFirst("permissions")?.Value;
            if (string.IsNullOrEmpty(perms)) return Task.CompletedTask;

            var entries = perms.Split(',', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);
            if (entries.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
