using System;
using System.Linq;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.DependencyInjection;

namespace AbpDesk.ConsoleDemo
{
    public class UserLister : ITransientDependency
    {
        private readonly IdentityUserManager _userManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IQueryableRepository<IdentityUser> _userRepository;

        public UserLister(
            IdentityUserManager userManager, 
            IQueryableRepository<IdentityUser> userRepository, 
            IUnitOfWorkManager unitOfWorkManager,
            IGuidGenerator guidGenerator)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _guidGenerator = guidGenerator;
        }

        public void List()
        {
            Console.WriteLine();
            Console.WriteLine("List of users:");

            using (var uow = _unitOfWorkManager.Begin())
            {
                //TODO: Create IdentityUser by a factory or manager to ensure requirements (like unique username) or just use UserManager.Create here?
                //_userRepository.Insert(new IdentityUser(_guidGenerator.Create(), "tugrul"), true);

                foreach (var user in _userRepository.ToList())
                {
                    Console.WriteLine("# " + user);

                    foreach (var roleName in AsyncHelper.RunSync(() => _userManager.GetRolesAsync(user)))
                    {
                        Console.WriteLine("  - " + roleName);
                    }
                }

                AsyncHelper.RunSync(() => uow.CompleteAsync());
            }
        }
    }
}