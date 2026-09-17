using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Shouldly;
using Volo.Abp.Content;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters;

public class RemoteStreamContentOutputFormatter_Tests
{
    [Fact]
    public async Task Should_Not_Copy_The_Stream_When_The_Request_Is_Aborted()
    {
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            await cancellationTokenSource.CancelAsync();

            var httpContext = new DefaultHttpContext();
            httpContext.RequestAborted = cancellationTokenSource.Token;
            httpContext.Response.Body = new MemoryStream();

            var writeContext = new OutputFormatterWriteContext(
                httpContext,
                (stream, encoding) => new StreamWriter(stream, encoding),
                typeof(IRemoteStreamContent),
                new RemoteStreamContent(
                    new MemoryStream(Encoding.UTF8.GetBytes("DownloadAsync")),
                    "download.rtf",
                    "application/rtf"));

            await new RemoteStreamContentOutputFormatter().WriteResponseBodyAsync(writeContext);

            httpContext.Response.Body.Length.ShouldBe(0);
        }
    }
}
