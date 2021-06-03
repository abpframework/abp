using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Menus
{
    public class MenuDto : FullAuditedEntityDto<Guid>
    {
        public List<MenuItemDto> Items { get; set; }
    }
}
