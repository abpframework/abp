using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Imaging;

public class ImageResizer : IImageResizer, ITransientDependency
{
    protected IEnumerable<IImageResizerContributor> ImageResizerContributors { get; }
    
    protected ImageResizeOptions ImageResizeOptions { get; }
    
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    
    public ImageResizer(
        IEnumerable<IImageResizerContributor> imageResizerContributors, 
        IOptions<ImageResizeOptions> imageResizeOptions, 
        ICancellationTokenProvider cancellationTokenProvider)
    {
        ImageResizerContributors = imageResizerContributors.Reverse();
        CancellationTokenProvider = cancellationTokenProvider;
        ImageResizeOptions = imageResizeOptions.Value;
    }
    
    public virtual async Task<ImageResizeResult<Stream>> ResizeAsync(
        [NotNull] Stream stream, 
        ImageResizeArgs resizeArgs, 
        string? mimeType = null, 
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(stream, nameof(stream));
        
        ChangeDefaultResizeMode(resizeArgs);
        
        if(!stream.CanRead)
        {
            return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
        }
        
        if(!stream.CanSeek)
        {
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, CancellationTokenProvider.FallbackToProvider(cancellationToken));
            SeekToBegin(memoryStream);
            stream = memoryStream;
        }
        
        foreach (var imageResizerContributor in ImageResizerContributors)
        {
            var result = await imageResizerContributor.TryResizeAsync(stream, resizeArgs, mimeType, CancellationTokenProvider.FallbackToProvider(cancellationToken));

            SeekToBegin(stream);
            
            if (result.State == ImageProcessState.Unsupported)
            {
                continue;
            }

            return result;
        }
        
        return new ImageResizeResult<Stream>(stream, ImageProcessState.Unsupported);
    }

    public virtual async Task<ImageResizeResult<byte[]>> ResizeAsync(
        [NotNull] byte[] bytes, 
        ImageResizeArgs resizeArgs, 
        string? mimeType = null, 
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(bytes, nameof(bytes));
        
        ChangeDefaultResizeMode(resizeArgs);
        
        foreach (var imageResizerContributor in ImageResizerContributors)
        {
            var result = await imageResizerContributor.TryResizeAsync(bytes, resizeArgs, mimeType, CancellationTokenProvider.FallbackToProvider(cancellationToken));
            
            if (result.State == ImageProcessState.Unsupported)
            {
                continue;
            }

            return result;
        }
        
        return new ImageResizeResult<byte[]>(bytes, ImageProcessState.Unsupported);
    }
    
    protected virtual void ChangeDefaultResizeMode(ImageResizeArgs resizeArgs)
    {
        if (resizeArgs.Mode == ImageResizeMode.Default)
        {
            resizeArgs.Mode = ImageResizeOptions.DefaultResizeMode;
        }
    }
    
    protected virtual void SeekToBegin(Stream stream)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }
    }
}