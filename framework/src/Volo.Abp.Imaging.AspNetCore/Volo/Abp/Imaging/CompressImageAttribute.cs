using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Content;

namespace Volo.Abp.Imaging;

public class CompressImageAttribute : ActionFilterAttribute
{
    public string[] Parameters { get; }

    public CompressImageAttribute(params string[] parameters)
    {
        Parameters = parameters;
    }

    public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var parameters = Parameters.Any()
            ? context.ActionArguments.Where(x => Parameters.Contains(x.Key)).ToArray()
            : context.ActionArguments.ToArray();
        
        var imageCompressor = context.HttpContext.RequestServices.GetRequiredService<IImageCompressor>();

        foreach (var (key, value) in parameters)
        {
            object? compressedValue = value switch {
                IFormFile file => await CompressImageAsync(file, imageCompressor),
                IRemoteStreamContent remoteStreamContent => await CompressImageAsync(remoteStreamContent, imageCompressor),
                Stream stream => await CompressImageAsync(stream, imageCompressor),
                IEnumerable<byte> bytes => await CompressImageAsync(bytes.ToArray(), imageCompressor),
                _ => null
            };

            if (compressedValue != null)
            {
                context.ActionArguments[key] = compressedValue;
            }
        }

        await next();
    }
    
    protected virtual async Task<IFormFile> CompressImageAsync(IFormFile file, IImageCompressor imageCompressor)
    {
        if(file.Headers == null || file.ContentType == null || !file.ContentType.StartsWith("image/"))
        {
            return file;
        }

        var result = await imageCompressor.CompressAsync(file.OpenReadStream(), file.ContentType);

        if (result.State != ImageProcessState.Done)
        {
            return file;
        }

        return new FormFile(result.Result, 0, result.Result.Length, file.Name, file.FileName) {
            Headers = file.Headers,
        };
    }
    
    protected virtual async Task<IRemoteStreamContent> CompressImageAsync(IRemoteStreamContent remoteStreamContent, IImageCompressor imageCompressor)
    {
        if(remoteStreamContent.ContentType == null || !remoteStreamContent.ContentType.StartsWith("image/"))
        {
            return remoteStreamContent;
        }
        
        var result = await imageCompressor.CompressAsync(remoteStreamContent.GetStream(), remoteStreamContent.ContentType);

        if (result.State != ImageProcessState.Done)
        {
            return remoteStreamContent;
        }

        var fileName = remoteStreamContent.FileName;
        var contentType = remoteStreamContent.ContentType;
        remoteStreamContent.Dispose();
        return new RemoteStreamContent(result.Result, fileName, contentType);
    }
    
    protected virtual async Task<Stream> CompressImageAsync(Stream stream, IImageCompressor imageCompressor)
    {
        var result = await imageCompressor.CompressAsync(stream);

        if (result.State != ImageProcessState.Done)
        {
            return stream;
        }

        await stream.DisposeAsync();
        return result.Result;
    }
    
    protected virtual async Task<byte[]> CompressImageAsync(byte[] bytes, IImageCompressor imageCompressor)
    {
        return (await imageCompressor.CompressAsync(bytes)).Result;
    }
}