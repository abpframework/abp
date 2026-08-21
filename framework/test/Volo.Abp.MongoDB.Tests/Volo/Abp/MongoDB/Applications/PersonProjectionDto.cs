using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.MongoDB.Applications;

public class PersonProjectionDto : EntityDto<Guid>
{
    public string Name { get; set; }
}
