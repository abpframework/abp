using System;

namespace Volo.Abp.Internal.Telemetry.Helpers;

internal class AbpProjectMetaData
{
    public Guid? ProjectId { get; set; }
    public string? Role { get; set; }
    public string? AbpSlnPath { get; set; }
}