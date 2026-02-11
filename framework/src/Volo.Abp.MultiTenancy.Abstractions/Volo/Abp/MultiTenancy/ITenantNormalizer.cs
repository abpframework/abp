namespace Volo.Abp.MultiTenancy;

public interface ITenantNormalizer
{
    string? NormalizeName(string? name);
}
