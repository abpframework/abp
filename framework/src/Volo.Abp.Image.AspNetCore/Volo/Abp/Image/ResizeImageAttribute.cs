using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Content;

namespace Volo.Abp.Image;

public class ResizeImageAttribute : ActionFilterAttribute
{
    public int? Width { get; }
    public int? Height { get; }
    
    public ImageResizeMode Mode { get; set; }
    public string[] Parameters { get; }
    
    protected IImageResizer ImageResizer { get; set; }
    
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
        
        ImageResizer = context.HttpContext.RequestServices.GetRequiredService<IImageResizer>();
        
        foreach (var (key, value) in parameters)
        {
            object resizedValue = value switch {
                IFormFile file => await ResizeImageAsync(file),
                IRemoteStreamContent remoteStreamContent => await ResizeImageAsync(remoteStreamContent),
                Stream stream => await ResizeImageAsync(stream),
                IEnumerable<byte> bytes => await ResizeImageAsync(bytes.ToArray()),
                _ => null
            };

            if (resizedValue != null)
            {
                context.ActionArguments[key] = resizedValue;
            }
        }

        await next();
    }
    
    protected async Task<IFormFile> ResizeImageAsync(IFormFile file)
    {
        if(file.ContentType == null || !file.ContentType.StartsWith("image/"))
        {
            return file;
        }
        
        var result = await ImageResizer.ResizeAsync(file.OpenReadStream(), new ImageResizeArgs(Width, Height, Mode), file.ContentType);
        
        if (result.IsSuccess)
        {
            return new FormFile(result.Result, 0, result.Result.Length, file.Name, file.FileName);
        }

        return file;
    }
    
    protected async Task<IRemoteStreamContent> ResizeImageAsync(IRemoteStreamContent remoteStreamContent)
    {
        if(remoteStreamContent.ContentType == null || !remoteStreamContent.ContentType.StartsWith("image/"))
        {
            return remoteStreamContent;
        }
        
        var result = await ImageResizer.ResizeAsync(remoteStreamContent.GetStream(), new ImageResizeArgs(Width, Height, Mode), remoteStreamContent.ContentType);
        
        if (result.IsSuccess)
        {
            var fileName = remoteStreamContent.FileName;
            var contentType = remoteStreamContent.ContentType;
            remoteStreamContent.Dispose();
            return new RemoteStreamContent(result.Result, fileName, contentType);
        }

        return remoteStreamContent;
    }
    
    protected async Task<Stream> ResizeImageAsync(Stream stream)
    {
        var result = await ImageResizer.ResizeAsync(stream, new ImageResizeArgs(Width, Height, Mode));
        
        if (result.IsSuccess)
        {
            await stream.DisposeAsync();
            return result.Result;
        }

        return stream;
    }
    
    protected async Task<byte[]> ResizeImageAsync(byte[] bytes)
    {
        return (await ImageResizer.ResizeAsync(bytes, new ImageResizeArgs(Width, Height, Mode))).Result;
    }
}