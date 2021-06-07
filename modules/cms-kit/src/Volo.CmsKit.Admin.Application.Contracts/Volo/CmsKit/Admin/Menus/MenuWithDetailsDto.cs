using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Menus
{
    public class MenuWithDetailsDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public List<MenuItemDto> Items { get; set; }
    }
}
