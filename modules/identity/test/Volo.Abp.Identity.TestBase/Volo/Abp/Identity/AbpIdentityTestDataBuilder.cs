using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Identity
{
    public class AbpIdentityTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IdentityTestData _testData;

        private IdentityRole _adminRole;
        private IdentityRole _moderator;
        private IdentityRole _supporterRole;

        public AbpIdentityTestDataBuilder(
            IGuidGenerator guidGenerator,
            IIdentityUserRepository userRepository,
            IIdentityClaimTypeRepository identityClaimTypeRepository,
            IIdentityRoleRepository roleRepository,
            ILookupNormalizer lookupNormalizer,
            IdentityTestData testData)
        {
            _guidGenerator = guidGenerator;
            _userRepository = userRepository;
            _identityClaimTypeRepository = identityClaimTypeRepository;
            _roleRepository = roleRepository;
            _lookupNormalizer = lookupNormalizer;
            _testData = testData;
        }

        public void Build()
        {
            AddRoles();
            AddUsers();
            AddClaimTypes();
        }

        private void AddRoles()
        {
            _adminRole = _roleRepository.FindByNormalizedName(_lookupNormalizer.NormalizeName("admin"));

            _moderator = new IdentityRole(_testData.RoleModeratorId, "moderator");
            _moderator.AddClaim(_guidGenerator, new Claim("test-claim", "test-value"));
            _roleRepository.Insert(_moderator);

            _supporterRole = new IdentityRole(_guidGenerator.Create(), "supporter");
            _roleRepository.Insert(_supporterRole);
        }

        private void AddUsers()
        {
            var adminUser = new IdentityUser(_guidGenerator.Create(), "administrator", "admin@abp.io");
            adminUser.AddRole(_adminRole.Id);
            adminUser.AddClaim(_guidGenerator, new Claim("TestClaimType", "42"));
            _userRepository.Insert(adminUser);

            var john = new IdentityUser(_testData.UserJohnId, "john.nash", "john.nash@abp.io");
            john.AddRole(_moderator.Id);
            john.AddRole(_supporterRole.Id);
            john.AddLogin(new UserLoginInfo("github", "john", "John Nash"));
            john.AddLogin(new UserLoginInfo("twitter", "johnx", "John Nash"));
            john.AddClaim(_guidGenerator, new Claim("TestClaimType", "42"));
            john.SetToken("test-provider", "test-name", "test-value");
            _userRepository.Insert(john);

            var david = new IdentityUser(_testData.UserDavidId, "david", "david@abp.io");
            _userRepository.Insert(david);

            var neo = new IdentityUser(_testData.UserNeoId, "neo", "neo@abp.io");
            neo.AddRole(_supporterRole.Id);
            neo.AddClaim(_guidGenerator, new Claim("TestClaimType", "43"));
            _userRepository.Insert(neo);
        }

        private void AddClaimTypes()
        {
            var ageClaim = new IdentityClaimType(_testData.AgeClaimId, "Age", false, false, null, null, null,IdentityClaimValueType.Int);
            _identityClaimTypeRepository.Insert(ageClaim);
            var educationClaim = new IdentityClaimType(_testData.EducationClaimId, "Education", true, false, null, null, null);
            _identityClaimTypeRepository.Insert(educationClaim);
        }
    }
}