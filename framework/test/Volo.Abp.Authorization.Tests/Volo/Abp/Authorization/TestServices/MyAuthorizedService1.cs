using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.TestServices
{
    [Authorize("MyAuthorizedService1")]
    public class MyAuthorizedService1 : IMyAuthorizedService1, ITransientDependency
    {
        [AllowAnonymous]
        public virtual int Anonymous()
        {
            return 42;
        }

        [AllowAnonymous]
        public virtual async Task<int> AnonymousAsync()
        {
            await Task.Delay(10);
            return 42;
        }

        public virtual int ProtectedByClass()
        {
            return 42;
        }

        public virtual async Task<int> ProtectedByClassAsync()
        {
            await Task.Delay(10);
            return 42;
        }
    }
}