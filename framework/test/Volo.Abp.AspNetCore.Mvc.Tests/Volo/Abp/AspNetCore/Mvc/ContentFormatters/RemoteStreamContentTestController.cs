using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters
{
    [Route("api/remote-stream-content-test")]
    public class RemoteStreamContentTestController : AbpController
    {
        [HttpGet]
        [Route("Download")]
        public async Task<IRemoteStreamContent> DownloadAsync()
        {
            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("stream"));

            return new RemoteStreamContent(memoryStream)
            {
                ContentType = "audio/mpeg"
            };
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<string> UploadAsync([FromBody]IRemoteStreamContent streamContent)
        {
            using (var reader = new StreamReader(streamContent.GetStream()))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
