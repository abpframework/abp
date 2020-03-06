using Microsoft.AspNetCore.Identity;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity
{
    public class RoleChangingEvents_Test : AbpIdentityDomainTestBase
    {
        protected readonly IIdentityRoleRepository RoleRepository;
        protected readonly IPermissionGrantRepository PermissionGrantRepository;
        protected readonly IdentityRoleManager RoleManager;
        protected readonly ILookupNormalizer LookupNormalizer;
        protected readonly IGuidGenerator GuidGenerator;
        protected readonly IUnitOfWorkManager UowManager;

        public RoleChangingEvents_Test()
        {
            RoleRepository = GetRequiredService<IIdentityRoleRepository>(); ;
            PermissionGrantRepository = GetRequiredService<IPermissionGrantRepository>(); ;
            RoleManager = GetRequiredService<IdentityRoleManager>(); ;
            LookupNormalizer = GetRequiredService<ILookupNormalizer>(); ;
            GuidGenerator = GetRequiredService<IGuidGenerator>();
            UowManager = GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Role_Update_Event_Test()
        {
            var role = await RoleRepository
                                .FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("moderator"))
                                ;

            var permissionGrantsInRole = await PermissionGrantRepository.GetListAsync("R", role.Name);
            permissionGrantsInRole.ShouldNotBeNull();
            permissionGrantsInRole.Count.ShouldBeGreaterThan(0);
            var count = permissionGrantsInRole.Count;

            using (var uow = UowManager.Begin())
            {
                var identityResult = await RoleManager.SetRoleNameAsync(role, "TestModerator");
                identityResult.Succeeded.ShouldBeTrue();
                var xx = await RoleRepository.UpdateAsync(role);
                await uow.CompleteAsync();
            }

            role = await RoleRepository.GetAsync(role.Id);
            role.Name.ShouldBe("TestModerator");

            permissionGrantsInRole = await PermissionGrantRepository.GetListAsync("R", role.Name);
            permissionGrantsInRole.Count.ShouldBe(count);
        }
    }
}
