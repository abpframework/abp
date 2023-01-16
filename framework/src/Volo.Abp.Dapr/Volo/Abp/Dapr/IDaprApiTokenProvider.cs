namespace Volo.Abp.Dapr;

public interface IDaprApiTokenProvider
{
    string GetDaprApiToken();

    string GetAppApiToken();
}
