using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.AgentTools.Sample;

[RemoteService(Name = SampleRemoteServices.AgentToolsSample)]
public class BookAppService : ApplicationService, IBookAppService
{
    public const string CreateThenFailErrorCode = "AgentToolsSample:CreateThenFail";

    private readonly IRepository<Book, Guid> _books;

    public BookAppService(IRepository<Book, Guid> books)
    {
        _books = books;
    }

    public async Task<BookDto> GetAsync(Guid id)
    {
        return Map(await _books.GetAsync(id));
    }

    public async Task<List<BookDto>> GetListAsync()
    {
        return (await _books.GetListAsync()).Select(Map).ToList();
    }

    [Authorize(SamplePermissions.Books.Create)]
    public async Task<BookDto> CreateAsync(CreateBookDto input)
    {
        return Map(await _books.InsertAsync(new Book { TenantId = CurrentTenant.Id, Name = input.Name }, autoSave: true));
    }

    public async Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input)
    {
        var book = await _books.GetAsync(id);
        book.Name = input.Name;
        return Map(await _books.UpdateAsync(book, autoSave: true));
    }

    [Authorize(SamplePermissions.Books.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _books.DeleteAsync(id, autoSave: true);
    }

    public async Task<BookDto> CreateThenFailAsync(CreateBookDto input)
    {
        await _books.InsertAsync(new Book { TenantId = CurrentTenant.Id, Name = input.Name }, autoSave: true);
        throw new BusinessException(CreateThenFailErrorCode, "The book was saved, then the operation failed.");
    }

    public async Task<List<BookDto>> SearchAsync(string name)
    {
        return (await _books.GetListAsync(b => b.Name.Contains(name))).Select(Map).ToList();
    }

    public async Task ArchiveAsync(Guid id)
    {
        await _books.DeleteAsync(id, autoSave: true);
    }

    public async Task<int> GetCountAsync()
    {
        return (int)await _books.GetCountAsync();
    }

    public async Task ResetAllAsync()
    {
        await _books.DeleteAsync(_ => true, autoSave: true);
    }

    public Task UploadCoverAsync(Guid id, IRemoteStreamContent cover)
    {
        return Task.CompletedTask;
    }

    public Task<IRemoteStreamContent> GetCoverAsync(Guid id)
    {
        return Task.FromResult<IRemoteStreamContent>(
            new RemoteStreamContent(new MemoryStream(Encoding.UTF8.GetBytes("cover")), "cover.txt", "text/plain"));
    }

    [AllowAnonymous]
    public async Task<int> GetPublicCountAsync()
    {
        return (int)await _books.GetCountAsync();
    }

    private static BookDto Map(Book book) => new() { Id = book.Id, Name = book.Name };
}
