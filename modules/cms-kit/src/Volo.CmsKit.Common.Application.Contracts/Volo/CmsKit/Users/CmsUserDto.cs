using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Users;

[Serializable]
public class CmsUserDto : EntityDto<Guid>
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual string UserName { get; protected set; }

    public virtual string Name { get; set; }

    public virtual string Surname { get; set; }
}
