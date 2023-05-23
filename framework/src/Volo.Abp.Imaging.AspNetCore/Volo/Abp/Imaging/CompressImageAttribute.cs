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
    
    protected IImageCompressor ImageCompressor { get; set; } // TODO: Pass parameter instead of storing on a property

    public CompressImageAttribute(params string[] parameters)
    {
        Parameters = parameters;
    }

    public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var parameters = Parameters.Any()
            ? context.ActionArguments.Where(x => Parameters.Contains(x.Key)).ToArray()
            : context.ActionArguments.ToArray();
        
        ImageCompressor = context.HttpContext.RequestServices.GetRequiredService<IImageCompressor>();

        foreach (var (key, value) in parameters)
        {
            object compressedValue = value switch {
                IFormFile file => await CompressImageAsync(file),
                IRemoteStreamContent remoteStreamContent => await CompressImageAsync(remoteStreamContent),
                Stream stream => await CompressImageAsync(stream),
                IEnumerable<byte> bytes => await CompressImageAsync(bytes.ToArray()),
                _ => null
            };

            if (compressedValue != null)
            {
                context.ActionArguments[key] = compressedValue;
            }
        }

        await next();
    }
    
    protected async Task<IFormFile> CompressImageAsync(IFormFile file)
    {
        if(file.ContentType == null || !file.ContentType.StartsWith("image/"))
        {
            return file;
        }

        var result = await ImageCompressor.CompressAsync(file.OpenReadStream(), file.ContentType);
        
        if (result.IsSuccess)
        {
            return new FormFile(result.Result, 0, result.Result.Length, file.Name, file.FileName);
        }

        return file;
    }
    
    protected async Task<IRemoteStreamContent> CompressImageAsync(IRemoteStreamContent remoteStreamContent)
    {
        if(remoteStreamContent.ContentType == null || !remoteStreamContent.ContentType.StartsWith("image/"))
        {
            return remoteStreamContent;
        }
        
        var result = await ImageCompressor.CompressAsync(remoteStreamContent.GetStream(), remoteStreamContent.ContentType);
        
        if (result.IsSuccess)
        {
            var fileName = remoteStreamContent.FileName;
            var contentType = remoteStreamContent.ContentType;
            remoteStreamContent.Dispose();
            return new RemoteStreamContent(result.Result, fileName, contentType);
        }

        return remoteStreamContent;
    }
    
    protected async Task<Stream> CompressImageAsync(Stream stream)
    {
        var result = await ImageCompressor.CompressAsync(stream);
        
        if (result.IsSuccess)
        {
            await stream.DisposeAsync();
            return result.Result;
        }

        return stream;
    }
    
    protected async Task<byte[]> CompressImageAsync(byte[] bytes)
    {
        return (await ImageCompressor.CompressAsync(bytes)).Result;
    }
}