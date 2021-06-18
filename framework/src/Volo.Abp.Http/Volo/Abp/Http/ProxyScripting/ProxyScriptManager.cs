using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Configuration;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;
using Volo.Abp.Minify.Scripts;

namespace Volo.Abp.Http.ProxyScripting
{
    public class ProxyScriptManager : IProxyScriptManager, ITransientDependency
    {
        private readonly IApiDescriptionModelProvider _modelProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IProxyScriptManagerCache _cache;
        private readonly AbpApiProxyScriptingOptions _options;

        public ProxyScriptManager(
            IApiDescriptionModelProvider modelProvider,
            IServiceProvider serviceProvider,
            IJsonSerializer jsonSerializer,
            IProxyScriptManagerCache cache,
            IOptions<AbpApiProxyScriptingOptions> options)
        {
            _modelProvider = modelProvider;
            _serviceProvider = serviceProvider;
            _jsonSerializer = jsonSerializer;
            _cache = cache;
            _options = options.Value;
        }

        public string GetScript(ProxyScriptingModel scriptingModel)
        {
            var cacheKey = CreateCacheKey(scriptingModel);

            if (scriptingModel.UseCache)
            {
                return _cache.GetOrAdd(cacheKey, () => CreateScript(scriptingModel));
            }

            var script = CreateScript(scriptingModel);
            _cache.Set(cacheKey, script);
            return script;
        }

        private string CreateScript(ProxyScriptingModel scriptingModel)
        {
            var apiModel = _modelProvider.CreateApiModel(new ApplicationApiDescriptionModelRequestDto {IncludeTypes = false});

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
            return _jsonSerializer.Serialize(new
            {
                model.GeneratorType,
                model.Modules,
                model.Controllers,
                model.Actions,
                model.Properties
            }).ToMd5();
        }
    }
}
