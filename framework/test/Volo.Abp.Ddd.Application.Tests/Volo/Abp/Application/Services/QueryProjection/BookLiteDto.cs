using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookLiteDto : EntityDto<Guid>
{
    public string Name { get; set; } = default!;
}
