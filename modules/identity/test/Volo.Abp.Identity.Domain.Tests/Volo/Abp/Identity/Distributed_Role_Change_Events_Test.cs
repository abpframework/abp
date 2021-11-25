using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

public class Distributed_Role_Change_Events_Test : AbpIdentityDomainTestBase
{
    protected readonly IIdentityRoleRepository RoleRepository;
    protected readonly IPermissionGrantRepository PermissionGrantRepository;
    protected readonly IdentityRoleManager RoleManager;
    protected readonly ILookupNormalizer LookupNormalizer;
    protected readonly IUnitOfWorkManager UowManager;
    protected readonly IDistributedCache<PermissionGrantCacheItem> Cache;

    public Distributed_Role_Change_Events_Test()
    {
        RoleRepository = GetRequiredService<IIdentityRoleRepository>();
        ;
        PermissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        ;
        RoleManager = GetRequiredService<IdentityRoleManager>();
        ;
        LookupNormalizer = GetRequiredService<ILookupNormalizer>();
        ;
        UowManager = GetRequiredService<IUnitOfWorkManager>();
        Cache = GetRequiredService<IDistributedCache<PermissionGrantCacheItem>>();
    }

    [Fact]
    public void Should_Register_Handler()
    {
        var x = GetRequiredService<IOptions<AbpDistributedEntityEventOptions>>();
        GetRequiredService<IOptions<AbpDistributedEntityEventOptions>>()
            .Value
            .AutoEventSelectors
            .ShouldContain(m => m.Name == "Entity:" + typeof(IdentityRole).FullName);

        GetRequiredService<IOptions<AbpDistributedEventBusOptions>>()
            .Value
            .Handlers
            .ShouldContain(h => h == typeof(RoleUpdateEventHandler) || h == typeof(RoleDeletedEventHandler));
    }

    [Fact]
    public async Task Role_Updated_Distributed_Event_Test()
    {
        var role = await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("moderator"));

        var permissionGrantsInRole = await PermissionGrantRepository.GetListAsync("R", role.Name);
        permissionGrantsInRole.ShouldNotBeNull();
        permissionGrantsInRole.Count.ShouldBeGreaterThan(0);
        var count = permissionGrantsInRole.Count;

        using (var uow = UowManager.Begin())
        {
            var identityResult = await RoleManager.SetRoleNameAsync(role, "TestModerator");
            identityResult.Succeeded.ShouldBeTrue();
            await RoleRepository.UpdateAsync(role);
            await uow.CompleteAsync();
        }

        role = await RoleRepository.GetAsync(role.Id);
        role.Name.ShouldBe("TestModerator");

        permissionGrantsInRole = await PermissionGrantRepository.GetListAsync("R", role.Name);
        permissionGrantsInRole.Count.ShouldBe(count);
    }

    [Fact]
    public async Task Role_Deleted_Distributed_Event_Test()
    {
        var role = await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("moderator"));
        var permissionGrantsInRole = await PermissionGrantRepository.GetListAsync("R", role.Name);

        var caches = permissionGrantsInRole.Select(x => new KeyValuePair<string, PermissionGrantCacheItem>(
            PermissionGrantCacheItem.CalculateCacheKey(x.Name, x.ProviderName, x.ProviderKey),
            new PermissionGrantCacheItem(true))).ToList();
        await Cache.SetManyAsync(caches);


        using (var uow = UowManager.Begin())
        {
            await RoleRepository.DeleteAsync(role);
            await uow.CompleteAsync();
        }

        var permissionGrantCaches = await Cache.GetManyAsync(caches.Select(x => x.Key));
        foreach (var cache in permissionGrantCaches)
        {
            cache.Value.ShouldBeNull();
        }
    }
}
