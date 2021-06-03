using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Menus
{
    public interface IMenuAdminAppService : ICrudAppService<MenuDto, Guid, PagedAndSortedResultRequestDto, MenuCreateInput>
    {

    }
}
