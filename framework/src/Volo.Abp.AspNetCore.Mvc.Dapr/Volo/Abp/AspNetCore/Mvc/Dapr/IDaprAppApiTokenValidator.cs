namespace Volo.Abp.AspNetCore.Mvc.Dapr;

public interface IDaprAppApiTokenValidator
{
    void CheckDaprAppApiToken();

    bool IsValidDaprAppApiToken();

    string? GetDaprAppApiTokenOrNull();
}
