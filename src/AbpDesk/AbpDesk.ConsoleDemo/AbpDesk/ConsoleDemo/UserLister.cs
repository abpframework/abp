using System;
using System.Linq;
using Volo.Abp.Identity;
using Volo.DependencyInjection;

namespace AbpDesk.ConsoleDemo
{
    public class UserLister : ITransientDependency
    {
        private readonly IdentityUserManager _userManager;

        public UserLister(IdentityUserManager userManager)
        {
            _userManager = userManager;
        }

        public void List()
        {
            foreach (var user in _userManager.Users.ToList())
            {
                Console.WriteLine(user);
            }
        }
    }
}