using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Linq;
using Volo.Abp.ObjectMapping;

namespace System.Linq
{
    public static class AbpPagingQueryableExtensions
    {
        /// <summary>
        /// Used for paging with an <see cref="IPagedResultRequest"/> object.
        /// </summary>
        /// <param name="query">Queryable to apply paging</param>
        /// <param name="pagedResultRequest">An object implements <see cref="IPagedResultRequest"/> interface</param>
        public static IQueryable<T> PageBy<T>([NotNull] this IQueryable<T> query,
            IPagedResultRequest pagedResultRequest)
        {
            Check.NotNull(query, nameof(query));

            return query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount);
        }

        public static IPagedResult<T> PageResult<T>([NotNull] this IQueryable<T> query,
            IPagedResultRequest pagedResultRequest)
        {
            Check.NotNull(query, nameof(query));

            List<T> items = query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount).ToList();

            int totalCount = query.Count();

            return new PagedResultDto<T>(totalCount, items);
        }

        public static IPagedResult<U> PageResult<T, U>([NotNull] this IQueryable<T> query,
            [NotNull] IObjectMapper objectMapper,
            IPagedResultRequest pagedResultRequest)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(objectMapper, nameof(objectMapper));

            List<T> items = query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount).ToList();

            int totalCount = query.Count();

            return new PagedResultDto<U>(totalCount, objectMapper.Map<IReadOnlyList<T>, IReadOnlyList<U>>(items));
        }

        public static IPagedResult<T> PageResult<T>([NotNull] this IQueryable<T> query,
            IPagedAndSortedResultRequest pagedAndSortedResultRequest)
        {
            Check.NotNull(query, nameof(query));

            List<T> items = query.PageBy(pagedAndSortedResultRequest.SkipCount, pagedAndSortedResultRequest.MaxResultCount)
                                 .OrderBy(pagedAndSortedResultRequest.Sorting)
                                 .ToList();

            int totalCount = query.Count();

            return new PagedResultDto<T>(totalCount, items);
        }

        public static IPagedResult<U> PageResult<T, U>([NotNull] this IQueryable<T> query,
            [NotNull] IObjectMapper objectMapper,
            IPagedAndSortedResultRequest pagedAndSortedResultRequest)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(objectMapper, nameof(objectMapper));

            List<T> items = query.PageBy(pagedAndSortedResultRequest.SkipCount, pagedAndSortedResultRequest.MaxResultCount)
                                 .OrderBy(pagedAndSortedResultRequest.Sorting)
                                 .ToList();

            int totalCount = query.Count();

            return new PagedResultDto<U>(totalCount, objectMapper.Map<IReadOnlyList<T>, IReadOnlyList<U>>(items));
        }

        public static async Task<IPagedResult<T>> PageResultAsync<T>([NotNull] this IQueryable<T> query,
            [NotNull] IAsyncQueryableExecuter asyncExecuter,
            IPagedResultRequest pagedResultRequest)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(asyncExecuter, nameof(asyncExecuter));

            List<T> items = await asyncExecuter.ToListAsync(query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount));

            int totalCount = await asyncExecuter.CountAsync(query);

            return new PagedResultDto<T>(totalCount, items);
        }

        public static async Task<IPagedResult<U>> PageResultAsync<T, U>([NotNull] this IQueryable<T> query,
            [NotNull] IAsyncQueryableExecuter asyncExecuter,
            [NotNull] IObjectMapper objectMapper,
            IPagedResultRequest pagedResultRequest)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(asyncExecuter, nameof(asyncExecuter));
            Check.NotNull(objectMapper, nameof(objectMapper));

            List<T> items = await asyncExecuter.ToListAsync(query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount));

            int totalCount = await asyncExecuter.CountAsync(query);

            return new PagedResultDto<U>(totalCount, objectMapper.Map<IReadOnlyList<T>, IReadOnlyList<U>>(items));
        }

        public static async Task<IPagedResult<T>> PageResultAsync<T>([NotNull] this IQueryable<T> query,
            [NotNull] IAsyncQueryableExecuter asyncExecuter,
            IPagedAndSortedResultRequest pagedAndSortedResultRequest)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(asyncExecuter, nameof(asyncExecuter));

            List<T> items = await asyncExecuter.ToListAsync(query.PageBy(pagedAndSortedResultRequest.SkipCount, pagedAndSortedResultRequest.MaxResultCount)
                                                                 .OrderBy(pagedAndSortedResultRequest.Sorting));

            var totalCount = await asyncExecuter.CountAsync(query);

            return new PagedResultDto<T>(totalCount, items);
        }

        public static async Task<IPagedResult<U>> PageResultAsync<T, U>([NotNull] this IQueryable<T> query,
            [NotNull] IAsyncQueryableExecuter asyncExecuter,
            [NotNull] IObjectMapper objectMapper,
            IPagedAndSortedResultRequest pagedAndSortedResultRequest)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(asyncExecuter, nameof(asyncExecuter));
            Check.NotNull(objectMapper, nameof(objectMapper));

            List<T> items = await asyncExecuter.ToListAsync(query.PageBy(pagedAndSortedResultRequest.SkipCount, pagedAndSortedResultRequest.MaxResultCount)
                                                                 .OrderBy(pagedAndSortedResultRequest.Sorting));

            int totalCount = await asyncExecuter.CountAsync(query);

            return new PagedResultDto<U>(totalCount, objectMapper.Map<IReadOnlyList<T>, IReadOnlyList<U>>(items));
        }

        public static IDetailedPagedResult<T> PageResult<T>([NotNull] this IQueryable<T> query,
            IPagedResultRequestByPage pagedResultRequestByPage)
        {
            Check.NotNull(query, nameof(query));

            PagedResult<T> pageResult = query.PageResult(pagedResultRequestByPage.Page, pagedResultRequestByPage.MaxResultCount);

            List<T> items = pageResult.Queryable.ToList();

            return new DetailedPagedResultDto<T>(pageResult.CurrentPage,
                pageResult.PageCount,
                pageResult.PageSize,
                pageResult.RowCount,
                items);
        }

        public static IDetailedPagedResult<U> PageResult<T, U>([NotNull] this IQueryable<T> query,
            [NotNull] IObjectMapper objectMapper, IPagedResultRequestByPage pagedResultRequestByPage)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(objectMapper, nameof(objectMapper));

            PagedResult<T> pageResult = query.PageResult(pagedResultRequestByPage.Page, pagedResultRequestByPage.MaxResultCount);

            List<T> items = pageResult.Queryable.ToList();

            return new DetailedPagedResultDto<U>(pageResult.CurrentPage,
                pageResult.PageCount,
                pageResult.PageSize,
                pageResult.RowCount,
                objectMapper.Map<IReadOnlyList<T>, IReadOnlyList<U>>(items));
        }

        public static IDetailedPagedResult<T> PageResult<T>([NotNull] this IQueryable<T> query,
            IPagedAndSortedResultRequestByPage pagedAndSortedResultRequestByPage)
        {
            Check.NotNull(query, nameof(query));

            PagedResult<T> pageResult = query.PageResult(pagedAndSortedResultRequestByPage.Page, pagedAndSortedResultRequestByPage.MaxResultCount);

            List<T> items = pageResult.Queryable.OrderBy(pagedAndSortedResultRequestByPage.Sorting).ToList();

            return new DetailedPagedResultDto<T>(pageResult.CurrentPage,
                pageResult.PageCount,
                pageResult.PageSize,
                pageResult.RowCount,
                items);
        }

        public static IDetailedPagedResult<U> PageResult<T, U>([NotNull] this IQueryable<T> query,
            [NotNull] IObjectMapper objectMapper,
            IPagedAndSortedResultRequestByPage pagedAndSortedResultRequestByPage)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(objectMapper, nameof(objectMapper));

            PagedResult<T> pageResult = query.PageResult(pagedAndSortedResultRequestByPage.Page, pagedAndSortedResultRequestByPage.MaxResultCount);

            List<T> items = pageResult.Queryable.OrderBy(pagedAndSortedResultRequestByPage.Sorting).ToList();

            return new DetailedPagedResultDto<U>(pageResult.CurrentPage,
                pageResult.PageCount,
                pageResult.PageSize,
                pageResult.RowCount,
                objectMapper.Map<IReadOnlyList<T>, IReadOnlyList<U>>(items));
        }

        public static async Task<IDetailedPagedResult<T>> PageResultAsync<T>([NotNull] this IQueryable<T> query,
            [NotNull] IAsyncQueryableExecuter asyncExecuter,
            IPagedResultRequestByPage pagedResultRequestByPage)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(asyncExecuter, nameof(asyncExecuter));

            int totalCount = await asyncExecuter.CountAsync(query);

            PagedResult<T> pageResult = query.PageResult(pagedResultRequestByPage.Page, pagedResultRequestByPage.MaxResultCount, totalCount);

            List<T> items = await asyncExecuter.ToListAsync(pageResult.Queryable);

            return new DetailedPagedResultDto<T>(pageResult.CurrentPage,
                pageResult.PageCount,
                pageResult.PageSize,
                pageResult.RowCount,
                items);
        }

        public static async Task<IDetailedPagedResult<U>> PageResultAsync<T, U>([NotNull] this IQueryable<T> query,
            [NotNull] IAsyncQueryableExecuter asyncExecuter,
            [NotNull] IObjectMapper objectMapper,
            IPagedResultRequestByPage pagedResultRequestByPage)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(asyncExecuter, nameof(asyncExecuter));
            Check.NotNull(objectMapper, nameof(objectMapper));

            int totalCount = await asyncExecuter.CountAsync(query);

            PagedResult<T> pageResult = query.PageResult(pagedResultRequestByPage.Page, pagedResultRequestByPage.MaxResultCount, totalCount);

            List<T> items = await asyncExecuter.ToListAsync(pageResult.Queryable);

            return new DetailedPagedResultDto<U>(pageResult.CurrentPage,
                pageResult.PageCount,
                pageResult.PageSize,
                pageResult.RowCount,
                objectMapper.Map<IReadOnlyList<T>, IReadOnlyList<U>>(items));
        }

        public static async Task<IDetailedPagedResult<T>> PageResultAsync<T>([NotNull] this IQueryable<T> query,
            [NotNull] IAsyncQueryableExecuter asyncExecuter,
            IPagedAndSortedResultRequestByPage pagedAndSortedResultRequestByPage)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(asyncExecuter, nameof(asyncExecuter));

            int totalCount = await asyncExecuter.CountAsync(query);

            PagedResult<T> pageResult = query.PageResult(pagedAndSortedResultRequestByPage.Page, pagedAndSortedResultRequestByPage.MaxResultCount, totalCount);

            List<T> items = await asyncExecuter.ToListAsync(pageResult.Queryable.OrderBy(pagedAndSortedResultRequestByPage.Sorting));

            return new DetailedPagedResultDto<T>(pageResult.CurrentPage,
                pageResult.PageCount,
                pageResult.PageSize,
                pageResult.RowCount,
                items);
        }

        public static async Task<IDetailedPagedResult<U>> PageResultAsync<T, U>([NotNull] this IQueryable<T> query,
            [NotNull] IAsyncQueryableExecuter asyncExecuter,
            [NotNull] IObjectMapper objectMapper,
            IPagedAndSortedResultRequestByPage pagedAndSortedResultRequestByPage)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(asyncExecuter, nameof(asyncExecuter));
            Check.NotNull(objectMapper, nameof(objectMapper));

            int totalCount = await asyncExecuter.CountAsync(query);

            PagedResult<T> pageResult = query.PageResult(pagedAndSortedResultRequestByPage.Page, pagedAndSortedResultRequestByPage.MaxResultCount, totalCount);

            List<T> items = await asyncExecuter.ToListAsync(pageResult.Queryable.OrderBy(pagedAndSortedResultRequestByPage.Sorting));

            return new DetailedPagedResultDto<U>(pageResult.CurrentPage,
                pageResult.PageCount,
                pageResult.PageSize,
                pageResult.RowCount,
                objectMapper.Map<IReadOnlyList<T>, IReadOnlyList<U>>(items));
        }
    }
}
