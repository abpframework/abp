using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AgentTools.Mcp.Sample;

/// <summary>
/// The tools of the MCP test host (remote service "McpSample"): McpSample_GetNotes (GET, free), McpSample_GetSecret
/// (GET, needs <see cref="McpSamplePermissions.Secret"/>), McpSample_CreateNote (POST, approval turned off), McpSample_DeleteNote
/// (DELETE, approval by default) and McpSample_GetDisabled (GET, disabled in the tests' policy store).
/// </summary>
public interface IMcpSampleAppService : IApplicationService
{
    [Description("Lists the notes of the current tenant.")]
    Task<List<string>> GetNotesAsync();

    Task<string> GetSecretAsync();

    Task<NoteDto> CreateNoteAsync(CreateNoteInput input);

    Task DeleteNoteAsync(Guid id);

    Task<string> GetDisabledAsync();
}

public class McpSampleAppService : ApplicationService, IMcpSampleAppService
{
    public const string RemoteServiceName = "McpSample";

    public const string GetNotes = "McpSample_GetNotes";
    public const string GetSecret = "McpSample_GetSecret";
    public const string CreateNote = "McpSample_CreateNote";
    public const string DeleteNote = "McpSample_DeleteNote";
    public const string GetDisabled = "McpSample_GetDisabled";

    private readonly IRepository<Note, Guid> _notes;

    public McpSampleAppService(IRepository<Note, Guid> notes)
    {
        _notes = notes;
    }

    public async Task<List<string>> GetNotesAsync()
    {
        return (await _notes.GetListAsync()).Select(n => n.Text).OrderBy(t => t).ToList();
    }

    [Authorize(McpSamplePermissions.Secret)]
    public Task<string> GetSecretAsync() => Task.FromResult("the secret");

    [AgentTool(RequiresApproval = false)]
    public async Task<NoteDto> CreateNoteAsync(CreateNoteInput input)
    {
        if (input.Text == "fail")
        {
            throw new BusinessException("McpSample:NoFail", "A note cannot say fail.");
        }

        var note = await _notes.InsertAsync(new Note { TenantId = CurrentTenant.Id, Text = input.Text }, autoSave: true);
        return new NoteDto { Id = note.Id, Text = note.Text };
    }

    public async Task DeleteNoteAsync(Guid id) => await _notes.DeleteAsync(id);

    public Task<string> GetDisabledAsync() => Task.FromResult("disabled");
}

public class CreateNoteInput
{
    [Required]
    [StringLength(20)]
    public string Text { get; set; } = default!;
}

public class NoteDto
{
    public Guid Id { get; set; }

    public string Text { get; set; } = default!;
}

public class Note : AggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public string Text { get; set; } = default!;
}

public class McpSampleDbContext : AbpDbContext<McpSampleDbContext>
{
    public DbSet<Note> Notes => Set<Note>();

    public McpSampleDbContext(DbContextOptions<McpSampleDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Note>(b =>
        {
            b.ToTable("Notes");
            b.ConfigureByConvention();
        });
    }
}

public static class McpSamplePermissions
{
    public const string Secret = "McpSample.Secret";
}

public class McpSamplePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        context.AddGroup("McpSample").AddPermission(McpSamplePermissions.Secret);
    }
}
