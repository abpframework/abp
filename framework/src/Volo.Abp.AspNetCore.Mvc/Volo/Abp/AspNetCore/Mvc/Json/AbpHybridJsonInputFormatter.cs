using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public class AbpHybridJsonInputFormatter : TextInputFormatter, IInputFormatterExceptionPolicy
    {
        public AbpHybridJsonInputFormatter()
        {
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationAnyJsonSyntax);
        }

        public async override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            return await GetTextInputFormatter(context).ReadRequestBodyAsync(context, encoding);
        }

        protected virtual TextInputFormatter GetTextInputFormatter(InputFormatterContext context)
        {
            var typesMatcher = context.HttpContext.RequestServices.GetRequiredService<AbpSystemTextJsonUnsupportedTypeMatcher>();
            if (!typesMatcher.Match(context.ModelType))
            {
                return context.HttpContext.RequestServices.GetRequiredService<SystemTextJsonInputFormatter>();
            }

            return context.HttpContext.RequestServices.GetRequiredService<NewtonsoftJsonInputFormatter>();
        }

        public virtual InputFormatterExceptionPolicy ExceptionPolicy => InputFormatterExceptionPolicy.MalformedInputExceptions;
    }
}
