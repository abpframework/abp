using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity
{
    public abstract class OrganizationUnitRepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IOrganizationUnitRepository _organizationUnitRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IdentityTestData _testData;
        private readonly IGuidGenerator _guidGenerator;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IIdentityRoleRepository _identityRoleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OrganizationUnitRepository_Tests()
        {
            _organizationUnitRepository = ServiceProvider.GetRequiredService<IOrganizationUnitRepository>();
            _lookupNormalizer = ServiceProvider.GetRequiredService<ILookupNormalizer>();
            _testData = GetRequiredService<IdentityTestData>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
            _organizationUnitManager = GetRequiredService<OrganizationUnitManager>();
            _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task GetChildrenAsync()
        {
            (await _organizationUnitRepository.GetChildrenAsync(_testData.RoleModeratorId)).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllChildrenWithParentCodeAsync()
        {
            (await _organizationUnitRepository.GetAllChildrenWithParentCodeAsync(OrganizationUnit.CreateCode(0), _guidGenerator.Create())).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var ouIds = (await _organizationUnitRepository.GetListAsync(includeDetails: true))
                        .Select(ou => ou.Id).Take(2);
            var ous = await _organizationUnitRepository.GetListAsync(ouIds);
            ous.Count.ShouldBe(2);
            ous.ShouldContain(ou => ou.Id == ouIds.First());
        }

        [Fact]
        public async Task GetOrganizationUnitAsync()
        {
            var organizationUnit = await _organizationUnitRepository.GetOrganizationUnitAsync("OU111");
            organizationUnit.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetCountAsync()
        {
            (await _organizationUnitRepository.GetCountAsync()).ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Eager_Load_OrganizationUnit_Collections()
        {
            var ou = (await _organizationUnitRepository.GetListAsync(includeDetails: true))
                .FirstOrDefault(ou => ou.DisplayName == "OU111");
            ou.Roles.ShouldNotBeNull();
            ou.Roles.Any().ShouldBeTrue();
        }
        [Fact]
        public async Task GetOrganizationUnitRolesAsync()
        {
            OrganizationUnit ou = await _organizationUnitRepository.GetOrganizationUnitAsync("OU111", true);

            var ou111Roles = await _organizationUnitRepository.GetOrganizationUnitRoles(ou.Id, true);
            ou111Roles.Count.ShouldBe(2);
            ou111Roles.ShouldContain(n => n.Name == "manager");
            ou111Roles.ShouldContain(n => n.Name == "moderator");
        }
    }
}
