﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Auth;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Console;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;
using Volo.Abp.Cli.ProjectBuilding.Templates.Wpf;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class TemplateInfoProvider : ITemplateInfoProvider, ITransientDependency
    {
        public ILogger<TemplateInfoProvider> Logger { get; set; }

        public ICancellationTokenProvider CancellationTokenProvider { get; }
        public IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }
        public AuthService AuthService { get; }

        public TemplateInfoProvider(ICancellationTokenProvider cancellationTokenProvider,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            AuthService authService)
        {
            CancellationTokenProvider = cancellationTokenProvider;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            AuthService = authService;

            Logger = NullLogger<TemplateInfoProvider>.Instance;
        }

        public async Task<TemplateInfo> GetDefaultAsync()
        {
            var defaultTemplateName = await CheckProLicenseAsync() ? AppProTemplate.TemplateName : AppTemplate.TemplateName;

            return Get(defaultTemplateName);
        }

        public TemplateInfo Get(string name)
        {
            switch (name)
            {
                case AppTemplate.TemplateName:
                    return new AppTemplate();
                case AppProTemplate.TemplateName:
                    return new AppProTemplate();
                case ModuleTemplate.TemplateName:
                    return new ModuleTemplate();
                case ModuleProTemplate.TemplateName:
                    return new ModuleProTemplate();
                case ConsoleTemplate.TemplateName:
                    return new ConsoleTemplate();
                case WpfTemplate.TemplateName:
                    return new WpfTemplate();
                default:
                    throw new Exception("There is no template found with given name: " + name);
            }
        }


        private async Task<bool> CheckProLicenseAsync()
        {
            if (!AuthService.IsLoggedIn())
            {
                return false;
            }

            try
            {
                var url = $"{CliUrls.WwwAbpIo}api/license/check-user";

                using (var client = new CliHttpClient())
                {
                    var response = await client.GetHttpResponseMessageWithRetryAsync(
                        url,
                        CancellationTokenProvider.Token,
                        Logger);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"ERROR: Remote server returns '{response.StatusCode}'");
                    }

                    await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<bool>(responseContent);

                    return result;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
