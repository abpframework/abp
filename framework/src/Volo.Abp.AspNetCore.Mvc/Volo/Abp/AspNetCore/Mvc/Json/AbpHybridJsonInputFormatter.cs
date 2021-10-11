using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public class AbpHybridJsonInputFormatter : TextInputFormatter, IInputFormatterExceptionPolicy
    {
        private readonly SystemTextJsonInputFormatter _systemTextJsonInputFormatter;
        private readonly NewtonsoftJsonInputFormatter _newtonsoftJsonInputFormatter;

        public AbpHybridJsonInputFormatter(SystemTextJsonInputFormatter systemTextJsonInputFormatter, NewtonsoftJsonInputFormatter newtonsoftJsonInputFormatter)
        {
            _systemTextJsonInputFormatter = systemTextJsonInputFormatter;
            _newtonsoftJsonInputFormatter = newtonsoftJsonInputFormatter;

            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationAnyJsonSyntax);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            return await GetTextInputFormatter(context).ReadRequestBodyAsync(context, encoding);
        }

        protected virtual TextInputFormatter GetTextInputFormatter(InputFormatterContext context)
        {
            var typesMatcher = context.HttpContext.RequestServices.GetRequiredService<AbpSystemTextJsonUnsupportedTypeMatcher>();

            if (!typesMatcher.Match(context.ModelType))
            {
                return _systemTextJsonInputFormatter;
            }

            return _newtonsoftJsonInputFormatter;
        }

        public virtual InputFormatterExceptionPolicy ExceptionPolicy => InputFormatterExceptionPolicy.MalformedInputExceptions;
    }
}
