using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization;

public interface IAbpAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    Task<List<string>> GetPoliciesNamesAsync();
}
