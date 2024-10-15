using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters;

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
        return new RemoteStreamContent(memoryStream, "download.rtf", "application/rtf");
    }
    
    [HttpGet]
    [Route("Download-With-Custom-Content-Disposition")]
    public async Task<IRemoteStreamContent> Download_With_Custom_Content_Disposition_Async()
    {
        var memoryStream = new MemoryStream();
        await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("DownloadAsync"));
        memoryStream.Position = 0;
        Response.Headers.Append("Content-Disposition", "attachment; filename=myDownload.rtf");
        return new RemoteStreamContent(memoryStream, "download.rtf", "application/rtf");
    }
    
    [HttpGet]
    [Route("Download_With_Chinese_File_Name")]
    public async Task<IRemoteStreamContent> Download_With_Chinese_File_Name_Async()
    {
        var memoryStream = new MemoryStream();
        await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("DownloadAsync"));
        memoryStream.Position = 0;
        return new RemoteStreamContent(memoryStream, "下载文件.rtf", "application/rtf");
    }

    [HttpPost]
    [Route("Upload")]
    public async Task<string> UploadAsync(IRemoteStreamContent file)
    {
        using (var reader = new StreamReader(file.GetStream()))
        {
            return await reader.ReadToEndAsync() + ":" + file.ContentType + ":" + file.FileName;
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
