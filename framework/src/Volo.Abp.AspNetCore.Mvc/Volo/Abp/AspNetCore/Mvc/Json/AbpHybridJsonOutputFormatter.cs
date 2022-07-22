using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.Json;

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
        await (await GetTextInputFormatter(context)).WriteResponseBodyAsync(context, selectedEncoding);
    }

    protected virtual async Task<TextOutputFormatter> GetTextInputFormatter(OutputFormatterWriteContext context)
    {
        var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<AbpHybridJsonFormatterOptions>>().Value;

        foreach (var outputFormatterType in options.TextOutputFormatters)
        {
            var outputFormatter = context.HttpContext.RequestServices.GetRequiredService(outputFormatterType).As<IAbpHybridJsonOutputFormatter>();
            if (await outputFormatter.CanHandleAsync(context.ObjectType))
            {
                return await outputFormatter.GetTextOutputFormatterAsync();
            }
        }

        throw new AbpException($"The {nameof(AbpHybridJsonOutputFormatter)} can't handle '{context.ObjectType.GetFullNameWithAssemblyName()}'!");
    }
}
