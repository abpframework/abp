using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

public abstract class OrganizationUnitManager_CreateMany_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected OrganizationUnitManager OrganizationUnitManager { get; }
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    protected ILookupNormalizer LookupNormalizer { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IGuidGenerator GuidGenerator { get; }

    protected OrganizationUnitManager_CreateMany_Tests()
    {
        OrganizationUnitManager = GetRequiredService<OrganizationUnitManager>();
        OrganizationUnitRepository = GetRequiredService<IOrganizationUnitRepository>();
        LookupNormalizer = GetRequiredService<ILookupNormalizer>();
        UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        GuidGenerator = GetRequiredService<IGuidGenerator>();
    }

    [Fact]
    public virtual async Task Should_Not_Insert_Anything_When_A_Group_Is_Not_Valid()
    {
        var validDisplayName = $"valid-{Guid.NewGuid():N}";

        using (var uow = UnitOfWorkManager.Begin())
        {
            var parent = await OrganizationUnitRepository.GetAsync("OU1");

            //The second group is not valid, OU11 is already a child of OU1.
            await Should.ThrowAsync<BusinessException>(async () =>
                await OrganizationUnitManager.CreateManyAsync([
                    new OrganizationUnit(GuidGenerator.Create(), validDisplayName),
                    new OrganizationUnit(GuidGenerator.Create(), "OU11", parent.Id)
                ]));
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await OrganizationUnitRepository.GetAsync(validDisplayName)).ShouldBeNull();

            await uow.CompleteAsync();
        }
    }
}
