using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.GlobalFilters;

public class AbpCompiledQueryCacheKeyGenerator : ICompiledQueryCacheKeyGenerator
{
    protected ICompiledQueryCacheKeyGenerator InnerCompiledQueryCacheKeyGenerator { get; }
    protected ICurrentDbContext CurrentContext { get; }

    public AbpCompiledQueryCacheKeyGenerator(
        ICompiledQueryCacheKeyGenerator innerCompiledQueryCacheKeyGenerator,
        ICurrentDbContext currentContext)
    {
        InnerCompiledQueryCacheKeyGenerator = innerCompiledQueryCacheKeyGenerator;
        CurrentContext = currentContext;
    }

    public virtual object GenerateCacheKey(Expression query, bool async)
    {
        var cacheKey = InnerCompiledQueryCacheKeyGenerator.GenerateCacheKey(query, async);
        if (CurrentContext.Context is IAbpEfCoreDbFunctionContext abpEfCoreDbFunctionContext)
        {
            var abpCacheKey = abpEfCoreDbFunctionContext.GetCompiledQueryCacheKey();
            var cacheKeyProviders = abpEfCoreDbFunctionContext.LazyServiceProvider.GetService<IEnumerable<IAbpEfCoreCompiledQueryCacheKeyProvider>>();
            if (cacheKeyProviders != null)
            {
                foreach (var provider in cacheKeyProviders)
                {
                    var key = provider.GetCompiledQueryCacheKey();
                    if (!key.IsNullOrWhiteSpace())
                    {
                        abpCacheKey += $":{key}";
                    }
                }
            }

            return new AbpCompiledQueryCacheKey(cacheKey, abpCacheKey);
        }

        return cacheKey;
    }

    private readonly struct AbpCompiledQueryCacheKey : IEquatable<AbpCompiledQueryCacheKey>
    {
        private readonly object _compiledQueryCacheKey;
        private readonly string _currentFilterCacheKey;

        public AbpCompiledQueryCacheKey(object compiledQueryCacheKey, string currentFilterCacheKey)
        {
            _compiledQueryCacheKey = compiledQueryCacheKey;
            _currentFilterCacheKey = currentFilterCacheKey;
        }

        public override bool Equals(object? obj)
        {
            return obj is AbpCompiledQueryCacheKey key && Equals(key);
        }

        public bool Equals(AbpCompiledQueryCacheKey other)
        {
            return _compiledQueryCacheKey.Equals(other._compiledQueryCacheKey) &&
                   _currentFilterCacheKey == other._currentFilterCacheKey;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_compiledQueryCacheKey, _currentFilterCacheKey);
        }
    }
}
