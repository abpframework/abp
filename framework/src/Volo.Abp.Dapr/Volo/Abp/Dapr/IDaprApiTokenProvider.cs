namespace Volo.Abp.Dapr;

public interface IDaprApiTokenProvider
{
    Task<string> GetAsync();
}