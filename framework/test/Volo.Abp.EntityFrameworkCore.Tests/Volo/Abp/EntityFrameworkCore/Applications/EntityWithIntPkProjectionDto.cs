using Volo.Abp.Application.Dtos;

namespace Volo.Abp.EntityFrameworkCore.Applications;

public class EntityWithIntPkProjectionDto : EntityDto<int>
{
    public string Name { get; set; }
}
