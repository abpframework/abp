using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity
{
    [Dependency(TryRegister = true)]
    public class HttpClientUserRoleFinder : IUserRoleFinder, ITransientDependency
    {
        private readonly IIdentityUserAppService _userAppService;

        public HttpClientUserRoleFinder(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task<string[]> GetRolesAsync(Guid userId)
        {
            var output = await _userAppService.GetRolesAsync(userId);
            return output.Items.Select(r => r.Name).ToArray();
        }
    }
}
