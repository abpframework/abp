using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Uow;

public class AlwaysDisableUnitOfWorkTransaction_Tests: AbpIntegratedTest<AbpUnitOfWorkModule>
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddAlwaysDisableUnitOfWorkTransaction();
    }

    [Fact]
    public void AlwaysDisableUnitOfWorkTransaction_Test()
    {
        GetService<UnitOfWorkManager>().ShouldNotBeNull();

        var unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        unitOfWorkManager.GetType().ShouldBe(typeof(AlwaysDisableTransactionsUnitOfWorkManager));

        using (var uow = unitOfWorkManager.Begin(isTransactional: true))
        {
            uow.Options.IsTransactional.ShouldBeFalse();
        }
    }
}
