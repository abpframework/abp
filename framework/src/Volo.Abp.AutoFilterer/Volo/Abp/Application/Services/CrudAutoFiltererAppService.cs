using AutoFilterer.Abstractions;
using AutoFilterer.Extensions;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services
{
    public class CrudAutoFiltererAppService<TEntity, TEntityDto, TKey>
        : CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, PaginationFilterBase>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected CrudAutoFiltererAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }

    public class CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput>
        : CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
        where TGetListInput : IFilter
    {
        protected CrudAutoFiltererAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }

    public class CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>
        : CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
        where TGetListInput : IFilter
    {
        protected CrudAutoFiltererAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }

    public class CrudAutoFiltererAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        : CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
        where TGetListInput : IFilter
    {
        protected CrudAutoFiltererAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }

        protected override IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetListInput input) 
            => query;

        protected override IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetListInput input) 
            => query;

        protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
            => query;

        protected override IQueryable<TEntity> CreateFilteredQuery(TGetListInput input) 
            => base.CreateFilteredQuery(input).ApplyFilter(input);
    }
}
