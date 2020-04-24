using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.TestServices
{
    [Authorize(Roles = "MyRole")]
    public class MyAuthorizedServiceWithRole : IMyAuthorizedServiceWithRole, ITransientDependency
    {
        public virtual Task<int> ProtectedByRole()
        {
            return Task.FromResult(42);
        }

        [Authorize(Roles = "MyAnotherRole")]
        public virtual Task<int> ProtectedByAnotherRole()
        {
            return Task.FromResult(42);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        public virtual Task<int> ProtectedByScheme()
        {
            return Task.FromResult(42);
        }
    }
}
