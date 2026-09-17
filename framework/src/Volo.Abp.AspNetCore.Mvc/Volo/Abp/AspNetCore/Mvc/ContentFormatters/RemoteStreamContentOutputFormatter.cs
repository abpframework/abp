using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters;

public class RemoteStreamContentOutputFormatter : OutputFormatter
{
    public RemoteStreamContentOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("*/*"));
    }

    protected override bool CanWriteType(Type? type)
    {
        return typeof(IRemoteStreamContent).IsAssignableFrom(type);
    }

    public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
    {
        var remoteStream = (IRemoteStreamContent?)context.Object;

        if (remoteStream != null)
        {
            context.HttpContext.Response.ContentType = remoteStream.ContentType;
            context.HttpContext.Response.ContentLength = remoteStream.ContentLength;

            if (!remoteStream.FileName.IsNullOrWhiteSpace() && !context.HttpContext.Response.Headers.ContainsKey(HeaderNames.ContentDisposition))
            {
                var contentDisposition = new ContentDispositionHeaderValue("attachment");
                contentDisposition.SetHttpFileName(remoteStream.FileName);
                context.HttpContext.Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            }

            var cancellationToken = context.HttpContext.RequestAborted;

            using (remoteStream)
            {
                var stream = remoteStream.GetStream();

                try
                {
                    await stream.CopyToAsync(context.HttpContext.Response.Body, cancellationToken);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    // The request was aborted, nothing can be written to the response anymore.
                }
            }
        }
    }
}
