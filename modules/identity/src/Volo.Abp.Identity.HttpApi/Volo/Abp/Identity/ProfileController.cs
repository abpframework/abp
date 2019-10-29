using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity
{
    [RemoteService]
    [Area("identity")]
    [ControllerName("Profile")]
    [Route("/api/identity/my-profile")]
    public class ProfileController : AbpController, IProfileAppService
    {
        private readonly IProfileAppService _profileAppService;

        public ProfileController(IProfileAppService profileAppService)
        {
            _profileAppService = profileAppService;
        }

        [HttpGet]
        public Task<ProfileDto> GetAsync()
        {
            return _profileAppService.GetAsync();
        }

        [HttpPut]
        public Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
        {
            return _profileAppService.UpdateAsync(input);
        }

        [HttpPost]
        [Route("change-password")]
        public Task ChangePasswordAsync(ChangePasswordInput input)
        {
            return _profileAppService.ChangePasswordAsync(input);
        }
    }
}
