using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.Json;

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
        return await (await GetTextInputFormatterAsync(context)).ReadRequestBodyAsync(context, encoding);
    }

    protected virtual async Task<TextInputFormatter> GetTextInputFormatterAsync(InputFormatterContext context)
    {
        var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<AbpHybridJsonFormatterOptions>>().Value;

        foreach (var inputFormatterType in options.TextInputFormatters)
        {
            var inputFormatter = context.HttpContext.RequestServices.GetRequiredService(inputFormatterType).As<IAbpHybridJsonInputFormatter>();
            if (await inputFormatter.CanHandleAsync(context.ModelType))
            {
                return await inputFormatter.GetTextInputFormatterAsync();
            }
        }

        throw new AbpException($"The {nameof(AbpHybridJsonInputFormatter)} can't handle '{context.ModelType.GetFullNameWithAssemblyName()}'!");
    }

    public virtual InputFormatterExceptionPolicy ExceptionPolicy => InputFormatterExceptionPolicy.MalformedInputExceptions;
}
