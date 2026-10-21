using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Shouldly;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.SignalR.Auditing.TestApp;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.AspNetCore.SignalR.Auditing;

[Collection("AbpAspNetCoreSignalR")]
public class AbpAuditHubFilter_Tests : AbpAspNetCoreTestBase<Program>
{
    private IAuditingStore _auditingStore;

    public AbpAuditHubFilter_Tests()
    {
        _auditingStore = GetRequiredService<IAuditingStore>();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAspNetCoreAuditingOptions>(options =>
        {
            options.IgnoredHubTypes.Add(typeof(IgnoredAuditHub));
        });

        _auditingStore = Substitute.For<IAuditingStore>();
        services.Replace(ServiceDescriptor.Singleton(_auditingStore));
    }

    [Fact]
    public async Task Should_Begin_An_Audit_Scope_For_Regular_Hubs()
    {
        (await HasAuditScopeInHubMethodAsync(new RegularAuditHub())).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Not_Begin_An_Audit_Scope_For_Ignored_Hubs()
    {
        (await HasAuditScopeInHubMethodAsync(new IgnoredAuditHub())).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Not_Begin_An_Audit_Scope_For_Hubs_Derived_From_Ignored_Hubs()
    {
        (await HasAuditScopeInHubMethodAsync(new DerivedIgnoredAuditHub())).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Save_The_Hub_Audit_Log_After_A_Regular_Hub_Method_Completes()
    {
        var noteId = await CreateNoteAsync("old");
        using var scope = GetRequiredService<IServiceScopeFactory>().CreateScope();
        var noteService = scope.ServiceProvider.GetRequiredService<AuditedNoteService>();

        var invocationTask = InvokeHubMethodAsync(scope.ServiceProvider, new RegularAuditHub(), async () =>
        {
            await noteService.UpdateTextAsync(noteId, "new");
        });

        await Task.Delay(100);
        invocationTask.IsCompleted.ShouldBeFalse();
        await _auditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditLogInfo>());

        GetRequiredService<AuditedWorkSignal>().Complete();
        await invocationTask;

        await ShouldHaveSavedTheNoteUpdateAsync(noteId);
    }

    [Fact]
    public async Task Should_Save_The_Audit_Log_Of_A_Service_That_Completes_After_An_Ignored_Hub_Method_Returns()
    {
        var noteId = await CreateNoteAsync("old");
        using var scope = GetRequiredService<IServiceScopeFactory>().CreateScope();

        var serviceTask = await StartServiceInIgnoredHubMethodAsync(scope.ServiceProvider, noteId);
        await _auditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditLogInfo>());

        GetRequiredService<AuditedWorkSignal>().Complete();
        await serviceTask;

        await ShouldHaveSavedTheNoteUpdateAsync(noteId);
    }

    [Fact]
    public async Task Should_Save_The_Entity_Changes_Saved_By_The_Unit_Of_Work_Of_A_Service_That_Completes_After_An_Ignored_Hub_Method_Returns()
    {
        var noteId = await CreateNoteAsync("old");
        using var scope = GetRequiredService<IServiceScopeFactory>().CreateScope();

        var serviceTask = await StartServiceInIgnoredHubMethodAsync(
            scope.ServiceProvider,
            noteService => noteService.UpdateTextWithoutSavingAsync(noteId, "new"));
        await _auditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditLogInfo>());

        GetRequiredService<AuditedWorkSignal>().Complete();
        await serviceTask;

        await ShouldHaveSavedTheNoteUpdateAsync(noteId, nameof(AuditedNoteService.UpdateTextWithoutSavingAsync));
        (await GetRequiredService<IRepository<AuditedNote, Guid>>().GetAsync(noteId)).Text.ShouldBe("new");
    }

    [Fact]
    public async Task Should_Save_The_Audit_Log_Of_A_Service_Called_By_An_Ignored_Hub_Over_A_Non_WebSocket_Transport()
    {
        var noteId = await CreateNoteAsync("old");
        using var scope = GetRequiredService<IServiceScopeFactory>().CreateScope();

        // Server-Sent Events keep the connection on a single GET request.
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var connectionRequest = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
        connectionRequest.Request.Method = HttpMethods.Get;
        connectionRequest.Request.Path = "/signalr-hubs/ignored";
        httpContextAccessor.HttpContext = connectionRequest;

        try
        {
            var serviceTask = await StartServiceInIgnoredHubMethodAsync(scope.ServiceProvider, noteId);

            GetRequiredService<AuditedWorkSignal>().Complete();
            await serviceTask;
        }
        finally
        {
            httpContextAccessor.HttpContext = null;
        }

        await ShouldHaveSavedTheNoteUpdateAsync(noteId);
        await _auditingStore.Received(1).SaveAsync(Arg.Is<AuditLogInfo>(x => x.HttpMethod == null));
    }

