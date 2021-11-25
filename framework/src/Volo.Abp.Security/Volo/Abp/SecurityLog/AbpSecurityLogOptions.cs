namespace Volo.Abp.SecurityLog;

public class AbpSecurityLogOptions
{
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// The name of the application or service writing security log.
    /// Default: null.
    /// </summary>
    public string ApplicationName { get; set; }

    public AbpSecurityLogOptions()
    {
        IsEnabled = true;
    }
}
