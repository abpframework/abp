using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SecurityLog;

namespace Volo.Abp.Identity;

public class AbpIdentityTestDataBuilder : ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IdentityUserManager _userManager;
    private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;
    private readonly IIdentityRoleRepository _roleRepository;
    private readonly IOrganizationUnitRepository _organizationUnitRepository;
    private readonly IIdentitySecurityLogRepository _identitySecurityLogRepository;
    private readonly ILookupNormalizer _lookupNormalizer;
    private readonly IdentityTestData _testData;
    private readonly OrganizationUnitManager _organizationUnitManager;
    private readonly IIdentityLinkUserRepository _identityLinkUserRepository;
    private readonly IdentityLinkUserManager _identityLinkUserManager;

    private IdentityRole _adminRole;
    private IdentityRole _moderatorRole;
    private IdentityRole _supporterRole;
    private IdentityRole _managerRole;
    private OrganizationUnit _ou111;
    private OrganizationUnit _ou112;

    public AbpIdentityTestDataBuilder(
        IGuidGenerator guidGenerator,
        IIdentityUserRepository userRepository,
        IdentityUserManager userManager,
        IIdentityClaimTypeRepository identityClaimTypeRepository,
        IIdentityRoleRepository roleRepository,
        IOrganizationUnitRepository organizationUnitRepository,
        IIdentitySecurityLogRepository identitySecurityLogRepository,
        ILookupNormalizer lookupNormalizer,
        IdentityTestData testData,
        OrganizationUnitManager organizationUnitManager,
        IIdentityLinkUserRepository identityLinkUserRepository,
        IdentityLinkUserManager identityLinkUserManager)
    {
        _guidGenerator = guidGenerator;
        _userRepository = userRepository;
        _userManager = userManager;
        _identityClaimTypeRepository = identityClaimTypeRepository;
        _roleRepository = roleRepository;
        _lookupNormalizer = lookupNormalizer;
        _testData = testData;
        _organizationUnitRepository = organizationUnitRepository;
        _organizationUnitManager = organizationUnitManager;
        _identityLinkUserRepository = identityLinkUserRepository;
        _identityLinkUserManager = identityLinkUserManager;
        _identitySecurityLogRepository = identitySecurityLogRepository;
    }

    public async Task Build()
    {
        await AddRoles();
        await AddOrganizationUnits();
        await AddUsers();
        await AddLinkUsers();
        await AddClaimTypes();
        await AddSecurityLogs();
    }

    private async Task AddRoles()
    {
        _adminRole = await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin"));

        _moderatorRole = new IdentityRole(_testData.RoleModeratorId, "moderator");
        _moderatorRole.AddClaim(_guidGenerator, new Claim("test-claim", "test-value"));
        await _roleRepository.InsertAsync(_moderatorRole);

        _supporterRole = new IdentityRole(_guidGenerator.Create(), "supporter");
        await _roleRepository.InsertAsync(_supporterRole);

        _managerRole = new IdentityRole(_guidGenerator.Create(), "manager");
        await _roleRepository.InsertAsync(_managerRole);
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
        var ou1 = await CreateOU("OU1", OrganizationUnit.CreateCode(1));
        var ou11 = await CreateOU("OU11", OrganizationUnit.CreateCode(1, 1), ou1.Id);
        _ou112 = await CreateOU("OU112", OrganizationUnit.CreateCode(1, 1, 2), ou11.Id);
        var ou12 = await CreateOU("OU12", OrganizationUnit.CreateCode(1, 2), ou1.Id);
        var ou2 = await CreateOU("OU2", OrganizationUnit.CreateCode(2));
        var ou21 = await CreateOU("OU21", OrganizationUnit.CreateCode(2, 1), ou2.Id);

        _ou111 = new OrganizationUnit(_guidGenerator.Create(), "OU111", ou11.Id);
        _ou111.Code = OrganizationUnit.CreateCode(1, 1, 1);
        _ou111.AddRole(_moderatorRole.Id);
        _ou111.AddRole(_managerRole.Id);
        await _organizationUnitRepository.InsertAsync(_ou111);
    }

    private async Task AddUsers()
    {
        var adminUser = new IdentityUser(_guidGenerator.Create(), "administrator", "admin@abp.io");
        adminUser.AddRole(_adminRole.Id);
        adminUser.AddClaim(_guidGenerator, new Claim("TestClaimType", "42"));
        await _userRepository.InsertAsync(adminUser);

        var john = new IdentityUser(_testData.UserJohnId, "john.nash", "john.nash@abp.io");
        john.AddRole(_moderatorRole.Id);
        john.AddRole(_supporterRole.Id);
        john.AddOrganizationUnit(_ou111.Id);
        john.AddOrganizationUnit(_ou112.Id);
        john.AddLogin(new UserLoginInfo("github", "john", "John Nash"));
        john.AddLogin(new UserLoginInfo("twitter", "johnx", "John Nash"));
        john.AddClaim(_guidGenerator, new Claim("TestClaimType", "42"));
        john.SetToken("test-provider", "test-name", "test-value");
        await _userRepository.InsertAsync(john);

        var david = new IdentityUser(_testData.UserDavidId, "david", "david@abp.io");
        david.AddOrganizationUnit(_ou112.Id);
        await _userRepository.InsertAsync(david);

        var neo = new IdentityUser(_testData.UserNeoId, "neo", "neo@abp.io");
        neo.AddRole(_supporterRole.Id);
        neo.AddClaim(_guidGenerator, new Claim("TestClaimType", "43"));
        neo.AddOrganizationUnit(_ou111.Id);
        await _userRepository.InsertAsync(neo);

        var bob = new IdentityUser(_testData.UserBobId, "bob", "bob@abp.io");
        bob.SetIsActive(false);
        await _userManager.CreateAsync(bob, "1q2w3E*");
    }

    private async Task AddLinkUsers()
    {
        var john = await _userRepository.GetAsync(_testData.UserJohnId);
        var david = await _userRepository.GetAsync(_testData.UserDavidId);
        var neo = await _userRepository.GetAsync(_testData.UserNeoId);

        await _identityLinkUserManager.LinkAsync(new IdentityLinkUserInfo(john.Id, john.TenantId),
            new IdentityLinkUserInfo(david.Id, david.TenantId));
        await _identityLinkUserManager.LinkAsync(new IdentityLinkUserInfo(david.Id, david.TenantId),
            new IdentityLinkUserInfo(neo.Id, neo.TenantId));
    }

    private async Task AddClaimTypes()
    {
        var ageClaim = new IdentityClaimType(_testData.AgeClaimId, "Age", false, false, null, null, null, IdentityClaimValueType.Int);
        await _identityClaimTypeRepository.InsertAsync(ageClaim);

        var educationClaim = new IdentityClaimType(_testData.EducationClaimId, "Education", true, false, null, null, null);
        await _identityClaimTypeRepository.InsertAsync(educationClaim);
    }

    private async Task<OrganizationUnit> CreateOU(string displayName, string code, Guid? parentId = null)
    {
        var ou = await _organizationUnitRepository.InsertAsync(new OrganizationUnit(_guidGenerator.Create(), displayName, parentId) { Code = code });
        return ou;
    }

    private async Task AddSecurityLogs()
    {
        await _identitySecurityLogRepository.InsertAsync(new IdentitySecurityLog(_guidGenerator, new SecurityLogInfo
        {
            ApplicationName = "Test-ApplicationName",
            Identity = "Test-Identity",
            Action = "Test-Action",
            UserId = _testData.UserJohnId,
            UserName = "john.nash",

            CreationTime = new DateTime(2020, 01, 01, 10, 0, 0)
        }));

        await _identitySecurityLogRepository.InsertAsync(new IdentitySecurityLog(_guidGenerator, new SecurityLogInfo
        {
            ApplicationName = "Test-ApplicationName",
            Identity = "Test-Identity",
            Action = "Test-Action",
            UserId = _testData.UserDavidId,
            UserName = "david",

            CreationTime = new DateTime(2020, 01, 02, 10, 0, 0)
        }));
    }
}
