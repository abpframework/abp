using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
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
        private readonly IIdentityUserRepository _identityUserRepository;

        public OrganizationUnitRepository_Tests()
        {
            _organizationUnitRepository = ServiceProvider.GetRequiredService<IOrganizationUnitRepository>();
            _lookupNormalizer = ServiceProvider.GetRequiredService<ILookupNormalizer>();
            _testData = GetRequiredService<IdentityTestData>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
            _organizationUnitManager = GetRequiredService<OrganizationUnitManager>();
            _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
            _identityUserRepository = GetRequiredService<IIdentityUserRepository>();
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
            var organizationUnit = await _organizationUnitRepository.GetAsync("OU111");
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
            OrganizationUnit ou = await _organizationUnitRepository.GetAsync("OU111", includeDetails: true);

            var ou111Roles = await _organizationUnitRepository.GetRolesAsync(ou, includeDetails: true);
            ou111Roles.Count.ShouldBe(2);
            ou111Roles.ShouldContain(n => n.Name == "manager");
            ou111Roles.ShouldContain(n => n.Name == "moderator");
        }

        [Fact]
        public async Task GetOrganizationUnitRolesWithPagingAsync()
        {
            OrganizationUnit ou = await _organizationUnitRepository.GetAsync("OU111", includeDetails: true);

            var ou111Roles = await _organizationUnitRepository.GetRolesAsync(ou, sorting: "name desc", maxResultCount: 1, includeDetails: true);
            ou111Roles.Count.ShouldBe(1);
            ou111Roles.ShouldContain(n => n.Name == "moderator");
        }

        [Fact]
        public async Task GetMembersInOrganizationUnitListAsync()
        {
            OrganizationUnit ou1 = await _organizationUnitRepository.GetAsync("OU111", true);
            OrganizationUnit ou2 = await _organizationUnitRepository.GetAsync("OU112", true);
            var users = await _identityUserRepository.GetUsersInOrganizationsListAsync(new List<Guid> { ou1.Id, ou2.Id });
            users.Count.ShouldBeGreaterThan(0);
        }
        [Fact]
        public async Task GetMembersInOrganizationUnitWithParamsAsync()
        {
            OrganizationUnit ou = await _organizationUnitRepository.GetAsync("OU111", true);
            var users = await _organizationUnitRepository.GetMembersAsync(ou, "UserName DESC", 5, 0, "n");

            users.Count.ShouldBeGreaterThan(1);
            users.Count.ShouldBeLessThanOrEqualTo(5);

            //Filter check
            users.ShouldAllBe(u => u.UserName.Contains("ne") || u.Email.Contains("n"));

            //Order check
            for (var i = 0; i < users.Count - 1; i++)
            {
                string.Compare(
                    users[i].UserName,
                    users[i + 1].UserName,
                    StringComparison.OrdinalIgnoreCase
                ).ShouldBeGreaterThan(0);
            }

            users = await _organizationUnitRepository.GetMembersAsync(ou, null, 999, 0, "undefined-username");
            users.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GetMembersCountOfOrganizationUnit()
        {
            OrganizationUnit ou = await _organizationUnitRepository.GetAsync("OU111", true);
            var usersCount = await _organizationUnitRepository.GetMembersCountAsync(ou);

            usersCount.ShouldBeGreaterThan(1);
        }

        [Fact]
        public async Task GetRolesCountOfOrganizationUnit()
        {
            OrganizationUnit ou = await _organizationUnitRepository.GetAsync("OU111", true);
            var rolesCount = await _organizationUnitRepository.GetRolesCountAsync(ou);

            rolesCount.ShouldBeGreaterThan(1);
        }

        [Fact]
        public async Task RemoveAllMembersOfOrganizationUnit()
        {
            OrganizationUnit ou = await _organizationUnitRepository.GetAsync("OU111", true);
            var membersCount = await _organizationUnitRepository.GetMembersCountAsync(ou);
            membersCount.ShouldBeGreaterThan(1);

            await _organizationUnitRepository.RemoveAllMembersAsync(ou);
            var newCount = await _organizationUnitRepository.GetMembersCountAsync(ou);
            newCount.ShouldBe(0);
        }

        [Fact]
        public async Task RemoveAllRolesOfOrganizationUnit()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                OrganizationUnit ou = await _organizationUnitRepository.GetAsync("OU111", true);
                var rolesCount = await _organizationUnitRepository.GetRolesCountAsync(ou);
                rolesCount.ShouldBeGreaterThan(1);

                await _organizationUnitRepository.RemoveAllRolesAsync(ou);

                await uow.SaveChangesAsync();

                var newCount = await _organizationUnitRepository.GetRolesCountAsync(ou);
                newCount.ShouldBe(0);

                await uow.CompleteAsync();
            }
        }
    }
}
