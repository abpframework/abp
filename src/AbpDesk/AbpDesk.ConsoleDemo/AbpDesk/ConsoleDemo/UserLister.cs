using System;
using System.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace AbpDesk.ConsoleDemo
{
    public class UserLister : ITransientDependency
    {
        private readonly IdentityUserManager _userManager;
        private readonly IRepository<IdentityUser, Guid> _userRepository;

        public UserLister(
            IdentityUserManager userManager,
            IRepository<IdentityUser, Guid> userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [UnitOfWork]
        public virtual void List()
        {
            Console.WriteLine();
            Console.WriteLine("List of users:");

            foreach (var user in _userRepository.ToList())
            {
                Console.WriteLine("# " + user);

                foreach (var roleName in AsyncHelper.RunSync(() => _userManager.GetRolesAsync(user)))
                {
                    Console.WriteLine("  - " + roleName);
                }
            }
        }
    }
}