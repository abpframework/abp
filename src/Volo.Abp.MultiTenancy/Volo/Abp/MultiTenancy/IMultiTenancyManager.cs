namespace Volo.Abp.MultiTenancy
{
    public interface IMultiTenancyManager
    {
        ITenantInfo CurrentTenant { get; }
    }
}