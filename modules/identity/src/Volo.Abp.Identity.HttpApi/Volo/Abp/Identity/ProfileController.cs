using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity
{
    [RemoteService]
    [Area("identity")]
    [ControllerName("Profile")]
    [Route("/api/identity/profiles")]
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
        [Route("ChangePassword")]
        public Task ChangePasswordAsync(string currentPassword, string newPassword)
        {
            return _profileAppService.ChangePasswordAsync(currentPassword, newPassword);
        }
    }
}
