namespace Volo.Abp.Identity;

public interface IIdentitySessionValidationResultAccessor
{
    bool? GetOrNull(string sessionId);

    void Set(string sessionId, bool isValid);
}
