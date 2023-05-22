using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Imaging;

public class ImageResizer : IImageResizer, ITransientDependency
{
    protected IEnumerable<IImageResizerContributor> ImageResizerContributors { get; }
    
    protected ImageResizeOptions ImageResizeOptions { get; }
    
    public ImageResizer(IEnumerable<IImageResizerContributor> imageResizerContributors, IOptions<ImageResizeOptions> imageResizeOptions)
    {
        ImageResizerContributors = imageResizerContributors;
        ImageResizeOptions = imageResizeOptions.Value;
    }
    
    public async Task<ImageProcessResult<Stream>> ResizeAsync(Stream stream, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default)
    {
        ChangeDefaultResizeMode(resizeArgs);
        
        foreach (var imageResizerContributor in ImageResizerContributors)
        {
            var result = await imageResizerContributor.TryResizeAsync(stream, resizeArgs, mimeType, cancellationToken);
            if (!result.IsSupported)
            {
                continue;
            }

            if (result.Exception != null)
            {
                throw result.Exception;
            }
            
            if (result.IsSuccess)
            {
                return new ImageProcessResult<Stream>(result.Result, result.IsSuccess);
            }
            
            return new ImageProcessResult<Stream>(stream, false);
        }
        
        return new ImageProcessResult<Stream>(stream, false);
    }

    public async Task<ImageProcessResult<byte[]>> ResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default)
    {
        ChangeDefaultResizeMode(resizeArgs);
        
        foreach (var imageResizerContributor in ImageResizerContributors)
        {
            var result = await imageResizerContributor.TryResizeAsync(bytes, resizeArgs, mimeType, cancellationToken);
            if (!result.IsSupported)
            {
                continue;
            }

            if (result.Exception != null)
            {
                throw result.Exception;
            }
            
            if (result.IsSuccess)
            {
                return new ImageProcessResult<byte[]>(result.Result, result.IsSuccess);
            }
            
            return new ImageProcessResult<byte[]>(bytes, false);
        }
        
        return new ImageProcessResult<byte[]>(bytes, false);
    }
    
    protected virtual void ChangeDefaultResizeMode(ImageResizeArgs resizeArgs)
    {
        if (resizeArgs.Mode == ImageResizeMode.Default)
        {
            resizeArgs.Mode = ImageResizeOptions.DefaultResizeMode;
        }
    }
}