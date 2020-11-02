using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public class AbpHybridJsonOutputFormatter : TextOutputFormatter
    {
        public AbpHybridJsonOutputFormatter()
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationAnyJsonSyntax);
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            await GetTextInputFormatter(context).WriteResponseBodyAsync(context, selectedEncoding);
        }

        protected virtual TextOutputFormatter GetTextInputFormatter(OutputFormatterWriteContext context)
        {
            var typesMatcher = context.HttpContext.RequestServices.GetRequiredService<AbpSystemTextJsonUnsupportedTypeMatcher>();
            if (!typesMatcher.Match(context.ObjectType))
            {
                return context.HttpContext.RequestServices.GetRequiredService<SystemTextJsonOutputFormatter>();
            }

            return context.HttpContext.RequestServices.GetRequiredService<NewtonsoftJsonOutputFormatter>();
        }
    }
}
