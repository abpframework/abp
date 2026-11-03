using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;

namespace Volo.Abp.EntityFrameworkCore;

public class AbpDbContextOptionsExtension : IDbContextOptionsExtension
{
    public void ApplyServices(IServiceCollection services)
    {
        services.Decorate<ICompiledQueryCacheKeyGenerator>((provider, inner) =>
            ActivatorUtilities.CreateInstance<AbpCompiledQueryCacheKeyGenerator>(provider, inner));

        // The user-defined functions are only translated by relational providers, and others, such as InMemory, cast IDatabase to their own type.
        if (services.IsAdded<IRelationalConnection>())
        {
            services.Decorate<IDatabase>((provider, inner) =>
                ActivatorUtilities.CreateInstance<AbpEfCoreDatabase>(provider, inner));
        }

        services.Replace(ServiceDescriptor.Scoped<IAsyncQueryProvider, AbpEntityQueryProvider>());
        services.TryAddSingleton<AbpEfCoreCurrentDbContext>();
    }

    public void Validate(IDbContextOptions options)
    {
        // EF Core applies ReplaceService after ApplyServices, so it would silently remove the wrappers.
        var replacedServices = options.FindExtension<CoreOptionsExtension>()?.ReplacedServices;
        if (replacedServices != null && replacedServices.Keys.Any(x => x.Item1 == typeof(ICompiledQueryCacheKeyGenerator) || x.Item1 == typeof(IDatabase)))
        {
            throw new AbpException(
                "ICompiledQueryCacheKeyGenerator and IDatabase cannot be replaced with ReplaceService while AbpDbContextOptionsExtension is used, " +
                "because ABP wraps them to apply the data filters. Remove the ReplaceService call, or set 'UseDbFunction' of 'AbpEfCoreGlobalFilterOptions' to false " +
                "and don't add AbpDbContextOptionsExtension yourself.");
        }
    }

    public DbContextOptionsExtensionInfo Info => new AbpOptionsExtensionInfo(this);

    private class AbpOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public AbpOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override bool IsDatabaseProvider => false;

        public override int GetServiceProviderHashCode()
        {
            return 0;
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is AbpOptionsExtensionInfo;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override string LogFragment => "AbpOptionsExtension";
    }
}
