using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services.Dtos;

namespace Volo.Abp.Identity
{
    //TODO: Consider a way of passing cancellation token to all async application service methods!

    public class UserAppService : IUserAppService
    {
        private readonly IIdentityUserRepository _userRepository;

        public UserAppService(IIdentityUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ListResultDto<IdentityUserDto>> GetAll()
        {
            var users = (await _userRepository.GetListAsync())
                .Select(u => new IdentityUserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName
                })
                .ToList();

            return new ListResultDto<IdentityUserDto>(users);
        }

        public async Task<IdentityUserDto> Get(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            return new IdentityUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
