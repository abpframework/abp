using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;

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
            return new AbpCompiledQueryCacheKey(cacheKey, abpEfCoreDbFunctionContext.GetCompiledQueryCacheKey());
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
