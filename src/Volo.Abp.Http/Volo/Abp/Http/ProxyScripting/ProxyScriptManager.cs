using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Configuration;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;

namespace Volo.Abp.Http.ProxyScripting
{
    public class ProxyScriptManager : IProxyScriptManager, ISingletonDependency
    {
        private readonly IApiDescriptionModelProvider _modelProvider;
        private readonly AbpApiProxyScriptingOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IJsonSerializer _jsonSerializer;

        private readonly ConcurrentDictionary<string, string> _cache;

        public ProxyScriptManager(
            IApiDescriptionModelProvider modelProvider, 
            IOptions<AbpApiProxyScriptingOptions> options,
            IServiceProvider serviceProvider,
            IJsonSerializer jsonSerializer)
        {
            _modelProvider = modelProvider;
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _jsonSerializer = jsonSerializer;

            _cache = new ConcurrentDictionary<string, string>();
        }

        public string GetScript(ProxyScriptingModel scriptingModel)
        {
            if (scriptingModel.UseCache)
            {
                return _cache.GetOrAdd(CreateCacheKey(scriptingModel), (key) => CreateScript(scriptingModel));
            }

            return _cache[CreateCacheKey(scriptingModel)] = CreateScript(scriptingModel);
        }

        private string CreateScript(ProxyScriptingModel scriptingModel)
        {
            var apiModel = _modelProvider.CreateApiModel();

            if (scriptingModel.IsPartialRequest())
            {
                apiModel = apiModel.CreateSubModel(scriptingModel.Modules, scriptingModel.Controllers, scriptingModel.Actions);
            }

            var generatorType = _options.Generators.GetOrDefault(scriptingModel.GeneratorType);
            if (generatorType == null)
            {
                throw new AbpException($"Could not find a proxy script generator with given name: {scriptingModel.GeneratorType}");
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService(generatorType).As<IProxyScriptGenerator>().CreateScript(apiModel);
            }
        }

        private string CreateCacheKey(ProxyScriptingModel model)
        {
            return _jsonSerializer.Serialize(model).ToMd5();
        }
    }
}
