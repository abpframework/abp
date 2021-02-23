using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters
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
            var remoteStream = (IRemoteStreamContent)context.Object;

            if (remoteStream != null)
            {
                context.HttpContext.Response.ContentType = remoteStream.ContentType;

                using (var stream = remoteStream.GetStream())
                {
                    stream.Position = 0;
                    await stream.CopyToAsync(context.HttpContext.Response.Body);
                }
            }
        }
    }
}
