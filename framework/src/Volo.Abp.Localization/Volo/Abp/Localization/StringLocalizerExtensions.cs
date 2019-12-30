using System;
using System.Globalization;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization
{
    public static class StringLocalizerExtensions
    {
        /// <summary>
        /// Change the CurrentCulture and CurrentUICulture of CultureInfo.
        /// If uiCulture is null, CultureInfo.CurrentUICulture will use culture.
        /// </summary>
        public static IDisposable Change(this IStringLocalizer localizer, CultureInfo culture, [CanBeNull]CultureInfo uiCulture = null)
        {
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = uiCulture ?? culture;

            return new DisposeAction(() =>
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            });
        }
    }
}