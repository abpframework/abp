using System.Globalization;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.Client
{
    internal static class MvcCachedApplicationConfigurationClientHelper
    {
        public static string CreateCacheKey(ICurrentUser currentUser)
        {
            return $"ApplicationConfiguration_{currentUser.Id?.ToString("N") ?? "Anonymous"}_{CultureInfo.CurrentUICulture.Name}";
        }
    }
}
