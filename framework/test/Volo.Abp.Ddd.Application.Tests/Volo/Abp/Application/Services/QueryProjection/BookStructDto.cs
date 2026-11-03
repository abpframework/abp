using System;

namespace Volo.Abp.Application.Services.QueryProjection;

public struct BookStructDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}
