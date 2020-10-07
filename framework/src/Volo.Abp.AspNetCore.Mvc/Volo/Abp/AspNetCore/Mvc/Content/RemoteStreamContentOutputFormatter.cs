using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.Content
{
    public class RemoteStreamContentOutputFormatter : OutputFormatter
    {
        public RemoteStreamContentOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("*/*"));
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(IRemoteStreamContent).IsAssignableFrom(type);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var httpContext = context.HttpContext;
            var remoteStream = context.Object as IRemoteStreamContent;
            using (var stream = remoteStream.GetStream())
                await stream.CopyToAsync(httpContext.Response.Body);
        }
    }
}
