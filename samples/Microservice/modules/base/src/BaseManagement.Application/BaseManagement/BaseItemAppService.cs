using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BaseManagement
{
    [Authorize(BaseManagementPermissions.BaseItems.Default)]
    public class BaseItemAppService : AsyncCrudAppService<BaseItem, BaseItemDto, Guid, BaseItemPagedRequestDto,
        CreateUpdateBaseItemDto, CreateUpdateBaseItemDto>, IBaseItemAppService
    {

        public BaseItemAppService(IRepository<BaseItem, Guid> repository)
            : base(repository)
        {
            base.CreatePolicyName = BaseManagementPermissions.BaseItems.Create;
            base.UpdatePolicyName = BaseManagementPermissions.BaseItems.Update;
            base.DeletePolicyName = BaseManagementPermissions.BaseItems.Delete;
        }

        protected override IQueryable<BaseItem> CreateFilteredQuery(BaseItemPagedRequestDto input)
        {
            return base.CreateFilteredQuery(input).WhereIf(input.BaseTypeGuid.HasValue,r=>r.BaseTypeGuid==input.BaseTypeGuid);
        }
    }
}