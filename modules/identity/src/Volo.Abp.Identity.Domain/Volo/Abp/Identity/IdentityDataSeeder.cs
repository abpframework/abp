using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity
{
    public class IdentityDataSeeder : ITransientDependency, IIdentityDataSeeder
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;

        public IdentityDataSeeder(
            IGuidGenerator guidGenerator,
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            ILookupNormalizer lookupNormalizer,
            IdentityUserManager userManager,
            IdentityRoleManager roleManager)
        {
            _guidGenerator = guidGenerator;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _lookupNormalizer = lookupNormalizer;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(
            string adminUserPassword,
            Guid? tenantId = null)
        {
            const string adminUserName = "admin";
            const string adminRoleName = "admin";

            //"admin" user
            var adminUser = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.Normalize(adminUserName));
            if (adminUser != null)
            {
                return;
            }

            adminUser = new IdentityUser(_guidGenerator.Create(), adminUserName, "admin@abp.io", tenantId);
            CheckIdentityErrors(await _userManager.CreateAsync(adminUser, adminUserPassword));

            //"admin" role
            var adminRole = await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.Normalize(adminRoleName));
            if (adminRole == null)
            {
                adminRole = new IdentityRole(_guidGenerator.Create(), adminRoleName, tenantId);

                adminRole.IsStatic = true;
                adminRole.IsPublic = true;

                CheckIdentityErrors(await _roleManager.CreateAsync(adminRole));
            }

            CheckIdentityErrors(await _userManager.AddToRoleAsync(adminUser, adminRoleName));
        }



        protected void CheckIdentityErrors(IdentityResult identityResult) //TODO: This is temporary and duplicate code!
        {
            if (!identityResult.Succeeded)
            {
                //TODO: A better exception that can be shown on UI as localized?
                throw new AbpException("Operation failed: " + identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}").JoinAsString(", "));
            }

            //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
        }
    }
}
