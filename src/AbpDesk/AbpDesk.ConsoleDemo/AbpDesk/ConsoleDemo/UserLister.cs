using System;
using System.Linq;
using Volo.Abp.Domain.Repositories;
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
        private readonly IQueryableRepository<IdentityUser> _userRepository;

        public UserLister(
            IdentityUserManager userManager, 
            IQueryableRepository<IdentityUser> userRepository, 
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void List()
        {
            Console.WriteLine();
            Console.WriteLine("List of users:");

            using (var uow = _unitOfWorkManager.Begin())
            {
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