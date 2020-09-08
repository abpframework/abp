using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Text.Formatting;

namespace Microsoft.AspNetCore.Identity
{
    public static class AbpIdentityResultExtensions
    {
        public static void CheckErrors(this IdentityResult identityResult)
        {
            if (identityResult.Succeeded)
            {
                return;
            }

            if (identityResult.Errors == null)
            {
                throw new ArgumentException("identityResult.Errors should not be null.");
            }

            throw new AbpIdentityResultException(identityResult);
        }

        public static string[] GetValuesFromErrorMessage(this IdentityResult identityResult, IStringLocalizer localizer)
        {
            if (identityResult.Succeeded)
            {
                throw new ArgumentException(
                    "identityResult.Succeeded should be false in order to get values from error.");
            }

            if (identityResult.Errors == null)
            {
                throw new ArgumentException("identityResult.Errors should not be null.");
            }

            var error = identityResult.Errors.First();
            var key = $"Volo.Abp.Identity:{error.Code}";

            using (CultureHelper.Use(CultureInfo.GetCultureInfo("en")))
            {
                var englishLocalizedString = localizer[key];

                if (englishLocalizedString.ResourceNotFound)
                {
                    return Array.Empty<string>();
                }

                if (FormattedStringValueExtracter.IsMatch(error.Description, englishLocalizedString.Value,
                    out var values))
                {
                    return values;
                }

                return Array.Empty<string>();
            }
        }

        public static string LocalizeErrors(this IdentityResult identityResult, IStringLocalizer localizer)
        {
            if (identityResult.Succeeded)
            {
                throw new ArgumentException("identityResult.Succeeded should be false in order to localize errors.");
            }

            if (identityResult.Errors == null)
            {
                throw new ArgumentException("identityResult.Errors should not be null.");
            }

            return identityResult.Errors.Select(err => LocalizeErrorMessage(err, localizer)).JoinAsString(", ");
        }

        public static string LocalizeErrorMessage(this IdentityError error, IStringLocalizer localizer)
        {
            var key = $"Volo.Abp.Identity:{error.Code}";

            var localizedString = localizer[key];

            if (!localizedString.ResourceNotFound)
            {
                using (CultureHelper.Use(CultureInfo.GetCultureInfo("en")))
                {
                    var englishLocalizedString = localizer[key];
                    if (!englishLocalizedString.ResourceNotFound)
                    {
                        if (FormattedStringValueExtracter.IsMatch(error.Description, englishLocalizedString.Value,
                            out var values))
                        {
                            return string.Format(localizedString.Value, values.Cast<object>().ToArray());
                        }
                    }
                }
            }

            return localizer["Identity.Default"];
        }

        public static string GetResultAsString(this SignInResult signInResult)
        {
            if (signInResult.Succeeded)
            {
                return "Succeeded";
            }

            if (signInResult.IsLockedOut)
            {
                return "IsLockedOut";
            }

            if (signInResult.IsNotAllowed)
            {
                return "IsNotAllowed";
            }

            if (signInResult.RequiresTwoFactor)
            {
                return "RequiresTwoFactor";
            }

            return "Unknown";
        }
    }
}
