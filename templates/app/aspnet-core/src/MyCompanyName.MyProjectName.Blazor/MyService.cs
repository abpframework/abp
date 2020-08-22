using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace MyCompanyName.MyProjectName.Blazor
{
    public class MyProfileService : ITransientDependency
    {
        private readonly IProfileAppService _profileAppService;

        public MyProfileService(IProfileAppService profileAppService)
        {
            _profileAppService = profileAppService;
        }

        public async Task<string> GetUserNameAsync()
        {
            return (await _profileAppService.GetAsync()).UserName;
        }
    }
}
