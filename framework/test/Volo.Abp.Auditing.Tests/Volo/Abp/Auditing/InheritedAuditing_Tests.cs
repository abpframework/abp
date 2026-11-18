using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.Auditing;

public class InheritedAuditing_Tests : AbpAuditingTestBase
{
    [Fact]
    public void Should_Use_The_Concrete_Class_For_Inherited_Methods()
    {
        var helper = GetRequiredService<IAuditingHelper>();

        helper.ShouldSaveAudit(typeof(DisabledDerivedAuditService).GetMethod(nameof(BaseAuditService.InheritedAsync)), true).ShouldBeFalse();
        helper.ShouldSaveAudit(typeof(EnabledDerivedAuditService).GetMethod(nameof(PlainAuditService.InheritedAsync))).ShouldBeTrue();
        helper.ShouldSaveAudit(typeof(DisabledDerivedAuditService).GetMethod(nameof(DisabledDerivedAuditService.AuditedAsync))).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Not_Log_Parameters_Of_An_Inherited_Method_On_A_Disabled_Service()
    {
        var service = GetRequiredService<DisabledDerivedAuditService>();
        var manager = GetRequiredService<IAuditingManager>();
        using (var scope = manager.BeginScope())
        {
            await service.InheritedAsync("test-secret");
            manager.Current.Log.Actions.ShouldBeEmpty();

            await service.AuditedAsync();
            manager.Current.Log.Actions.ShouldHaveSingleItem().MethodName.ShouldBe(nameof(DisabledDerivedAuditService.AuditedAsync));
        }
    }
}

public class BaseAuditService : IAuditingEnabled
{
    public virtual Task InheritedAsync(string secret) => Task.CompletedTask;
}

[DisableAuditing]
public class DisabledDerivedAuditService : BaseAuditService, ITransientDependency
{
    [Audited]
    public virtual Task AuditedAsync() => Task.CompletedTask;
}

public class PlainAuditService
{
    public virtual Task InheritedAsync() => Task.CompletedTask;
}

[Audited]
public class EnabledDerivedAuditService : PlainAuditService
{
}
