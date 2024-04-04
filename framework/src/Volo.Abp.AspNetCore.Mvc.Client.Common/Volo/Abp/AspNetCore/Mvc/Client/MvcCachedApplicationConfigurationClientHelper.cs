using System.Globalization;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public static class MvcCachedApplicationConfigurationClientHelper
{
    public static string CreateCacheKey(ICurrentUser currentUser)
    {
        var userKey = currentUser.Id?.ToString("N") ?? "Anonymous";
        return $"ApplicationConfiguration_{userKey}_{CultureInfo.CurrentUICulture.Name}";
    }
}
