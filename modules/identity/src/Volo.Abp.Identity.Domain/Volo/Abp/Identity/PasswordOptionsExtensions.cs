using System;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity
{
    public static class PasswordOptionsExtensions
    {
        public static IDisposable ClearRequirements(this PasswordOptions options)
        {
            var oldRequireDigit = options.RequireDigit;
            var oldRequiredLength = options.RequiredLength;
            var oldRequireLowercase = options.RequireLowercase;
            var oldRequireUppercase = options.RequireUppercase;
            var oldRequiredUniqueChars = options.RequiredUniqueChars;
            var oldRequireNonAlphanumeric = options.RequireNonAlphanumeric;

            options.RequireDigit = false;
            options.RequiredLength = 1;
            options.RequireLowercase = false;
            options.RequireUppercase = false;
            options.RequiredUniqueChars = 1;
            options.RequireNonAlphanumeric = false;

            return new DisposeAction(() =>
            {
                options.RequireDigit = oldRequireDigit;
                options.RequiredLength = oldRequiredLength;
                options.RequireLowercase = oldRequireLowercase;
                options.RequireUppercase = oldRequireUppercase;
                options.RequiredUniqueChars = oldRequiredUniqueChars;
                options.RequireNonAlphanumeric = oldRequireNonAlphanumeric;
            });
        }
    }
}
