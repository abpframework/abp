using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.AspNetCore.Mvc.Json;

public class AbpSystemTextJsonFormatter : IAbpHybridJsonInputFormatter, IAbpHybridJsonOutputFormatter, ITransientDependency
{
    private readonly AbpSystemTextJsonUnsupportedTypeMatcher _unsupportedTypeMatcher;
    private readonly IOptions<JsonOptions> _jsonOptions;
    private readonly ILoggerFactory _loggerFactory;

    public AbpSystemTextJsonFormatter(AbpSystemTextJsonUnsupportedTypeMatcher unsupportedTypeMatcher,
        IOptions<JsonOptions> jsonOptions,
        ILoggerFactory loggerFactory)
    {
        _unsupportedTypeMatcher = unsupportedTypeMatcher;
        _jsonOptions = jsonOptions;
        _loggerFactory = loggerFactory;
    }

    Task<bool> IAbpHybridJsonInputFormatter.CanHandleAsync(Type type)
    {
        return Task.FromResult(!_unsupportedTypeMatcher.Match(type));
    }

    public virtual Task<TextInputFormatter> GetTextInputFormatterAsync()
    {
        return Task.FromResult<TextInputFormatter>(new SystemTextJsonInputFormatter(
            _jsonOptions.Value,
            _loggerFactory.CreateLogger<SystemTextJsonInputFormatter>()));
    }

    Task<bool> IAbpHybridJsonOutputFormatter.CanHandleAsync(Type type)
    {
        return Task.FromResult(!_unsupportedTypeMatcher.Match(type));
    }

    public Task<TextOutputFormatter> GetTextOutputFormatterAsync()
    {
        var jsonSerializerOptions = _jsonOptions.Value.JsonSerializerOptions;
        if (jsonSerializerOptions.Encoder is null)
        {
            // If the user hasn't explicitly configured the encoder, use the less strict encoder that does not encode all non-ASCII characters.
            jsonSerializerOptions = new JsonSerializerOptions(jsonSerializerOptions)
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
        }

        return Task.FromResult<TextOutputFormatter>(new SystemTextJsonOutputFormatter(jsonSerializerOptions));
    }
}
