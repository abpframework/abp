using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity.Organizations;

namespace Volo.Abp.Identity
{
    public class AbpIdentityTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IOrganizationUnitRepository _organizationUnitRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IdentityTestData _testData;
        private readonly OrganizationUnitManager _organizationUnitManager;

        private IdentityRole _adminRole;
        private IdentityRole _moderator;
        private IdentityRole _supporterRole;
        private OrganizationUnit _ou111;
        private OrganizationUnit _ou112;

        public AbpIdentityTestDataBuilder(
            IGuidGenerator guidGenerator,
            IIdentityUserRepository userRepository,
            IIdentityClaimTypeRepository identityClaimTypeRepository,
            IIdentityRoleRepository roleRepository,
            IOrganizationUnitRepository organizationUnitRepository,
            ILookupNormalizer lookupNormalizer,
            IdentityTestData testData,
            OrganizationUnitManager organizationUnitManager)
        {
            _guidGenerator = guidGenerator;
            _userRepository = userRepository;
            _identityClaimTypeRepository = identityClaimTypeRepository;
            _roleRepository = roleRepository;
            _lookupNormalizer = lookupNormalizer;
            _testData = testData;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
        }

        public async Task Build()
        {
            await AddRoles().ConfigureAwait(false);
            await AddOrganizationUnits().ConfigureAwait(false);
            await AddUsers().ConfigureAwait(false);
            await AddClaimTypes().ConfigureAwait(false);
        }

        private async Task AddRoles()
        {
            _adminRole = await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin")).ConfigureAwait(false);

            _moderator = new IdentityRole(_testData.RoleModeratorId, "moderator");
            _moderator.AddClaim(_guidGenerator, new Claim("test-claim", "test-value"));
            await _roleRepository.InsertAsync(_moderator).ConfigureAwait(false);

            _supporterRole = new IdentityRole(_guidGenerator.Create(), "supporter");
            await _roleRepository.InsertAsync(_supporterRole).ConfigureAwait(false);
        }

        /* Creates OU tree as shown below:
         * 
         * - OU1
         *   - OU11
         *     - OU111
         *     - OU112
         *   - OU12
         * - OU2
         *   - OU21
         */
        private async Task AddOrganizationUnits()
        {
            var ou1 = await CreateOU("OU1", OrganizationUnit.CreateCode(1)).ConfigureAwait(false);
            var ou11 = await CreateOU("OU11", OrganizationUnit.CreateCode(1, 1), ou1.Id).ConfigureAwait(false);
            _ou112 = await CreateOU("OU112", OrganizationUnit.CreateCode(1, 1, 2), ou11.Id).ConfigureAwait(false);
            var ou12 = await CreateOU("OU12", OrganizationUnit.CreateCode(1, 2), ou1.Id).ConfigureAwait(false);
            var ou2 = await CreateOU("OU2", OrganizationUnit.CreateCode(2)).ConfigureAwait(false);
            var ou21 = await CreateOU("OU21", OrganizationUnit.CreateCode(2, 1), ou2.Id).ConfigureAwait(false);

            _ou111 = new OrganizationUnit(null, "OU111", ou11.Id);
            _ou111.Code = OrganizationUnit.CreateCode(1, 1, 1);
            _ou111.AddRole(_moderator.Id);
            await _organizationUnitRepository.InsertAsync(_ou111).ConfigureAwait(false);
        }

        private async Task AddUsers()
        {
            var adminUser = new IdentityUser(_guidGenerator.Create(), "administrator", "admin@abp.io");
            adminUser.AddRole(_adminRole.Id);
            adminUser.AddClaim(_guidGenerator, new Claim("TestClaimType", "42"));
            await _userRepository.InsertAsync(adminUser).ConfigureAwait(false);

            var john = new IdentityUser(_testData.UserJohnId, "john.nash", "john.nash@abp.io");
            john.AddRole(_moderator.Id);
            john.AddRole(_supporterRole.Id);
            john.AddOrganizationUnit(_ou111.Id);
            john.AddOrganizationUnit(_ou112.Id);
            john.AddLogin(new UserLoginInfo("github", "john", "John Nash"));
            john.AddLogin(new UserLoginInfo("twitter", "johnx", "John Nash"));
            john.AddClaim(_guidGenerator, new Claim("TestClaimType", "42"));
            john.SetToken("test-provider", "test-name", "test-value");
            await _userRepository.InsertAsync(john).ConfigureAwait(false);

            var david = new IdentityUser(_testData.UserDavidId, "david", "david@abp.io");
            await _userRepository.InsertAsync(david).ConfigureAwait(false);

            var neo = new IdentityUser(_testData.UserNeoId, "neo", "neo@abp.io");
            neo.AddRole(_supporterRole.Id);
            neo.AddClaim(_guidGenerator, new Claim("TestClaimType", "43"));
            await _userRepository.InsertAsync(neo).ConfigureAwait(false);
        }


        private async Task AddClaimTypes()
        {
            var ageClaim = new IdentityClaimType(_testData.AgeClaimId, "Age", false, false, null, null, null, IdentityClaimValueType.Int);
            await _identityClaimTypeRepository.InsertAsync(ageClaim).ConfigureAwait(false);
            var educationClaim = new IdentityClaimType(_testData.EducationClaimId, "Education", true, false, null, null, null);
            await _identityClaimTypeRepository.InsertAsync(educationClaim).ConfigureAwait(false);
        }

        private async Task<OrganizationUnit> CreateOU(string displayName, string code, Guid? parentId = null)
        {
            var ou = await _organizationUnitRepository.InsertAsync(new OrganizationUnit(null, displayName, parentId) { Code = code }).ConfigureAwait(false);
            return ou;
        }
    }
}