using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Identity.Organizations;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Identity
{
    public abstract class OrganizationUnitRepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
        protected ILookupNormalizer LookupNormalizer { get; }
        private readonly IdentityTestData _testData;
        private readonly IGuidGenerator _guidGenerator;

        public OrganizationUnitRepository_Tests()
        {
            OrganizationUnitRepository = ServiceProvider.GetRequiredService<IOrganizationUnitRepository>();
            LookupNormalizer = ServiceProvider.GetRequiredService<ILookupNormalizer>();
            _testData = GetRequiredService<IdentityTestData>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
        }

        [Fact]
        public async Task GetChildrenAsync()
        {
            (await OrganizationUnitRepository.GetChildrenAsync(_testData.RoleModeratorId)).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllChildrenWithParentCodeAsync()
        {
            (await OrganizationUnitRepository.GetAllChildrenWithParentCodeAsync(OrganizationUnit.CreateCode(0), _guidGenerator.Create())).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var ouIds = (await OrganizationUnitRepository.GetListAsync(includeDetails: true))
                        .Select(ou => ou.Id).Take(2);
            var ous = await OrganizationUnitRepository.GetListAsync(ouIds);
            ous.Count.ShouldBe(2);
            ous.ShouldContain(ou => ou.Id == ouIds.First());
        }

        [Fact]
        public async Task GetOrganizationUnitAsync()
        {
            var organizationUnit = await OrganizationUnitRepository.GetOrganizationUnitAsync("OU111");
            organizationUnit.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetCountAsync()
        {
            (await OrganizationUnitRepository.GetCountAsync()).ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Eager_Load_OrganizationUnit_Collections()
        {
            var ou = (await OrganizationUnitRepository.GetListAsync(includeDetails: true))
                .FirstOrDefault(ou => ou.DisplayName == "OU111");
            ou.Roles.ShouldNotBeNull();
            ou.Roles.Any().ShouldBeTrue();
        }
    }
}
