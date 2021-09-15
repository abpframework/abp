using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Data
{
    //TODO: Create a Volo.Abp.Data.Filtering namespace?
    public class DataFilter : IDataFilter, ISingletonDependency
    {
        public IReadOnlyDictionary<Type, IBasicDataFilter> ReadOnlyFilters => Filters;

        protected readonly ConcurrentDictionary<Type, IBasicDataFilter> Filters = new();

        protected readonly IServiceProvider ServiceProvider;

        public DataFilter(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public virtual IDisposable Enable<TFilter>()
            where TFilter : class
        {
            return GetOrAddFilter<TFilter>().Enable();
        }

        public virtual IDisposable Disable<TFilter>()
            where TFilter : class
        {
            return GetOrAddFilter<TFilter>().Disable();
        }

        public virtual bool IsActive<TFilter>()
            where TFilter : class
        {
            return GetOrAddFilter<TFilter>().IsActive;
        }

        public virtual bool IsActive(Type filterType)
        {
            return GetOrAddFilter(filterType).IsActive;
        }

        public virtual bool IsEnabled<TFilter>()
            where TFilter : class
        {
            return GetOrAddFilter<TFilter>().IsEnabled;
        }

        public virtual bool IsEnabled(Type filterType)
        {
            return GetOrAddFilter(filterType).IsEnabled;
        }

        public virtual IDataFilter<TFilter> GetOrAddFilter<TFilter>()
            where TFilter : class
        {
            return Filters.GetOrAdd(
                typeof(TFilter),
                () => ServiceProvider.GetRequiredService<IDataFilter<TFilter>>()
            ) as IDataFilter<TFilter>;
        }

        public virtual IBasicDataFilter GetOrAddFilter(Type filterType)
        {
            if (filterType == null
                // Should have no more than 1 interface type argument
                || filterType.GenericTypeArguments.Length > 1
                // Should be a generic filter interface e.g. ISoftDelete without unbound generic parameters e.g. ISoftDelete<>
                || (filterType.GenericTypeArguments.Length == 0 && (!filterType.IsInterface || filterType.ContainsGenericParameters))
                // Should be a filter interface with a concrete parameter e.g. Blog (filter == ISoftDelete<Blog>)
                || (filterType.GenericTypeArguments.Length == 1 && filterType.GenericTypeArguments[0].IsGenericType))
            {
                throw new AbpException($"The {nameof(filterType)} \"{(filterType == null ? "<null>" : filterType.GetFriendlyName())}\"" +
                    " is not a valid data filter type");
            }

            return Filters.GetOrAdd(
                filterType,
                (type) => ServiceProvider.GetRequiredService(
                    typeof(IDataFilter<>).MakeGenericType(type)) as IBasicDataFilter
            );
        }
    }

    public class DataFilter<TFilter> : IDataFilter<TFilter>
        where TFilter : class
    {
        public virtual bool IsActive => Filter.Value != null && Filter.Value.IsActive;

        public virtual bool IsEnabled
        {
            get
            {
                EnsureInitialized();
                return Filter.Value.IsEnabled;
            }
        }

        protected readonly AbpDataFilterOptions Options;

        protected readonly AsyncLocal<DataFilterState> Filter;

        public DataFilter(IOptions<AbpDataFilterOptions> options)
        {
            Options = options.Value;
            Filter = new AsyncLocal<DataFilterState>();
        }

        public virtual IDisposable Enable()
        {
            EnsureInitialized();

            if (!IsEnabled)
            {
                Filter.Value.IsEnabled = true;
            }

            if (IsActive)
            {
                return new DisposeAction(() => {
                    if (IsEnabled) Filter.Value.IsEnabled = false;
                });
            }
            else
            {
                Filter.Value.IsActive = true;

                return new DisposeAction(() => {
                    Filter.Value.IsActive = false;
                    if (IsEnabled) Filter.Value.IsEnabled = false;
                });
            }
        }

        public virtual IDisposable Disable()
        {
            EnsureInitialized();

            if (IsEnabled)
            {
                Filter.Value.IsEnabled = false;
            }

            if (IsActive)
            {
                return new DisposeAction(() => {
                    if (!IsEnabled) Filter.Value.IsEnabled = true;
                });
            }
            else
            {
                Filter.Value.IsActive = true;

                return new DisposeAction(() => {
                    Filter.Value.IsActive = false;
                    if (!IsEnabled) Filter.Value.IsEnabled = true;
                });
            }
        }

        protected virtual void EnsureInitialized()
        {
            if (Filter.Value != null)
            {
                return;
            }

            Filter.Value = Options.DefaultStates.GetOrDefault(typeof(TFilter))?.Clone()
                ?? new DataFilterState(Options.DefaultFilterState, false);
        }
    }
}