using System.Linq.Expressions;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Application.Services.Dtos;

namespace System.Linq
{
    /// <summary>
    /// Some useful extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class AbpQueryableExtensions
    {
        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static IQueryable<T> PageBy<T>([NotNull] this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            Check.NotNull(query, nameof(query));

            return query.Skip(skipCount).Take(maxResultCount);
        }

        /// <summary>
        /// Used for paging with an <see cref="IPagedResultRequest"/> object.
        /// </summary>
        /// <param name="query">Queryable to apply paging</param>
        /// <param name="pagedResultRequest">An object implements <see cref="IPagedResultRequest"/> interface</param>
        public static IQueryable<T> PageBy<T>([NotNull] this IQueryable<T> query, IPagedResultRequest pagedResultRequest)
        {
            Check.NotNull(query, nameof(query));

            return query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount);
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            Check.NotNull(query, nameof(query));

            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            Check.NotNull(query, nameof(query));

            return condition
                ? query.Where(predicate)
                : query;
        }
    }
}
