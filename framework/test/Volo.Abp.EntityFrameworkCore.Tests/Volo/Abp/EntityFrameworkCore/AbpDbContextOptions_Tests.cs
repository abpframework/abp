using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore;

public class AbpDbContextOptions_Tests : EntityFrameworkCoreTestBase
{
    private readonly List<(DbContext DbContext, string Name)> _executedCallbacks = new();

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddAbpDbContext<CallbackTestDbContext>();
        services.AddAbpDbContext<CallbackTestSecondDbContext>();

        services.Configure<AbpDbContextOptions>(options =>
        {
            options.ConfigureDefaultOnConfiguring((dbContext, _) => Record(dbContext, "DefaultOnConfiguring"));
            options.ConfigureDefaultConvention((dbContext, _) => Record(dbContext, "DefaultConventions"));
            options.ConfigureDefaultOnModelCreating((dbContext, _) => Record(dbContext, "DefaultOnModelCreating"));

            for (var i = 1; i <= 3; i++)
            {
                var index = i;
                options.ConfigureOnConfiguring<CallbackTestDbContext>((dbContext, _) => Record(dbContext, $"OnConfiguring{index}"));
                options.ConfigureConventions<CallbackTestDbContext>((dbContext, _) => Record(dbContext, $"Conventions{index}"));
                options.ConfigureOnModelCreating<CallbackTestDbContext>((dbContext, _) => Record(dbContext, $"OnModelCreating{index}"));
            }

            for (var i = 1; i <= 2; i++)
            {
                var index = i;
                options.ConfigureOnConfiguring<CallbackTestSecondDbContext>((dbContext, _) => Record(dbContext, $"SecondOnConfiguring{index}"));
                options.ConfigureConventions<CallbackTestSecondDbContext>((dbContext, _) => Record(dbContext, $"SecondConventions{index}"));
                options.ConfigureOnModelCreating<CallbackTestSecondDbContext>((dbContext, _) => Record(dbContext, $"SecondOnModelCreating{index}"));
            }
        });

        base.AfterAddApplication(services);
    }

    [Fact]
    public void Should_Execute_All_Typed_Callbacks_In_Registration_Order()
    {
        var dbContext = GetRequiredService<CallbackTestDbContext>();

        _ = dbContext.Model;

        GetExecutedCallbacks(dbContext).ShouldBe(new[]
        {
            "DefaultOnConfiguring",
            "OnConfiguring1",
            "OnConfiguring2",
            "OnConfiguring3",
            "DefaultConventions",
            "Conventions1",
            "Conventions2",
            "Conventions3",
            "DefaultOnModelCreating",
            "OnModelCreating1",
            "OnModelCreating2",
            "OnModelCreating3"
        });
    }

    [Fact]
    public void Should_Only_Execute_Typed_Callbacks_Of_The_Same_DbContext()
    {
        var dbContext = GetRequiredService<CallbackTestSecondDbContext>();

        _ = dbContext.Model;

        GetExecutedCallbacks(dbContext).ShouldBe(new[]
        {
            "DefaultOnConfiguring",
            "SecondOnConfiguring1",
            "SecondOnConfiguring2",
            "DefaultConventions",
            "SecondConventions1",
            "SecondConventions2",
            "DefaultOnModelCreating",
            "SecondOnModelCreating1",
            "SecondOnModelCreating2"
        });
    }

    private void Record(DbContext dbContext, string name)
    {
        lock (_executedCallbacks)
        {
            _executedCallbacks.Add((dbContext, name));
        }
    }

    private List<string> GetExecutedCallbacks(DbContext dbContext)
    {
        lock (_executedCallbacks)
        {
            return _executedCallbacks.Where(x => x.DbContext == dbContext).Select(x => x.Name).ToList();
        }
    }
}

public class CallbackTestDbContext : AbpDbContext<CallbackTestDbContext>
{
    public CallbackTestDbContext(DbContextOptions<CallbackTestDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ReplaceService<IModelCacheKeyFactory, UnitTestModelCacheKeyFactory>();
        base.OnConfiguring(optionsBuilder);
    }
}

public class CallbackTestSecondDbContext : AbpDbContext<CallbackTestSecondDbContext>
{
    public CallbackTestSecondDbContext(DbContextOptions<CallbackTestSecondDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ReplaceService<IModelCacheKeyFactory, UnitTestModelCacheKeyFactory>();
        base.OnConfiguring(optionsBuilder);
    }
}
