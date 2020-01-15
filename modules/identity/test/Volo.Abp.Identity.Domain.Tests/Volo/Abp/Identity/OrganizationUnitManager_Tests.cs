using Microsoft.AspNetCore.Identity;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity.Organizations;
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
        public OrganizationUnitManager_Tests()
        {
            _organizationUnitManager = GetRequiredService<OrganizationUnitManager>();
            _organizationUnitRepository = GetRequiredService<IOrganizationUnitRepository>();
            _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
            _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
            _testData = GetRequiredService<IdentityTestData>();
        }

        [Fact]
        public async Task CreateAsnyc()
        {
            await _organizationUnitManager.CreateAsync(new OrganizationUnit(null, "Root 1"));

            var root1 = await _organizationUnitRepository.GetOrganizationUnitAsync("Root 1").ConfigureAwait(false);
            root1.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var ou = await _organizationUnitRepository.GetOrganizationUnitAsync("OU111").ConfigureAwait(false);
            ou.Code = OrganizationUnit.CreateCode(123);
            await _organizationUnitManager.UpdateAsync(ou);

            var ouAfterChange = await _organizationUnitRepository.GetOrganizationUnitAsync("OU111").ConfigureAwait(false);
            ouAfterChange.Code.ShouldContain("123");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var ou = await _organizationUnitRepository.GetOrganizationUnitAsync("OU11").ConfigureAwait(false);
            await _organizationUnitManager.DeleteAsync(ou.Id);

            (await _organizationUnitRepository.GetOrganizationUnitAsync("OU11").ConfigureAwait(false)).ShouldBeNull();
        }

        [Fact]
        public async Task MoveAsync()
        {
            var ou1 = await _organizationUnitRepository.GetOrganizationUnitAsync("OU1").ConfigureAwait(false);
            var ou2 = await _organizationUnitRepository.GetOrganizationUnitAsync("OU2").ConfigureAwait(false);

            await _organizationUnitManager.MoveAsync(ou1.Id, ou2.Id);

            ou1 = await _organizationUnitRepository.GetOrganizationUnitAsync("OU1").ConfigureAwait(false);
            ou1.ParentId.ShouldBe(ou2.Id);
            ou1.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2));

            var ou11 = await _organizationUnitRepository.GetOrganizationUnitAsync("OU11").ConfigureAwait(false);
            ou11.ParentId.ShouldBe(ou1.Id);
            ou11.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2, 1));

            var ou111 = await _organizationUnitRepository.GetOrganizationUnitAsync("OU111").ConfigureAwait(false);
            ou111.ParentId.ShouldBe(ou11.Id);
            ou111.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2, 1, 1));

            var ou112 = await _organizationUnitRepository.GetOrganizationUnitAsync("OU112").ConfigureAwait(false);
            ou112.ParentId.ShouldBe(ou11.Id);
            ou112.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2, 1, 2));

            var ou12 = await _organizationUnitRepository.GetOrganizationUnitAsync("OU12").ConfigureAwait(false);
            ou12.ParentId.ShouldBe(ou1.Id);
            ou12.Code.ShouldBe(OrganizationUnit.CreateCode(2, 2, 2));
        }

        [Fact]
        public async Task AddRoleToOrganizationUnitAsync()
        {
            var ou = await _organizationUnitRepository.GetOrganizationUnitAsync("OU1", true).ConfigureAwait(false);
            var adminRole = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin")).ConfigureAwait(false);
            await _organizationUnitManager.AddRoleToOrganizationUnitAsync(adminRole, ou);
            
            //TODO: This method has a bug: add role not work
            ou = await _organizationUnitRepository.GetOrganizationUnitAsync("OU1", includeDetails: true).ConfigureAwait(false);
            ou.Roles.FirstOrDefault().RoleId.ShouldBe(adminRole.Id);
        }

        [Fact]
        public async Task RemoveRoleFromOrganizationUnitAsync()
        {
            var ou = await _organizationUnitRepository.GetOrganizationUnitAsync("OU1", true).ConfigureAwait(false);
            var adminRole = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin")).ConfigureAwait(false);
            await _organizationUnitManager.AddRoleToOrganizationUnitAsync(adminRole.Id, ou.Id);

            await _organizationUnitManager.RemoveRoleFromOrganizationUnitAsync(adminRole.Id, ou.Id);
            ou = await _organizationUnitRepository.GetOrganizationUnitAsync("OU1", includeDetails: true).ConfigureAwait(false);
            ou.Roles.FirstOrDefault(r => r.RoleId == adminRole.Id).ShouldBeNull();
        }
    }
}
