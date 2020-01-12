using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Linq;
using System.Linq.Dynamic.Core;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application
{
    public class PagedAndSortedOperation : IPagedAndSortedOperation
    {
        protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public PagedAndSortedOperation()
        {
            AsyncQueryableExecuter = DefaultAsyncQueryableExecuter.Instance;
        }

        public virtual async Task<(int TotalCount, List<TEntity> Entities)> ListAsync<TEntity, TInput>(
            TInput input,
            Func<TInput, IQueryable<TEntity>> createFilteredQuery,
            Func<IQueryable<TEntity>, TInput, IQueryable<TEntity>> applySorting = null,
            Func<IQueryable<TEntity>, TInput, IQueryable<TEntity>> applyPaging = null)
            where TEntity : class, IEntity
        {
            if (createFilteredQuery == null) throw new Exception("createFilteredQuery must be not null");
            var query = createFilteredQuery.Invoke(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query).ConfigureAwait(false);

            query = applySorting != null ? applySorting.Invoke(query, input) : ApplySorting(query, input);
            query = applyPaging != null ? applyPaging.Invoke(query, input) : ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query).ConfigureAwait(false);

            return (TotalCount: totalCount, Entities: entities);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        public virtual IQueryable<TEntity> ApplySorting<TEntity, TInput>(IQueryable<TEntity> query, TInput input)
            where TEntity : class, IEntity
        {
            //Try to sort query if available
            if (input is ISortedResultRequest sortInput)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }

            //IQueryable.Take requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                if (typeof(TEntity).IsAssignableTo<IHasCreationTime>())
                {
                    return query.OrderBy($"{nameof(IHasCreationTime.CreationTime)} Desc");
                }
                if (Volo.Abp.Reflection.ReflectionHelper.IsAssignableToGenericType(typeof(TEntity), typeof(IEntity<>)))
                {
                    
                    return query.OrderBy($"{nameof(IEntity<Guid>.Id)} Desc");
                }
                
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        public virtual IQueryable<TEntity> ApplyPaging<TEntity, TInput>(IQueryable<TEntity> query, TInput input)
            where TEntity : class, IEntity
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }



    }
}
