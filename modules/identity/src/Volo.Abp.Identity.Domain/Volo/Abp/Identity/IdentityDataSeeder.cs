﻿using System;
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
        public virtual async Task<IdentityDataSeedResult> SeedAsync(
            string adminEmail,
            string adminPassword,
            Guid? tenantId = null)
        {
            Check.NotNullOrWhiteSpace(adminEmail, nameof(adminEmail));
            Check.NotNullOrWhiteSpace(adminPassword, nameof(adminPassword));

            var result = new IdentityDataSeedResult();

            //"admin" user
            const string adminUserName = "admin";
            var adminUser = await _userRepository.FindByNormalizedUserNameAsync(
                _lookupNormalizer.NormalizeName(adminUserName)
            ).ConfigureAwait(false);

            if (adminUser != null)
            {
                return result;
            }

            adminUser = new IdentityUser(
                _guidGenerator.Create(),
                adminUserName,
                adminEmail,
                tenantId
            )
            {
                Name = adminUserName
            };

            (await _userManager.CreateAsync(adminUser, adminPassword).ConfigureAwait(false)).CheckErrors();
            result.CreatedAdminUser = true;

            //"admin" role
            const string adminRoleName = "admin";
            var adminRole = await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName(adminRoleName)).ConfigureAwait(false);
            if (adminRole == null)
            {
                adminRole = new IdentityRole(
                    _guidGenerator.Create(),
                    adminRoleName,
                    tenantId
                )
                {
                    IsStatic = true,
                    IsPublic = true
                };

                (await _roleManager.CreateAsync(adminRole).ConfigureAwait(false)).CheckErrors();
                result.CreatedAdminRole = true;
            }

            (await _userManager.AddToRoleAsync(adminUser, adminRoleName).ConfigureAwait(false)).CheckErrors();

            return result;
        }
    }
}
