namespace Volo.Abp.EntityFrameworkCore.ConnectionStrings;

public class AbpConnectionStringCheckResult
{
    public bool Connected { get; set; }

    public bool DatabaseExists { get; set; }
}
