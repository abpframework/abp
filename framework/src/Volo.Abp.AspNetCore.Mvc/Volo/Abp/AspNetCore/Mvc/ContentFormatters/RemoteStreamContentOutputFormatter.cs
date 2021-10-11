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

                if (!remoteStream.FileName.IsNullOrWhiteSpace())
                {
                    var contentDisposition = new ContentDispositionHeaderValue("attachment");
                    contentDisposition.SetHttpFileName(remoteStream.FileName);
                    context.HttpContext.Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
                }

                using (remoteStream)
                {
                    await remoteStream.GetStream().CopyToAsync(context.HttpContext.Response.Body);
                }
            }
        }
    }
}
