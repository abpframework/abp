using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Content;

namespace Volo.Abp.Image;

public static class IRemoteStreamContentExtensions
{
    public async static Task<IRemoteStreamContent> CompressImageAsync(this IRemoteStreamContent remoteStreamContent, IImageFormat imageFormat, IImageCompressor imageCompressor, CancellationToken cancellationToken = default)
    {
        if (!imageCompressor.CanCompress(imageFormat))
        {
            return remoteStreamContent;
        }

        var compressedImageStream = await remoteStreamContent.GetStream().CompressImageAsync(imageFormat, imageCompressor, cancellationToken);
        
        return new RemoteStreamContent(compressedImageStream, remoteStreamContent.FileName, remoteStreamContent.ContentType);
    }
    
    public async static Task<IRemoteStreamContent> CompressImageAsync(this IRemoteStreamContent remoteStreamContent, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var imageFormatDetector = serviceProvider.GetRequiredService<IImageFormatDetector>();
        var imageCompressorSelector = serviceProvider.GetRequiredService<IImageCompressorSelector>();
        var format = imageFormatDetector.FindFormat(remoteStreamContent.GetStream());
        return await remoteStreamContent.CompressImageAsync(format, imageCompressorSelector.FindCompressor(format), cancellationToken);
    }
    
    public async static Task<IRemoteStreamContent> ResizeImageAsync(this IRemoteStreamContent remoteStreamContent, IImageResizeParameter imageResizeParameter, IImageFormat imageFormat, IImageResizer imageResizer, CancellationToken cancellationToken = default)
    {
        if (!imageResizer.CanResize(imageFormat))
        {
            return remoteStreamContent;
        }

        var resizedImageStream = await remoteStreamContent.GetStream().ResizeImageAsync(imageResizeParameter, imageFormat, imageResizer, cancellationToken);
        
        return new RemoteStreamContent(resizedImageStream, remoteStreamContent.FileName, remoteStreamContent.ContentType);
    }
    
    public async static Task<IRemoteStreamContent> ResizeImageAsync(this IRemoteStreamContent remoteStreamContent, IImageResizeParameter imageResizeParameter, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var imageFormatDetector = serviceProvider.GetRequiredService<IImageFormatDetector>();
        var imageResizerSelector = serviceProvider.GetRequiredService<IImageResizerSelector>();
        var format = imageFormatDetector.FindFormat(remoteStreamContent.GetStream());
        return await remoteStreamContent.ResizeImageAsync(imageResizeParameter, format, imageResizerSelector.FindResizer(format), cancellationToken);
    }
}