using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.EntityFrameworkCore.Applications;

public class PersonProjectionDto : EntityDto<Guid>
{
    public string Name { get; set; }
}
