using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters
{
    public class RemoteStreamContentInputFormatter : InputFormatter
    {
        public RemoteStreamContentInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("*/*"));
        }

        protected override bool CanReadType(Type type)
        {
            return typeof(IRemoteStreamContent) == type;
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            return InputFormatterResult.SuccessAsync(new RemoteStreamContent(context.HttpContext.Request.Body)
            {
                ContentType = context.HttpContext.Request.ContentType
            });
        }
    }
}
