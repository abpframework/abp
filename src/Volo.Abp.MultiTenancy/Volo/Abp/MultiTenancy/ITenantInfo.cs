namespace Volo.Abp.MultiTenancy
{
    public interface ITenantInfo
    {
        string Id { get; }

        string Name { get; }
    }
}