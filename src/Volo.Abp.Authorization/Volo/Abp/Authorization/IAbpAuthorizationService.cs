using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization
{
    public interface IAbpAuthorizationService : IAuthorizationService
    {
        Task CheckAsync(string policyName);
    }
}