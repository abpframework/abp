using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public class AbpHybridJsonOutputFormatter : TextOutputFormatter
    {
        private readonly SystemTextJsonOutputFormatter _systemTextJsonOutputFormatter;
        private readonly NewtonsoftJsonOutputFormatter _newtonsoftJsonOutputFormatter;

        public AbpHybridJsonOutputFormatter(SystemTextJsonOutputFormatter systemTextJsonOutputFormatter, NewtonsoftJsonOutputFormatter newtonsoftJsonOutputFormatter)
        {
            _systemTextJsonOutputFormatter = systemTextJsonOutputFormatter;
            _newtonsoftJsonOutputFormatter = newtonsoftJsonOutputFormatter;

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationAnyJsonSyntax);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            await GetTextInputFormatter(context).WriteResponseBodyAsync(context, selectedEncoding);
        }

        protected virtual TextOutputFormatter GetTextInputFormatter(OutputFormatterWriteContext context)
        {
            var typesMatcher = context.HttpContext.RequestServices.GetRequiredService<AbpSystemTextJsonUnsupportedTypeMatcher>();
            if (!typesMatcher.Match(context.ObjectType))
            {
                return _systemTextJsonOutputFormatter;
            }

            return _newtonsoftJsonOutputFormatter;
        }
    }
}
