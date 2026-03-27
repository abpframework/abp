using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Configuration;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;

namespace Volo.Abp.Http.ProxyScripting;

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

    public async Task<string> GetScriptAsync(ProxyScriptingModel scriptingModel)
    {
        var cacheKey = CreateCacheKey(scriptingModel);

        if (scriptingModel.UseCache)
        {
            return await _cache.GetOrAddAsync(cacheKey, () => CreateScriptAsync(scriptingModel));
        }

        return await CreateScriptAsync(scriptingModel);
    }

    private async Task<string> CreateScriptAsync(ProxyScriptingModel scriptingModel)
    {
        var apiModel = await _modelProvider.CreateApiModelAsync(new ApplicationApiDescriptionModelRequestDto { IncludeTypes = false });

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
            return scope.ServiceProvider
                .GetRequiredService(generatorType)
                .As<IProxyScriptGenerator>()
                .CreateScript(apiModel);
        }
    }

    private string CreateCacheKey(ProxyScriptingModel model)
    {
        return _jsonSerializer.Serialize(new {
            model.GeneratorType,
            model.Modules,
            model.Controllers,
            model.Actions,
            model.Properties
        }).ToMd5();
    }
}
