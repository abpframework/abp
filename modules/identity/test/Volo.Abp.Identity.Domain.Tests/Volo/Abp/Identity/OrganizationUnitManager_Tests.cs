using Microsoft.AspNetCore.Identity;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity
{
    public class OrganizationUnitManager_Tests : AbpIdentityDomainTestBase
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IOrganizationUnitRepository _organizationUnitRepository;
        private readonly IdentityTestData _testData;
        private readonly IIdentityRoleRepository _identityRoleRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IGuidGenerator _guidGenerator;
        public OrganizationUnitManager_Tests()
        {
            _organizationUnitManager = GetRequiredService<OrganizationUnitManager>();
            _organizationUnitRepository = GetRequiredService<IOrganizationUnitRepository>();
            _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
            _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
            _testData = GetRequiredService<IdentityTestData>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
            _guidGenerator = GetService<IGuidGenerator>();
        }

        [Fact]
        public async Task CreateAsnyc()
        {
            await _organizationUnitManager.CreateAsync(new OrganizationUnit(_guidGenerator.Create(), "Root 1"));

            var root1 = await _organizationUnitRepository.GetAsync("Root 1");
            root1.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var ou = await _organizationUnitRepository.GetAsync("OU111");
            ou.DisplayName = "OU111 Updated";
            await _organizationUnitManager.UpdateAsync(ou);

            var ouAfterChange = await _organizationUnitRepository.GetAsync("OU111 Updated");
            ouAfterChange.DisplayName.ShouldContain("OU111 Updated");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var ou = await _organizationUnitRepository.GetAsync("OU11");
            await _organizationUnitManager.DeleteAsync(ou.Id);

            (await _organizationUnitRepository.GetAsync("OU11")).ShouldBeNull();
        }

        [Fact]
        public async Task MoveAsync()
        {
            var ou1 = await _organizationUnitRepository.GetAsync("OU1");
            var ou2 = await _organizationUnitRepository.GetAsync("OU2");

            await _organizationUnitManager.MoveAsync(ou1.Id, ou2.Id);

            ou1 = await _organizationUnitRepository.GetAsync("OU1");
            ou1.ParentId.ShouldBe(ou2.Id);
            ou1.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2));

            var ou11 = await _organizationUnitRepository.GetAsync("OU11");
            ou11.ParentId.ShouldBe(ou1.Id);
            ou11.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2, 1));

            var ou111 = await _organizationUnitRepository.GetAsync("OU111");
            ou111.ParentId.ShouldBe(ou11.Id);
            ou111.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2, 1, 1));

            var ou112 = await _organizationUnitRepository.GetAsync("OU112");
            ou112.ParentId.ShouldBe(ou11.Id);
            ou112.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2, 1, 2));

            var ou12 = await _organizationUnitRepository.GetAsync("OU12");
            ou12.ParentId.ShouldBe(ou1.Id);
            ou12.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2, 2));
        }

        [Fact]
        public async Task AddRoleToOrganizationUnitAsync()
        {
            OrganizationUnit ou = null;
            IdentityRole adminRole = null;

            using (var uow = _unitOfWorkManager.Begin())
            {
                ou = await _organizationUnitRepository.GetAsync("OU1", true);
                adminRole = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin"));
                await _organizationUnitManager.AddRoleToOrganizationUnitAsync(adminRole, ou);
                await _organizationUnitRepository.UpdateAsync(ou);

                await uow.CompleteAsync();
            }

            ou = await _organizationUnitRepository.GetAsync("OU1", includeDetails: true);
            ou.Roles.First().RoleId.ShouldBe(adminRole.Id);
        }

        [Fact]
        public async Task RemoveRoleFromOrganizationUnitAsync()
        {
            var ou = await _organizationUnitRepository.GetAsync("OU1", true);
            var adminRole = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin"));
            await _organizationUnitManager.AddRoleToOrganizationUnitAsync(adminRole.Id, ou.Id);

            await _organizationUnitManager.RemoveRoleFromOrganizationUnitAsync(adminRole.Id, ou.Id);
            ou = await _organizationUnitRepository.GetAsync("OU1", includeDetails: true);
            ou.Roles.FirstOrDefault(r => r.RoleId == adminRole.Id).ShouldBeNull();
        }
    }
}
