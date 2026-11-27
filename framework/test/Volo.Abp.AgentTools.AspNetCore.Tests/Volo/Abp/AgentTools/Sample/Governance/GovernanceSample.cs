using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.AgentTools.Sample.Governance;

// Remote service "GovernanceSample" (root path "governance"), own namespace so the other samples' tool lists stay as they are.

public static class GovernanceSampleConsts
{
    public const string RemoteServiceName = "GovernanceSample";

    public const string RootPath = "governance";
}

/// <summary>ABP CRUD service: its permissions are policy-name properties checked inside the method bodies.</summary>
public interface IBookCrudAppService
    : ICrudAppService<BookDto, Guid, PagedAndSortedResultRequestDto, CreateBookDto, UpdateBookDto>
{
}

public class BookCrudAppService
    : CrudAppService<Book, BookDto, Guid, PagedAndSortedResultRequestDto, CreateBookDto, UpdateBookDto>, IBookCrudAppService
{
    public BookCrudAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {
        GetPolicyName = SamplePermissions.Books.Default;
        GetListPolicyName = SamplePermissions.Books.Default;
        CreatePolicyName = SamplePermissions.Books.Create;
        UpdatePolicyName = SamplePermissions.Books.Create;
        DeletePolicyName = SamplePermissions.Books.Delete;
    }

    protected override IQueryable<Book> ApplyDefaultSorting(IQueryable<Book> query) => query.OrderBy(b => b.Name);

    protected override BookDto MapToGetOutputDto(Book entity) => new() { Id = entity.Id, Name = entity.Name };

    protected override BookDto MapToGetListOutputDto(Book entity) => MapToGetOutputDto(entity);

    protected override Book MapToEntity(CreateBookDto createInput) => new() { TenantId = CurrentTenant.Id, Name = createInput.Name };

    protected override void MapToEntity(UpdateBookDto updateInput, Book entity) => entity.Name = updateInput.Name;
}

public interface ISampleVaultAppService : IApplicationService
{
    Task<string> GetSecretAsync();

    /// <summary>Inserts a book (autoSave), then throws a non-business exception with an internal message.</summary>
    Task<BookDto> CreateBookThenCrashAsync(CreateBookDto input);

    /// <summary>JSON body whose DTO carries a stream property: never a tool.</summary>
    Task UploadAsync(VaultUploadDto input);
}

public class VaultUploadDto
{
    public string Name { get; set; } = default!;

    public IRemoteStreamContent? File { get; set; }
}

[RemoteService(IsEnabled = false)]
public class SampleVaultAppService : ApplicationService, ISampleVaultAppService
{
    public const string InternalDetail = "connection to vault-db-7 refused";

    private readonly IRepository<Book, Guid> _books;

    public SampleVaultAppService(IRepository<Book, Guid> books)
    {
        _books = books;
    }

    public Task<string> GetSecretAsync() => Task.FromResult("42");

    public async Task<BookDto> CreateBookThenCrashAsync(CreateBookDto input)
    {
        await _books.InsertAsync(new Book { TenantId = CurrentTenant.Id, Name = input.Name }, autoSave: true);
        throw new InvalidOperationException(InternalDetail);
    }

    public Task UploadAsync(VaultUploadDto input) => Task.CompletedTask;
}

/// <summary>Declares an action for the derived controller; carries no attributes itself.</summary>
public abstract class SampleVaultControllerBase : AbpControllerBase
{
    protected ISampleVaultAppService Vault { get; }

    protected SampleVaultControllerBase(ISampleVaultAppService vault)
    {
        Vault = vault;
    }

    [HttpGet("secret")]
    public Task<string> GetSecretAsync() => Vault.GetSecretAsync();
}

/// <summary>Class-level [Authorize] on the actual controller; GetSecretAsync is declared on the base controller.</summary>
[RemoteService(Name = GovernanceSampleConsts.RemoteServiceName)]
[Area(GovernanceSampleConsts.RootPath)]
[Route("api/governance/vault")]
[Authorize(SamplePermissions.Reports.Default)]
public class SampleVaultController : SampleVaultControllerBase, ISampleVaultAppService
{
    public SampleVaultController(ISampleVaultAppService vault)
        : base(vault)
    {
    }

    [HttpPost("book")]
    public Task<BookDto> CreateBookThenCrashAsync(CreateBookDto input) => Vault.CreateBookThenCrashAsync(input);

    [HttpPost("upload")]
    public Task UploadAsync([FromBody] VaultUploadDto input) => Vault.UploadAsync(input);
}

/// <summary>An interface whose DI registration is the controller itself: skipped, controllers are never invoked.</summary>
public interface ISampleSelfServedAppService : IApplicationService
{
    Task<string> GetAsync();
}

[RemoteService(Name = GovernanceSampleConsts.RemoteServiceName)]
[Area(GovernanceSampleConsts.RootPath)]
[Route("api/governance/self-served")]
[ExposeServices(typeof(ISampleSelfServedAppService), IncludeSelf = true)]
public class SampleSelfServedController : AbpControllerBase, ISampleSelfServedAppService
{
    [HttpGet]
    public Task<string> GetAsync() => Task.FromResult("self");
}
