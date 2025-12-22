using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions.Resources;

namespace MyCompanyName.MyProjectName.Books;

public class BookDto : AuditedEntityDto<Guid>, IHasResourcePermissions
{
    public string Name { get; set; }
    public string BookType { get; set; }
    public DateTime PublishDate { get; set; }
    public float Price { get; set; }
    public string Author { get; set; }
    public Dictionary<string, bool> ResourcePermissions { get; private set; } = new();
}
