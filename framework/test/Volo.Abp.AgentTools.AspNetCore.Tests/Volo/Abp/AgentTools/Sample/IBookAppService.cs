using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.Abp.AgentTools.Sample;

/// <summary>Conventional controller under remote service "AgentToolsSample", root path "app".</summary>
[RemoteService(Name = SampleRemoteServices.AgentToolsSample)]
public interface IBookAppService : IApplicationService
{
    Task<BookDto> GetAsync(Guid id);

    /// <summary>Lists the books of the current tenant.</summary>
    Task<List<BookDto>> GetListAsync();

    Task<BookDto> CreateAsync(CreateBookDto input);

    Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input);

    Task DeleteAsync(Guid id);

    /// <summary>Inserts a book (autoSave), then throws a BusinessException: the insert must be rolled back.</summary>
    Task<BookDto> CreateThenFailAsync(CreateBookDto input);

    // POST by convention; the attribute makes it a read-only tool with its own name and description.
    [AgentTool(Name = "Book_Find", Description = "Finds books whose name contains the given text.", ReadOnly = true)]
    Task<List<BookDto>> SearchAsync(string name);

    // POST by convention; the attribute makes it destructive without approval.
    [AgentTool(Destructive = true, RequiresApproval = false)]
    Task ArchiveAsync(Guid id);

    [Description("Counts the books of the current tenant.")]
    [AgentTool(RequiresApproval = true)]
    Task<int> GetCountAsync();

    [DisableAgentTool]
    Task ResetAllAsync();

    Task UploadCoverAsync(Guid id, IRemoteStreamContent cover);

    Task<IRemoteStreamContent> GetCoverAsync(Guid id);

    /// <summary>[AllowAnonymous] on the implementation.</summary>
    Task<int> GetPublicCountAsync();
}
