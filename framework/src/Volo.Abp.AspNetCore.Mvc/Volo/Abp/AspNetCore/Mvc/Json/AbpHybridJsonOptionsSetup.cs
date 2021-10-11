using System.Buffers;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public class AbpHybridJsonOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly IOptions<JsonOptions> _jsonOptions;
        private readonly IOptions<MvcNewtonsoftJsonOptions> _mvcNewtonsoftJsonOptions;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ArrayPool<char> _charPool;
        private readonly ObjectPoolProvider _objectPoolProvider;

        public AbpHybridJsonOptionsSetup(
            IOptions<JsonOptions> jsonOptions,
            IOptions<MvcNewtonsoftJsonOptions> mvcNewtonsoftJsonOptions,
            ILoggerFactory loggerFactory,
            ArrayPool<char> charPool,
            ObjectPoolProvider objectPoolProvider)
        {
            _jsonOptions = jsonOptions;
            _mvcNewtonsoftJsonOptions = mvcNewtonsoftJsonOptions;
            _loggerFactory = loggerFactory;
            _charPool = charPool;
            _objectPoolProvider = objectPoolProvider;
        }

        public void Configure(MvcOptions options)
        {
            var systemTextJsonInputFormatter = new SystemTextJsonInputFormatter(
                _jsonOptions.Value,
                _loggerFactory.CreateLogger<SystemTextJsonInputFormatter>());

            var newtonsoftJsonInputFormatter = new NewtonsoftJsonInputFormatter(
                _loggerFactory.CreateLogger<NewtonsoftJsonInputFormatter>(),
                _mvcNewtonsoftJsonOptions.Value.SerializerSettings,
                _charPool,
                _objectPoolProvider,
                options,
                _mvcNewtonsoftJsonOptions.Value);

            options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
            options.InputFormatters.RemoveType<NewtonsoftJsonInputFormatter>();
            options.InputFormatters.Add(new AbpHybridJsonInputFormatter(systemTextJsonInputFormatter, newtonsoftJsonInputFormatter));

            var jsonSerializerOptions = _jsonOptions.Value.JsonSerializerOptions;
            if (jsonSerializerOptions.Encoder is null)
            {
                // If the user hasn't explicitly configured the encoder, use the less strict encoder that does not encode all non-ASCII characters.
                jsonSerializerOptions = new JsonSerializerOptions(jsonSerializerOptions)
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
            }

            var systemTextJsonOutputFormatter = new SystemTextJsonOutputFormatter(jsonSerializerOptions);
            var newtonsoftJsonOutputFormatter = new NewtonsoftJsonOutputFormatter(
                _mvcNewtonsoftJsonOptions.Value.SerializerSettings,
                _charPool,
                options);

            options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
            options.OutputFormatters.RemoveType<NewtonsoftJsonOutputFormatter>();
            options.OutputFormatters.Add(new AbpHybridJsonOutputFormatter(systemTextJsonOutputFormatter, newtonsoftJsonOutputFormatter));
        }
    }
}
