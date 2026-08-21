using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MemoryDb;
using Volo.Abp.TestApp.MemoryDb;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.Uow;

[Route("api/unitofwork-test")]
public class UnitOfWorkTestController : AbpController
{
    private readonly TestUnitOfWorkConfig _testUnitOfWorkConfig;

    public UnitOfWorkTestController(TestUnitOfWorkConfig testUnitOfWorkConfig)
    {
        _testUnitOfWorkConfig = testUnitOfWorkConfig;
    }

    [HttpGet]
    [Route("ActionRequiresUow")]
    public ActionResult ActionRequiresUow()
    {
        CurrentUnitOfWork.ShouldNotBeNull();
        CurrentUnitOfWork.Options.IsTransactional.ShouldBeFalse();

        return Content("OK");
    }

    [HttpPost]
    [Route("ActionRequiresUowPost")]
    public ActionResult ActionRequiresUowPost()
    {
        CurrentUnitOfWork.ShouldNotBeNull();
        CurrentUnitOfWork.Options.IsTransactional.ShouldBeTrue();

        return Content("OK");
    }

    [AcceptVerbs("QUERY")]
    [Route("ActionRequiresUowQuery")]
    public ActionResult ActionRequiresUowQuery()
    {
        CurrentUnitOfWork.ShouldNotBeNull();
        CurrentUnitOfWork.Options.IsTransactional.ShouldBeFalse();

        return Content("OK");
    }

    [HttpGet]
    [Route("HandledException")]
    [UnitOfWork(isTransactional: true)]
    public void HandledException()
    {
        CurrentUnitOfWork.ShouldNotBeNull();
        CurrentUnitOfWork.Options.IsTransactional.ShouldBeTrue();

        throw new UserFriendlyException("This is a sample exception!");
    }

    [HttpGet]
    [Route("ExceptionOnComplete")]
    public void ExceptionOnComplete()
    {
        CurrentUnitOfWork.ShouldNotBeNull();
        CurrentUnitOfWork.Options.IsTransactional.ShouldBeFalse();

        _testUnitOfWorkConfig.ThrowExceptionOnComplete = true;
    }

    [HttpGet]
    [Route("CommitBeforeResponseFlush")]
    public async Task CommitBeforeResponseFlush()
    {
        var uow = CurrentUnitOfWork;
        uow.ShouldNotBeNull();

        // Start the response from inside the pipeline, before the middleware would commit.
        await Response.WriteAsync("first");
        await Response.Body.FlushAsync();

        await Response.WriteAsync(uow.IsCompleted ? ":completed" : ":not-completed");
    }

    [HttpGet]
    [Route("CommitThenThrowAfterResponseFlush")]
    public async Task CommitThenThrowAfterResponseFlush()
    {
        var uow = CurrentUnitOfWork;

        await Response.WriteAsync("first");
        await Response.Body.FlushAsync();

        _testUnitOfWorkConfig.UowCompletedAfterResponseFlush = uow.IsCompleted;

        throw new UserFriendlyException("boom after the response was already flushed");
    }

    [HttpGet]
    [Route("ReadRepositoryAfterResponseFlush")]
    public async Task ReadRepositoryAfterResponseFlush()
    {
        var repository = LazyServiceProvider.LazyGetRequiredService<IRepository<Person, Guid>>();

        var before = (await repository.GetListAsync()).Count;
        await Response.WriteAsync($"before=ok({before})");
        await Response.Body.FlushAsync();

        string after;
        try
        {
            var count = (await repository.GetListAsync()).Count;
            after = $";after=ok({count},ambient={(UnitOfWorkManager.Current == null ? "null" : "present")})";
        }
        catch (Exception ex)
        {
            after = $";after=threw:{ex.GetType().Name}";
        }

        await Response.WriteAsync(after);
    }

    [HttpGet]
    [Route("RawDatabaseProviderAfterResponseFlush")]
    public async Task RawDatabaseProviderAfterResponseFlush()
    {
        var databaseProvider = LazyServiceProvider
            .LazyGetRequiredService<IMemoryDatabaseProvider<TestAppMemoryDbContext>>();

        await Response.WriteAsync("first");
        await Response.Body.FlushAsync();

        string outcome;
        try
        {
            await databaseProvider.GetDatabaseAsync();
            outcome = ":ok";
        }
        catch (AbpException)
        {
            // Raw provider access has no ambient uow once the response started, so it throws.
            outcome = ":threw-AbpException";
        }

        await Response.WriteAsync(outcome);
    }

    [HttpGet]
    [Route("NestedUowDuringResponseFlush")]
    public async Task NestedUowDuringResponseFlush()
    {
        using (var nested = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
        {
            await Response.WriteAsync("first");
            await Response.Body.FlushAsync();

            // The outer request unit of work (nested.Outer) must not have been completed on response
            // start while a nested unit of work is current.
            await Response.WriteAsync(nested.Outer!.IsCompleted ? ":outer-completed" : ":outer-not-completed");

            string outcome;
            try
            {
                await nested.CompleteAsync();
                outcome = ":nested-completed-by-owner";
            }
            catch (AbpException)
            {
                outcome = ":nested-already-completed";
            }

            await Response.WriteAsync(outcome);
        }
    }

    [HttpGet]
    [Route("CompleteCurrentUow")]
    public async Task CompleteCurrentUow()
    {
        // Complete the request unit of work inside the action, without writing the response yet.
        // The middleware must still try to complete it at the end of the pipeline (original behavior).
        await CurrentUnitOfWork.CompleteAsync();
    }
}
