using System;
using System.Buffers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Json;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.NewtonsoftJson;

public class AbpNewtonsoftJsonFormatter : IAbpHybridJsonInputFormatter, IAbpHybridJsonOutputFormatter, ITransientDependency
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ArrayPool<char> _charPool;
    private readonly ObjectPoolProvider _objectPoolProvider;
    private readonly IOptions<MvcOptions> _mvcOptions;
    private readonly IOptions<MvcNewtonsoftJsonOptions> _mvcNewtonsoftJsonOptions;

    public AbpNewtonsoftJsonFormatter(
        ILoggerFactory loggerFactory,
        ArrayPool<char> charPool,
        ObjectPoolProvider objectPoolProvider,
        IOptions<MvcOptions> mvcOptions,
        IOptions<MvcNewtonsoftJsonOptions> mvcNewtonsoftJsonOptions)
    {
        _loggerFactory = loggerFactory;
        _charPool = charPool;
        _objectPoolProvider = objectPoolProvider;
        _mvcOptions = mvcOptions;
        _mvcNewtonsoftJsonOptions = mvcNewtonsoftJsonOptions;
    }

    Task<bool> IAbpHybridJsonInputFormatter.CanHandleAsync(Type type)
    {
        return Task.FromResult(true);
    }

    public Task<TextInputFormatter> GetTextInputFormatterAsync()
    {
        return Task.FromResult<TextInputFormatter>(new NewtonsoftJsonInputFormatter(
            _loggerFactory.CreateLogger<NewtonsoftJsonInputFormatter>(),
            _mvcNewtonsoftJsonOptions.Value.SerializerSettings,
            _charPool,
            _objectPoolProvider,
            _mvcOptions.Value,
            _mvcNewtonsoftJsonOptions.Value));
    }

    Task<bool> IAbpHybridJsonOutputFormatter.CanHandleAsync(Type type)
    {
        return Task.FromResult(true);
    }

    public Task<TextOutputFormatter> GetTextOutputFormatterAsync()
    {
        return Task.FromResult<TextOutputFormatter>(new NewtonsoftJsonOutputFormatter(
            _mvcNewtonsoftJsonOptions.Value.SerializerSettings,
            _charPool,
            _mvcOptions.Value,
            _mvcNewtonsoftJsonOptions.Value));
    }
}
