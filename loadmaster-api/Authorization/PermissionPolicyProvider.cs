#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace loadmaster_api.Authorization
{
    // A simple policy provider that understands policies named "Permission:<name>"
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "Permission:";
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallbackPolicyProvider.GetDefaultPolicyAsync()!;

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync()!;

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX))
            {
                var permission = policyName.Substring(POLICY_PREFIX.Length);
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(permission))
                    .RequireAuthenticatedUser()
                    .Build();
                return Task.FromResult(policy);
            }

            return _fallbackPolicyProvider.GetPolicyAsync(policyName)!;
        }
    }
}
