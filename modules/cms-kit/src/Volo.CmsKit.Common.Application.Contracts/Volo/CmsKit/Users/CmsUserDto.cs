using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Users;

[Serializable]
public class CmsUserDto : ExtensibleEntityDto<Guid>
{
    public virtual Guid? TenantId { get; set; }

    public virtual string UserName { get; set; }

    public virtual string Name { get; set; }

    public virtual string Surname { get; set; }
}
