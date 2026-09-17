using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

[ExposeServices(
    typeof(IRepository<Book, Guid>),
    typeof(IReadOnlyRepository<Book, Guid>),
    typeof(IReadOnlyRepository<Book>))]
public class BookRepository : RepositoryBase<Book, Guid>, ISingletonDependency
{
    private readonly List<Book> _books = new();

    public BookRepository()
        : base("InMemory")
    {

    }

    public override Task<IQueryable<Book>> GetQueryableAsync()
    {
        return Task.FromResult(_books.AsQueryable());
    }

    [Obsolete("Use GetQueryableAsync method.")]
    protected override IQueryable<Book> GetQueryable()
    {
        return _books.AsQueryable();
    }

    public override Task<Book> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var book = _books.FirstOrDefault(x => x.Id == id);
        if (book == null)
        {
            throw new EntityNotFoundException(typeof(Book), id);
        }

        return Task.FromResult(book);
    }

    public override Task<Book> FindAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_books.FirstOrDefault(x => x.Id == id));
    }

    public override Task<Book> FindAsync(Expression<Func<Book, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_books.AsQueryable().FirstOrDefault(predicate));
    }

    public override Task<List<Book>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_books.ToList());
    }

    public override Task<List<Book>> GetListAsync(Expression<Func<Book, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_books.AsQueryable().Where(predicate).ToList());
    }

    public override Task<List<Book>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_books.Skip(skipCount).Take(maxResultCount).ToList());
    }

    public override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((long)_books.Count);
    }

    public override Task<Book> InsertAsync(Book entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        _books.Add(entity);
        return Task.FromResult(entity);
    }

    public override Task<Book> UpdateAsync(Book entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(entity);
    }

    public override Task DeleteAsync(Book entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        _books.Remove(entity);
        return Task.CompletedTask;
    }

    public override Task DeleteAsync(Expression<Func<Book, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        _books.RemoveAll(new Predicate<Book>(predicate.Compile()));
        return Task.CompletedTask;
    }

    public override Task DeleteDirectAsync(Expression<Func<Book, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(predicate);
    }
}
