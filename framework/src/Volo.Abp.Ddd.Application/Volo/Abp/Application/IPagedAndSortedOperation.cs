using System;
using System.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Linq;

namespace Volo.Abp.Application
{
    public interface IPagedAndSortedOperation: Volo.Abp.DependencyInjection.ITransientDependency
    {
        System.Threading.Tasks.Task<(int TotalCount, System.Collections.Generic.List<TEntity> Entities)> ListAsync<TEntity, TInput>(
            TInput input,
            System.Func<TInput, System.Linq.IQueryable<TEntity>> createFilteredQuery,
            Func<IQueryable<TEntity>, TInput, IQueryable<TEntity>> applySorting = null,
            Func<IQueryable<TEntity>, TInput, IQueryable<TEntity>> applyPaging = null)
            where TEntity : class, IEntity;

        IQueryable<TEntity> ApplySorting<TEntity, TInput>(IQueryable<TEntity> query, TInput input)
            where TEntity : class, IEntity;

        IQueryable<TEntity> ApplyPaging<TEntity, TInput>(IQueryable<TEntity> query, TInput input)
            where TEntity : class, IEntity;
    }
}