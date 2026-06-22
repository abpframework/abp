using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.Auditing;

public class AuditingHelper_Tests : AbpAuditingTestBase
{
    private readonly IAuditingHelper _auditingHelper;
    protected IAuditingStore AuditingStore;

    public AuditingHelper_Tests()
    {
        _auditingHelper = GetRequiredService<IAuditingHelper>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        AuditingStore = Substitute.For<IAuditingStore>();
        services.Replace(ServiceDescriptor.Singleton(AuditingStore));
    }

    [Fact]
    public async Task Should_Write_AuditLog_Without_DisableAuditing()
    {
        var myAuditedObject = GetRequiredService<MyAuditedObject>();

        await myAuditedObject.DoItAsync();

        await AuditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
    }

    [Fact]
    public async Task Should_Not_Write_AuditLog_With_DisableAuditing()
    {
        var myAuditedObject = GetRequiredService<MyAuditedObject>();

        using (_auditingHelper.DisableAuditing())
        {
            await myAuditedObject.DoItAsync();
        }

        await AuditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditLogInfo>());
    }

    [Fact]
    public async Task Should_Write_AuditLog_After_DisableAuditing_Scope_Disposed()
    {
        var myAuditedObject = GetRequiredService<MyAuditedObject>();

        using (_auditingHelper.DisableAuditing())
        {
            await myAuditedObject.DoItAsync();
        }

        AuditingStore.ClearReceivedCalls();

        await myAuditedObject.DoItAsync();

        await AuditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
    }

    [Fact]
    public async Task Should_Not_Write_AuditLog_With_Nested_DisableAuditing()
    {
        var myAuditedObject = GetRequiredService<MyAuditedObject>();

        using (_auditingHelper.DisableAuditing())
        {
            await myAuditedObject.DoItAsync();

            using (_auditingHelper.DisableAuditing())
            {
                await myAuditedObject.DoItAsync();
            }

            await myAuditedObject.DoItAsync();
        }

        await AuditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditLogInfo>());
    }

    [Fact]
    public void Should_Return_True_When_Auditing_Is_Enabled()
    {
        Assert.True(_auditingHelper.IsAuditingEnabled());
    }

    [Fact]
    public void Should_Return_False_When_Auditing_Is_Disabled()
    {
        using (_auditingHelper.DisableAuditing())
        {
            Assert.False(_auditingHelper.IsAuditingEnabled());
        }
    }

    [Fact]
    public void Should_Return_True_After_DisableAuditing_Scope_Disposed()
    {
        using (_auditingHelper.DisableAuditing())
        {
            Assert.False(_auditingHelper.IsAuditingEnabled());
        }

        Assert.True(_auditingHelper.IsAuditingEnabled());
    }

    [Fact]
    public void Should_Return_False_With_Nested_DisableAuditing()
    {
        using (_auditingHelper.DisableAuditing())
        {
            Assert.False(_auditingHelper.IsAuditingEnabled());

            using (_auditingHelper.DisableAuditing())
            {
                Assert.False(_auditingHelper.IsAuditingEnabled());
            }

            Assert.False(_auditingHelper.IsAuditingEnabled());
        }
    }

    public interface IMyAuditedObject : ITransientDependency, IAuditingEnabled
    {
    }

    public class MyAuditedObject : IMyAuditedObject
    {
        public virtual Task DoItAsync()
        {
            return Task.CompletedTask;
        }
    }
}
