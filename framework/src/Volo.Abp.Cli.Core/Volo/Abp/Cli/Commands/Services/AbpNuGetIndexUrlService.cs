using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Licensing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services
{
    public class AbpNuGetIndexUrlService : ITransientDependency
    {
        private readonly IApiKeyService _apiKeyService;
        public ILogger<AbpNuGetIndexUrlService> Logger { get; set; }

        public AbpNuGetIndexUrlService(IApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
            Logger = NullLogger<AbpNuGetIndexUrlService>.Instance;
        }

        public async Task<string> GetAsync()
        {
            var apiKeyResult = await _apiKeyService.GetApiKeyOrNullAsync();

            if (apiKeyResult == null)
            {
                Logger.LogWarning("You are not signed in! Use the CLI command \"abp login <username>\" to sign in, then try again.");
                return null;
            }

            if (!string.IsNullOrWhiteSpace(apiKeyResult.ErrorMessage))
            {
                Logger.LogWarning(apiKeyResult.ErrorMessage);
                return null;
            }

            if (string.IsNullOrEmpty(apiKeyResult.ApiKey))
            {
                Logger.LogError("Couldn't retrieve your NuGet API key! You can re-sign in with the CLI command \"abp login <username>\".");
                return null;
            }

            return CliUrls.GetNuGetServiceIndexUrl(apiKeyResult.ApiKey);
        }
    }
}
