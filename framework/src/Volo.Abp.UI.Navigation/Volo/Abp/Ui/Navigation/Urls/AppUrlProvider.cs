using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Ui.Navigation.Urls
{
    public class AppUrlProvider : IAppUrlProvider, ITransientDependency
    {
        protected AppUrlOptions Options { get; }

        public AppUrlProvider(IOptions<AppUrlOptions> options)
        {
            Options = options.Value;
        }

        public virtual string GetUrl(string appName, string urlName = null)
        {
            var app = Options.Applications[appName];

            if (urlName.IsNullOrEmpty())
            {
                if (app.RootUrl.IsNullOrEmpty())
                {
                    throw new AbpException($"RootUrl for the application '{appName}' was not configured. Use {nameof(AppUrlOptions)} to configure it!");
                }

                return app.RootUrl;
            }

            var url = app.Urls.GetOrDefault(urlName);
            if (url.IsNullOrEmpty())
            {
                throw new AbpException($"Url, named '{urlName}', for the application '{appName}' was not configured. Use {nameof(AppUrlOptions)} to configure it!");
            }

            if (app.RootUrl == null)
            {
                return url;
            }

            return app.RootUrl.EnsureEndsWith('/') + url;
        }
    }
}