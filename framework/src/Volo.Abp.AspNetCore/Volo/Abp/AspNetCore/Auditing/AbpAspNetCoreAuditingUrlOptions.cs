namespace Volo.Abp.AspNetCore.Auditing;

public class AbpAspNetCoreAuditingUrlOptions
{
    public bool IncludeSchema { get; set; }
    
    public bool IncludeHost { get; set; }
    
    public bool IncludeQuery { get; set; }
}