    private Task<Task> StartServiceInIgnoredHubMethodAsync(IServiceProvider serviceProvider, Guid noteId)
    {
        return StartServiceInIgnoredHubMethodAsync(serviceProvider, noteService => noteService.UpdateTextAsync(noteId, "new"));
    }

    private async Task<Task> StartServiceInIgnoredHubMethodAsync(IServiceProvider serviceProvider, Func<AuditedNoteService, Task> serviceCall)
    {
        var noteService = serviceProvider.GetRequiredService<AuditedNoteService>();

        Task serviceTask = null;
        await InvokeHubMethodAsync(serviceProvider, new IgnoredAuditHub(), () =>
        {
            // Like Blazor's ComponentHub, return before the started work completes.
            serviceTask = serviceCall(noteService);
            return Task.CompletedTask;
        });

        return serviceTask;
    }

    private async Task<Guid> CreateNoteAsync(string text)
    {
        var noteId = Guid.NewGuid();
        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin(requiresNew: true))
        {
            await GetRequiredService<IRepository<AuditedNote, Guid>>().InsertAsync(new AuditedNote(noteId, text));
            await uow.CompleteAsync();
        }

        _auditingStore.ClearReceivedCalls();
        return noteId;
    }

    private async Task ShouldHaveSavedTheNoteUpdateAsync(Guid noteId, string methodName = nameof(AuditedNoteService.UpdateTextAsync))
    {
        await _auditingStore.Received(1).SaveAsync(Arg.Any<AuditLogInfo>());
        await _auditingStore.Received(1).SaveAsync(Arg.Is<AuditLogInfo>(x =>
            x.Actions.Count == 1 &&
            x.Actions[0].MethodName == methodName &&
            x.EntityChanges.Count == 1 &&
            x.EntityChanges[0].ChangeType == EntityChangeType.Updated &&
            x.EntityChanges[0].EntityTypeFullName == typeof(AuditedNote).FullName &&
            x.EntityChanges[0].EntityId == noteId.ToString() &&
            x.EntityChanges[0].PropertyChanges.Any(p =>
                p.PropertyName == nameof(AuditedNote.Text) &&
                p.OriginalValue == "\"old\"" &&
                p.NewValue == "\"new\"")));
    }

    private async Task<bool> HasAuditScopeInHubMethodAsync(Hub hub)
    {
        using var scope = GetRequiredService<IServiceScopeFactory>().CreateScope();
        var auditingManager = scope.ServiceProvider.GetRequiredService<IAuditingManager>();

        var hasAuditScope = false;
        await InvokeHubMethodAsync(scope.ServiceProvider, hub, () =>
        {
            hasAuditScope = auditingManager.Current != null;
            return Task.CompletedTask;
        });

        return hasAuditScope;
    }

    private static async Task InvokeHubMethodAsync(IServiceProvider serviceProvider, Hub hub, Func<Task> hubMethod)
    {
        var invocationContext = new HubInvocationContext(
            new TestHubCallerContext(),
            serviceProvider,
            hub,
            typeof(RegularAuditHub).GetMethod(nameof(RegularAuditHub.Ping)),
            Array.Empty<object>());

        // AbpAspNetCoreSignalRModule also registers the hub context accessor filter before the audit filter.
        await new AbpHubContextAccessorHubFilter().InvokeMethodAsync(invocationContext, context =>
            new AbpAuditHubFilter().InvokeMethodAsync(context, async _ =>
            {
                await hubMethod();
                return null;
            }));
    }

    [DisableConventionalRegistration]
    public class RegularAuditHub : Hub
    {
        public void Ping()
        {
        }
    }

    [DisableConventionalRegistration]
    public class IgnoredAuditHub : Hub
    {
    }

    [DisableConventionalRegistration]
    public class DerivedIgnoredAuditHub : IgnoredAuditHub
    {
    }
}
