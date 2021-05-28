using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
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
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("DownloadAsync"));
            memoryStream.Position = 0;
            return new RemoteStreamContent(memoryStream, "application/rtf");
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<string> UploadAsync(IRemoteStreamContent file)
        {
            using (var reader = new StreamReader(file.GetStream()))
            {
                return await reader.ReadToEndAsync() + ":" + file.ContentType;
            }
        }

        [HttpPost]
        [Route("Upload-Raw")]
        public async Task<string> UploadRawAsync(IRemoteStreamContent file)
        {
            using (var reader = new StreamReader(file.GetStream()))
            {
                return await reader.ReadToEndAsync() + ":" + file.ContentType;
            }
        }
    }
}
