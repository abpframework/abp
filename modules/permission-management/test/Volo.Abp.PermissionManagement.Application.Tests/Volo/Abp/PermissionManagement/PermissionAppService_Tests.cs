using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Xunit;

namespace Volo.Abp.PermissionManagement.Application.Tests.Volo.Abp.PermissionManagement
{
    public class PermissionAppService_Tests : AbpPermissionManagementApplicationTestBase
    {
        private readonly IPermissionAppService _permissionAppService;
        private readonly IPermissionGrantRepository _permissionGrantRepository;

        public PermissionAppService_Tests()
        {
            _permissionAppService = GetRequiredService<IPermissionAppService>();
            _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        }

        [Fact]
        public async Task GetAsync()
        {
            var permissionListResultDto = await _permissionAppService.GetAsync(UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString());

            permissionListResultDto.ShouldNotBeNull();
            permissionListResultDto.EntityDisplayName.ShouldBe(PermissionTestDataBuilder.User1Id.ToString());
            permissionListResultDto.Groups.Count.ShouldBe(1);
            permissionListResultDto.Groups.ShouldContain(x => x.Name == "TestGroup");

            permissionListResultDto.Groups.First().Permissions.Count.ShouldBe(4);
            permissionListResultDto.Groups.First().Permissions.ShouldContain(x => x.Name == "MyPermission1");
            permissionListResultDto.Groups.First().Permissions.ShouldContain(x => x.Name == "MyPermission2");
            permissionListResultDto.Groups.First().Permissions.ShouldContain(x => x.Name == "MyPermission2.ChildPermission1");
            permissionListResultDto.Groups.First().Permissions.ShouldContain(x => x.Name == "MyPermission3");

        }

        [Fact]
        public async Task UpdateAsync()
        {
            (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
                "Test")).ShouldBeNull();

            await _permissionAppService.UpdateAsync("Test",
                "Test", new UpdatePermissionsDto()
                {
                    Permissions = new UpdatePermissionDto[]
                    {
                        new UpdatePermissionDto()
                        {
                            IsGranted = true,
                            Name = "MyPermission1"
                        }
                    }
                });

            (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
                "Test")).ShouldNotBeNull();
        }

        [Fact]
        public async Task Update_Revoke_Test()
        {
            await _permissionGrantRepository.InsertAsync(
                new PermissionGrant(
                    Guid.NewGuid(),
                    "MyPermission1",
                    "Test",
                    "Test"
                )
            );
            (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
                "Test")).ShouldNotBeNull();

            await _permissionAppService.UpdateAsync("Test",
                "Test", new UpdatePermissionsDto()
                {
                    Permissions = new UpdatePermissionDto[]
                    {
                        new UpdatePermissionDto()
                        {
                            IsGranted = false,
                            Name = "MyPermission1"
                        }
                    }
                });

            (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
                "Test")).ShouldBeNull();
        }
    }
}
