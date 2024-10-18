namespace Volo.Abp.EntityFrameworkCore.GlobalFilters;

public class AbpEfCoreGlobalFilterOptions
{
    /// <summary>
    /// Use User-defined function mapping to filter data.
    /// https://learn.microsoft.com/en-us/ef/core/querying/user-defined-function-mapping
    /// </summary>
    public bool UseDbFunction { get; set; }
}
