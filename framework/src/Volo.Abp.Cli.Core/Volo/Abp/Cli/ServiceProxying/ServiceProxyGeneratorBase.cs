using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ServiceProxying;

public abstract class ServiceProxyGeneratorBase<T> : IServiceProxyGenerator where T : IServiceProxyGenerator
{
    public IJsonSerializer JsonSerializer { get; }

    public CliHttpClientFactory CliHttpClientFactory { get; }

    public ILogger<T> Logger { get; set; }

    protected ServiceProxyGeneratorBase(CliHttpClientFactory cliHttpClientFactory, IJsonSerializer jsonSerializer)
    {
        CliHttpClientFactory = cliHttpClientFactory;
        JsonSerializer = jsonSerializer;
        Logger = NullLogger<T>.Instance;
    }

    public abstract Task GenerateProxyAsync(GenerateProxyArgs args);

    protected virtual async Task<ApplicationApiDescriptionModel> GetApplicationApiDescriptionModelAsync(GenerateProxyArgs args)
    {
        Check.NotNull(args.Url, nameof(args.Url));

        var client = CliHttpClientFactory.CreateClient();

        var apiDefinitionResult = await client.GetStringAsync(CliUrls.GetApiDefinitionUrl(args.Url));
        var apiDefinition = JsonSerializer.Deserialize<ApplicationApiDescriptionModel>(apiDefinitionResult);

        var moduleDefinition = apiDefinition.Modules.FirstOrDefault(x => string.Equals(x.Key, args.Module, StringComparison.CurrentCultureIgnoreCase)).Value;
        if (moduleDefinition == null)
        {
            throw new CliUsageException($"Module name: {args.Module} is invalid");
        }

        var serviceType = args.ServiceType ?? GetDefaultServiceType(args);

        switch (serviceType)
        {
            case ServiceType.Application:
                moduleDefinition.Controllers.RemoveAll(x => !x.Value.IsRemoteService);
                break;
            case ServiceType.Integration:
                moduleDefinition.Controllers.RemoveAll(x => !x.Value.IsIntegrationService);
                break;
        }

        var apiDescriptionModel = ApplicationApiDescriptionModel.Create();
        apiDescriptionModel.AddModule(moduleDefinition);
        return apiDescriptionModel;
    }

    protected abstract ServiceType? GetDefaultServiceType(GenerateProxyArgs args);

    protected string GetLoggerOutputPath(string path, string workDirectory)
    {
        return path.Replace(workDirectory, string.Empty).TrimStart(Path.DirectorySeparatorChar);
    }
}
