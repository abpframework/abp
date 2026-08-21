using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Uow;

[Route("api/uow-visibility")]
public class UowVisibilityController : AbpController
{
    private readonly IRepository<UowVisibilityTestEntity, Guid> _repository;

    public UowVisibilityController(IRepository<UowVisibilityTestEntity, Guid> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Route("insert-then-read")]
    [UnitOfWork(isTransactional: true)]
    public async Task InsertThenRead()
    {
        var name = Guid.NewGuid().ToString("N");
        await _repository.InsertAsync(new UowVisibilityTestEntity(Guid.NewGuid(), name));

        await Response.WriteAsync("inserted");
        await Response.Body.FlushAsync();

        int count;
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
        {
            count = await _repository.CountAsync(x => x.Name == name);
            await uow.CompleteAsync();
        }

        await Response.WriteAsync(count == 1 ? ":visible" : ":not-visible");
    }

    [HttpGet]
    [Route("insert")]
    [UnitOfWork(isTransactional: true)]
    public async Task Insert(string name)
    {
        await _repository.InsertAsync(new UowVisibilityTestEntity(Guid.NewGuid(), name));
        await Response.WriteAsync("inserted");
    }

    [HttpGet]
    [Route("count")]
    public async Task Count(string name)
    {
        var count = await _repository.CountAsync(x => x.Name == name);
        await Response.WriteAsync(count.ToString());
    }

    // Insert (autoSave sends the INSERT to the transaction) then throw before the response: must roll back.
    [HttpGet]
    [Route("insert-then-throw")]
    [UnitOfWork(isTransactional: true)]
    public async Task InsertThenThrow(string name)
    {
        await _repository.InsertAsync(new UowVisibilityTestEntity(Guid.NewGuid(), name), autoSave: true);
        throw new AbpException("boom before the response started");
    }

    // Insert, flush the response (committed here when enabled), then throw: the committed row survives.
    [HttpGet]
    [Route("insert-flush-then-throw")]
    [UnitOfWork(isTransactional: true)]
    public async Task InsertFlushThenThrow(string name)
    {
        await _repository.InsertAsync(new UowVisibilityTestEntity(Guid.NewGuid(), name));

        await Response.WriteAsync("inserted");
        await Response.Body.FlushAsync();

        throw new AbpException("boom after the response started");
    }
    // Insert, register a completed handler that throws (runs after commit), then flush.
    [HttpGet]
    [Route("insert-flush-throwing-completed-handler")]
    [UnitOfWork(isTransactional: true)]
    public async Task InsertFlushWithThrowingCompletedHandler(string name)
    {
        await _repository.InsertAsync(new UowVisibilityTestEntity(Guid.NewGuid(), name));
        CurrentUnitOfWork.OnCompleted(() => throw new AbpException("boom in a completed handler"));

        await Response.WriteAsync("inserted");
        await Response.Body.FlushAsync();
    }
    // Insert a valid row plus an invalid one (Name is required): the commit at response start fails.
    [HttpGet]
    [Route("insert-then-fail-commit")]
    [UnitOfWork(isTransactional: true)]
    public async Task InsertThenFailCommit(string name)
    {
        await _repository.InsertAsync(new UowVisibilityTestEntity(Guid.NewGuid(), name));
        await _repository.InsertAsync(new UowVisibilityTestEntity(Guid.NewGuid(), null));

        await Response.WriteAsync("inserted");
        await Response.Body.FlushAsync();
    }

    // The action succeeds (so the action filter saves changes), then serializing the object result throws
    // before the response starts. The upstream exception middleware writes the error response after the
    // request unit of work is disposed, so response-start completion must not commit the failed request.
    [HttpGet]
    [Route("insert-then-throw-in-serialization")]
    [UnitOfWork(isTransactional: true)]
    public async Task<IActionResult> InsertThenThrowInSerialization(string name)
    {
        await _repository.InsertAsync(new UowVisibilityTestEntity(Guid.NewGuid(), name));
        return Ok(new ThrowingOnSerializeDto());
    }

    public class ThrowingOnSerializeDto
    {
        public string Value => throw new AbpException("boom while serializing the object result");
    }
}
