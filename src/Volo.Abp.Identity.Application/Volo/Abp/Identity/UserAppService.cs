using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services.Dtos;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity
{
    //TODO: Consider a way of passing cancellation token to all async application service methods!

    public class UserAppService : IUserAppService
    {
        private readonly IIdentityUserRepository _userRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserAppService(
            IIdentityUserRepository userRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userRepository = userRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<ListResultDto<IdentityUserDto>> GetAll()
        {
            //Use conventional unit of work for application services when it's available!
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var users = (await _userRepository.GetListAsync())
                    .Select(u => new IdentityUserDto
                    {
                        Id = u.Id,
                        Email = u.Email,
                        UserName = u.UserName
                    })
                    .ToList();

                await unitOfWork.CompleteAsync();

                return new ListResultDto<IdentityUserDto>(users);
            }
        }

        public async Task<IdentityUserDto> Get(Guid id)
        {
            //Use conventional unit of work for application services when it's available!
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var user = await _userRepository.GetAsync(id);

                await unitOfWork.CompleteAsync();

                return new IdentityUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };
            }
        }
    }
}
