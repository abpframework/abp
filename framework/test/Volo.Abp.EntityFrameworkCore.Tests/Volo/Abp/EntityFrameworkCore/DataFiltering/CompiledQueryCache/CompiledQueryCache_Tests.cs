using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCache_Tests : CompiledQueryCacheTestBase<CompiledQueryCacheTestModule>
{
    protected override bool UseDbFunction => true;

    [Fact]
    public async Task Should_Not_Run_Compiled_Query_Without_SoftDelete_Filter()
    {
        var query = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.SoftDeleteRows.Select(x => x.Name));

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(query(db).ToList())));
    }

    [Fact]
    public async Task Should_Not_Run_Compiled_Query_Without_MultiTenant_Filter()
    {
        var query = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.MultiTenantRows.Select(x => x.Name));

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(query(db).ToList())));
    }

    [Fact]
    public async Task Should_Not_Run_Compiled_Query_First_Run_Inside_Another_Query_Without_Filters()
    {
        var countRows = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());

        // EF Core wraps the exceptions thrown while it evaluates the parameters of the outer query.
        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                Task.FromResult(db.Rows.Where(row => row.Name.Length > countRows(db)).Select(row => row.Name).ToList())));
        exception.InnerException.ShouldBeOfType<AbpException>();

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(new[] { countRows(db).ToString() }.ToList())));
    }

    [Fact]
    public async Task Should_Not_Run_Compiled_Async_Query_First_Run_Inside_Another_Query_Without_Filters()
    {
        var countRowsAsync = EF.CompileAsyncQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());

        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                Task.FromResult(db.Rows
                    .Where(row => row.Name.Length > countRowsAsync(db).GetAwaiter().GetResult())
                    .Select(row => row.Name)
                    .ToList())));
        exception.InnerException.ShouldBeOfType<AbpException>();

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, async db => new[] { (await countRowsAsync(db)).ToString() }.ToList()));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Should_Not_Run_Compiled_Query_First_Run_Inside_Another_Query_With_Disabled_Filters(bool asyncCompiledQuery)
    {
        var countRows = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());
        var countRowsAsync = EF.CompileAsyncQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());
        var dataFilter = GetRequiredService<IDataFilter>();

        using (dataFilter.Disable<ISoftDelete>())
        using (dataFilter.Disable<IMultiTenant>())
        {
            var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
                RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                    Task.FromResult(asyncCompiledQuery
                        ? db.Rows.Where(row => row.Name.Length > countRowsAsync(db).GetAwaiter().GetResult()).Select(row => row.Name).ToList()
                        : db.Rows.Where(row => row.Name.Length > countRows(db)).Select(row => row.Name).ToList())));
            exception.InnerException.ShouldBeOfType<AbpException>();
        }

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, async db =>
                new[] { (asyncCompiledQuery ? await countRowsAsync(db) : countRows(db)).ToString() }.ToList()));
    }

    [Fact]
    public async Task Should_Not_Run_Compiled_Query_First_Run_While_Another_Query_Is_Compiled_Without_Filters()
    {
        var countRows = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());
        var countRowsAsync = EF.CompileAsyncQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());
        var hooks = GetRequiredService<CompiledQueryCacheTestHooks>();

        hooks.OnQueryCompilationStarting = db =>
        {
            hooks.OnQueryCompilationStarting = null;
            countRows(db);
        };
        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                Task.FromResult(db.Rows.TagWith(Guid.NewGuid().ToString()).Select(row => row.Name).ToList())));

        hooks.OnQueryCompilationStarting = db =>
        {
            hooks.OnQueryCompilationStarting = null;
            countRowsAsync(db).GetAwaiter().GetResult();
        };
        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                db.Rows.TagWith(Guid.NewGuid().ToString()).Select(row => row.Name).ToListAsync()));

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(new[] { countRows(db).ToString() }.ToList())));
    }

    [Fact]
    public async Task Should_Not_Run_Compiled_Query_First_Run_While_Cache_Key_Is_Generated_Without_Filters()
    {
        var countRows = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());
        var hooks = GetRequiredService<CompiledQueryCacheTestHooks>();

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
            {
                hooks.OnGetCompiledQueryCacheKey = () =>
                {
                    hooks.OnGetCompiledQueryCacheKey = null;
                    countRows(db);
                };

                return Task.FromResult(db.Rows.TagWith(Guid.NewGuid().ToString()).Select(row => row.Name).ToList());
            }));

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(new[] { countRows(db).ToString() }.ToList())));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Should_Not_Precompile_Query_While_Cache_Key_Is_Generated_Without_Filters(bool async)
    {
        var hooks = GetRequiredService<CompiledQueryCacheTestHooks>();

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
            {
                hooks.OnGetCompiledQueryCacheKey = () =>
                {
                    hooks.OnGetCompiledQueryCacheKey = null;
#pragma warning disable EF1001, EF9100
                    var queryCompiler = db.GetService<Microsoft.EntityFrameworkCore.Query.Internal.IQueryCompiler>();
                    var query = db.Rows.Select(row => row.Name).Expression;
                    if (async)
                    {
                        queryCompiler.PrecompileQuery<IAsyncEnumerable<string>>(query, true);
                    }
                    else
                    {
                        queryCompiler.PrecompileQuery<IEnumerable<string>>(query, false);
                    }
#pragma warning restore EF1001, EF9100
                };

                return Task.FromResult(db.Rows.TagWith(Guid.NewGuid().ToString()).Select(row => row.Name).ToList());
            }));
    }

    [Fact]
    public async Task Should_Provide_Current_DbContext_While_Cache_Key_Is_Generated()
    {
        var hooks = GetRequiredService<CompiledQueryCacheTestHooks>();

        await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, async db =>
        {
            var currentDbContext = db.GetService<AbpEfCoreCurrentDbContext>();
            var contexts = new List<IAbpEfCoreDbFunctionContext>();
            hooks.OnGetCompiledQueryCacheKey = () => contexts.Add(currentDbContext.Context);
            try
            {
                db.Rows.Select(row => row.Name).ToList();
                await db.Rows.Select(row => row.Name).ToListAsync();
            }
            finally
            {
                hooks.OnGetCompiledQueryCacheKey = null;
            }

            contexts.Count.ShouldBe(2);
            contexts.ShouldAllBe(context => context == db);
            return new List<string>();
        });
    }

    [Fact]
    public async Task Should_Not_Compile_Query_With_Cache_Key_Generated_Outside_Of_Query_Provider()
    {
        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
            {
                var expression = ((IQueryable<CompiledQueryCacheTestRow>)db.Rows).Expression;
                db.GetService<ICompiledQueryCacheKeyGenerator>().GenerateCacheKey(expression, false);
                db.GetService<IDatabase>().CompileQuery<IEnumerable<CompiledQueryCacheTestRow>>(expression, false);
                return Task.FromResult(new List<string>());
            }));
    }

    [Fact]
    public async Task Should_Apply_Filters_To_Normal_Query_After_Compiled_Query_Failed()
    {
        var countRows = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());

        await Should.ThrowAsync<InvalidOperationException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                Task.FromResult(db.Rows.Where(row => row.Name.Length > countRows(db)).Select(row => row.Name).ToList())));

        (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                db.Rows.OrderBy(row => row.Name).Select(row => row.Name).ToListAsync()))
            .ShouldBe(new[] { "A-live" });

        (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, async db =>
            {
                Should.Throw<InvalidOperationException>(() => db.Rows.Where(row => row.Name.Length > countRows(db)).Select(row => row.Name).ToList());
                return await db.Rows.OrderBy(row => row.Name).Select(row => row.Name).ToListAsync();
            }))
            .ShouldBe(new[] { "A-live" });
    }

    [Fact]
    public async Task Should_Run_Compiled_Query_That_Ignores_Filters()
    {
        var countAllRows = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.Rows.IgnoreQueryFilters().Count());

        (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(new[] { countAllRows(db).ToString() }.ToList())))
            .ShouldBe(new[] { CompiledQueryCacheTestModuleBase.Rows.Length.ToString() });
    }

    [Fact]
    public void Should_Not_Allow_Replacing_Database()
    {
        var options = new DbContextOptionsBuilder<CompiledQueryCacheTestDbContext>()
            .UseSqlite("Data Source=:memory:")
            .ReplaceService<IDatabase, RelationalDatabase>()
            .AddAbpDbContextOptionsExtension()
            .Options;

        Should.Throw<AbpException>(() => new CompiledQueryCacheTestDbContext(options).Dispose());
    }

    [Fact]
    public async Task Should_Not_Compile_Query_Inside_Another_Query_Without_Filters()
    {
        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                Task.FromResult(db.Rows.Where(row => row.Name.Length > CompileRowsQuery(db)).Select(row => row.Name).ToList())));
        exception.InnerException.ShouldBeOfType<AbpException>();
    }

    private static int CompileRowsQuery(CompiledQueryCacheTestDbContext db)
    {
        db.GetService<IDatabase>().CompileQuery<IEnumerable<CompiledQueryCacheTestRow>>(((IQueryable<CompiledQueryCacheTestRow>)db.Rows).Expression, false);
        return 0;
    }

    [Fact]
    public async Task Should_Not_Run_Compiled_Async_Query_Without_Filters()
    {
        var query = EF.CompileAsyncQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Select(x => x.Name));

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => ToListAsync(query(db))));
    }
}
