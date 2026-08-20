using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookDto : EntityDto<Guid>
{
    public string Name { get; set; } = default!;
}
