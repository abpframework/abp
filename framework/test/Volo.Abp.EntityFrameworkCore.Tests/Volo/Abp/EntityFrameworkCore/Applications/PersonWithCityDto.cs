using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.EntityFrameworkCore.Applications;

public class PersonWithCityDto : EntityDto<Guid>
{
    public string Name { get; set; }

    public string CityName { get; set; }
}
