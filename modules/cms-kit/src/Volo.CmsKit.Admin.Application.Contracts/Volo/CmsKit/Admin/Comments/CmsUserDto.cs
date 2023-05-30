using System;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Admin.Comments;

[Serializable]
public class CmsUserDto : ExtensibleObject
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }
}
