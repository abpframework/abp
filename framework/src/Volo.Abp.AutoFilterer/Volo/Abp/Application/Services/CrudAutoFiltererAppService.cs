using AutoFilterer.Abstractions;
using AutoFilterer.Extensions;
using System.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoFilterer;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services
{
    public abstract class CrudAutoFiltererAppService<TEntity, TEntityDto, TKey>
        : CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, AbpPaginationFilterBase>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected CrudAutoFiltererAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }

    public abstract class CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput>
        : CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
        where TGetListInput : IFilter
    {
        protected CrudAutoFiltererAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }

    public abstract class CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>
        : CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
        where TGetListInput : IFilter
    {
        protected CrudAutoFiltererAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }

    public abstract class CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        : CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
        where TGetListInput : IFilter
    {
        protected CrudAutoFiltererAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }

        protected override IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetListInput input)
        {
            if (input is IPaginationFilter filter)
                return query.ToPaged(filter.Page, filter.PerPage);

            return base.ApplyPaging(query, input);
        }

        protected override IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetListInput input)
        {
            if (input is IOrderable orderableFilter)
                return orderableFilter.ApplyOrder(query);

            return base.ApplySorting(query, input);
        }

        protected override IQueryable<TEntity> CreateFilteredQuery(TGetListInput input)
        {
            var query = base.CreateFilteredQuery(input);

            if (input is IOrderablePaginationFilter fullFilter)
            {
                return fullFilter.ApplyFilterWithoutPaginationAndOrdering(query);
            }
            else if (input is IFilter filter)
            {
                return filter.ApplyFilterTo(query);
            }

            return query;
        }
    }
}
