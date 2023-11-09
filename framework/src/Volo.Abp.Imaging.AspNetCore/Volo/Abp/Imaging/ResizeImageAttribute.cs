using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Content;

namespace Volo.Abp.Imaging;

public class ResizeImageAttribute : ActionFilterAttribute
{
    public int? Width { get; }
    public int? Height { get; }
    
    public ImageResizeMode Mode { get; set; }
    public string[] Parameters { get; }
    
    public ResizeImageAttribute(int width, int height, params string[] parameters)
    {
        Width = width;
        Height = height;
        Parameters = parameters;
    }
    
    public ResizeImageAttribute(int size, params string[] parameters)
    {
        Width = size;
        Height = size;
        Parameters = parameters;
    }

    public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var parameters = Parameters.Any()
            ? context.ActionArguments.Where(x => Parameters.Contains(x.Key)).ToArray()
            : context.ActionArguments.ToArray();
        
        var imageResizer = context.HttpContext.RequestServices.GetRequiredService<IImageResizer>();
        
        foreach (var (key, value) in parameters)
        {
            object? resizedValue = value switch {
                IFormFile file => await ResizeImageAsync(file, imageResizer),
                IRemoteStreamContent remoteStreamContent => await ResizeImageAsync(remoteStreamContent, imageResizer),
                Stream stream => await ResizeImageAsync(stream, imageResizer),
                IEnumerable<byte> bytes => await ResizeImageAsync(bytes.ToArray(), imageResizer),
                _ => null
            };

            if (resizedValue != null)
            {
                context.ActionArguments[key] = resizedValue;
            }
        }

        await next();
    }
    
    protected virtual async Task<IFormFile> ResizeImageAsync(IFormFile file, IImageResizer imageResizer)
    {
        if(file.Headers == null ||  file.ContentType == null || !file.ContentType.StartsWith("image/"))
        {
            return file;
        }
        
        var result = await imageResizer.ResizeAsync(file.OpenReadStream(), new ImageResizeArgs(Width, Height, Mode), file.ContentType);

        if (result.State != ImageProcessState.Done)
        {
            return file;
        }

        return new FormFile(result.Result, 0, result.Result.Length, file.Name, file.FileName) {
            Headers = file.Headers
        };
    }
    
    protected virtual async Task<IRemoteStreamContent> ResizeImageAsync(IRemoteStreamContent remoteStreamContent, IImageResizer imageResizer)
    {
        if(remoteStreamContent.ContentType == null || !remoteStreamContent.ContentType.StartsWith("image/"))
        {
            return remoteStreamContent;
        }
        
        var result = await imageResizer.ResizeAsync(remoteStreamContent.GetStream(), new ImageResizeArgs(Width, Height, Mode), remoteStreamContent.ContentType);

        if (result.State != ImageProcessState.Done)
        {
            return remoteStreamContent;
        }

        var fileName = remoteStreamContent.FileName;
        var contentType = remoteStreamContent.ContentType;
        remoteStreamContent.Dispose();
        return new RemoteStreamContent(result.Result, fileName, contentType);
    }
    
    protected virtual async Task<Stream> ResizeImageAsync(Stream stream, IImageResizer imageResizer)
    {
        var result = await imageResizer.ResizeAsync(stream, new ImageResizeArgs(Width, Height, Mode));

        if (result.State != ImageProcessState.Done)
        {
            return stream;
        }

        await stream.DisposeAsync();
        return result.Result;
    }
    
    protected virtual async Task<byte[]> ResizeImageAsync(byte[] bytes, IImageResizer imageResizer)
    {
        return (await imageResizer.ResizeAsync(bytes, new ImageResizeArgs(Width, Height, Mode))).Result;
    }
}