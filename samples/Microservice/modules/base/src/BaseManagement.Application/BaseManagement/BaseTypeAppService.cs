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
    [Authorize(BaseManagementPermissions.BaseTypes.Default)]
    public class BaseTypeAppService : AsyncCrudAppService<BaseType, BaseTypeDto, Guid, PagedAndSortedResultRequestDto,
        CreateBaseTypeDto, UpdateBaseTypeDto>, IBaseTypeAppService
    {

        public BaseTypeAppService(IRepository<BaseType, Guid> repository)
            : base(repository)
        {

        }

    }
}