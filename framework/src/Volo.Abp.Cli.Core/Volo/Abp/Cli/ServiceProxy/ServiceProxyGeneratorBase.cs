using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ServiceProxy
{
    public abstract class ServiceProxyGeneratorBase<T> : IServiceProxyGenerator where T: IServiceProxyGenerator
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

            if (!apiDefinition.Modules.TryGetValue(args.Module, out var moduleDefinition))
            {
                throw new CliUsageException($"Module name: {args.Module} is invalid");
            }

            var apiDescriptionModel = ApplicationApiDescriptionModel.Create();
            apiDescriptionModel.AddModule(moduleDefinition);

            return apiDescriptionModel;
        }
    }
}
