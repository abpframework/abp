using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpApplicationBuilderExtensions
    {
        private const string ExceptionHandlingMiddlewareMarker = "_AbpExceptionHandlingMiddleware_Added";

        public static void InitializeApplication([NotNull] this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
            app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>().Initialize(app.ApplicationServices);
        }

        public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<AbpAuditingMiddleware>();
        }

        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
        {
            return app
                .UseAbpExceptionHandling()
                .UseMiddleware<AbpUnitOfWorkMiddleware>();
        }

        public static IApplicationBuilder UseAbpRequestLocalization(this IApplicationBuilder app)
        {
            IReadOnlyList<LanguageInfo> languages;
            string defaultLanguage;

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var languageProvider = scope.ServiceProvider.GetRequiredService<ILanguageProvider>();
                var settingManager = scope.ServiceProvider.GetRequiredService<ISettingManager>();

                languages = AsyncHelper.RunSync(() => languageProvider.GetLanguagesAsync());
                defaultLanguage = settingManager.GetOrNull(LocalizationSettingNames.DefaultLanguage);
            }

            if (!languages.Any())
            {
                return app.UseRequestLocalization();
            }

            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = DefaultGetRequestCulture(defaultLanguage, languages),

                SupportedCultures = languages
                    .Select(l => l.CultureName)
                    .Distinct()
                    .Select(c => new CultureInfo(c))
                    .ToArray(),

                SupportedUICultures = languages
                    .Select(l => l.UiCultureName)
                    .Distinct()
                    .Select(c => new CultureInfo(c))
                    .ToArray()
            };
            
            return app.UseRequestLocalization(options);
        }

        public static IApplicationBuilder UseAbpExceptionHandling(this IApplicationBuilder app)
        {
            if (app.Properties.ContainsKey(ExceptionHandlingMiddlewareMarker))
            {
                return app;
            }

            app.Properties[ExceptionHandlingMiddlewareMarker] = true;
            return app.UseMiddleware<AbpExceptionHandlingMiddleware>();
        }

        private static RequestCulture DefaultGetRequestCulture(string defaultLanguage, IReadOnlyList<LanguageInfo> languages)
        {
            if (defaultLanguage == null)
            {
                var firstLanguage = languages.First();
                return new RequestCulture(firstLanguage.CultureName, firstLanguage.UiCultureName);
            }

            if (!defaultLanguage.Contains(";"))
            {
                return new RequestCulture(defaultLanguage, defaultLanguage);
            }

            var splitted = defaultLanguage.Split(';');
            return new RequestCulture(splitted[0], splitted[1]);

        }
    }
}
