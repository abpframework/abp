using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity;

public class NullIdentitySessionValidationResultAccessor : IIdentitySessionValidationResultAccessor, ISingletonDependency
{
    public static NullIdentitySessionValidationResultAccessor Instance { get; } = new();

    public bool? GetOrNull(string sessionId)
    {
        return null;
    }

    public void Set(string sessionId, bool isValid)
    {
    }
}
